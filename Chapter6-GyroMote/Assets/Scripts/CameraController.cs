/*A collection of Camera Functions that allows it to Orbit a point.
 * 
 * Create a GameObject with a parent/child structure like this:
 * BaseNode -> AxisNode -> CameraNode -> Camera
 * CameraNode is positioned with z at the Camera's default position.
 * Place this script on the Base node, assign the AxisNode to CameraRigs[0] and the Camera to CameraObject.
 * 
 * Then, in a GameController, call Orbit(), Zoom(), Pan() or Elevate() in an Update loop with the desired input.
 * For example,
 * 
 * void Update(){
 * 	if(Input.GetMouse(0)){
 * 		Orbit(Input.mousePosition);
 * 	}
 * }
 * 
 * Lerping functions also available. Those use MovementCoroutines.cs.
 * 
 * -Wai Kay Kong
 */

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public GameObject CameraObject;
	public GameObject VerticalRig;
	
	public float RotateSensitivity;
	
	[SerializeField]
	private float VerticalMin;
	[SerializeField]
	private float VerticalMax;
	
	public float ZoomSensitivity;
	[SerializeField]
	private float ZoomMin;
	[SerializeField]
	private float ZoomMax;
	
	public float PanSensitivity;
	[SerializeField]
	private float PanMin;
	[SerializeField]
	private float PanMax;
	
	public float ElevateSensitivity;
	[SerializeField]
	private float ElevateMin;
	[SerializeField]
	private float ElevateMax;
	
	private float HorizontalAxis;
	private float VerticalAxis;
	private float ZoomAxis;
	private float ElevateAxis;
	
	//For use with resetting the camera in Orbit Mode
	private float OriginalHorizontalAxis;
	private float OriginalVerticalAxis;
	private float OriginalZoomAxis;
	private float OriginalElevateAxis;
	
	private bool IsMovingInternal;
	public bool IsMoving{
		get{return IsMovingInternal;}
	}
	
	void Awake() {
		HorizontalAxis = transform.eulerAngles.y;
		//In VerticalAxis, the code uses negative values, so the retrieved value should be negative.
		VerticalAxis = VerticalRig.transform.localEulerAngles.x - 360f;
		ZoomAxis = CameraObject.transform.localPosition.z;
		
		//Set original Values
		OriginalHorizontalAxis = HorizontalAxis;
		OriginalVerticalAxis = VerticalAxis;
		OriginalZoomAxis = ZoomAxis;
		OriginalElevateAxis = ElevateAxis;
	}
	
	public void Orbit(Vector2 OrbitInput, bool Override = false){
		if(IsMovingInternal) return;
		//Rotation
		if(OrbitInput != Vector2.zero || Override){
			//Get Mouse Axis Input
			HorizontalAxis += OrbitInput.x * RotateSensitivity * CalculateZoomRatio();
			VerticalAxis += OrbitInput.y * RotateSensitivity * CalculateZoomRatio();
			
			//Keep Horizontal between -360 and 360. Don't use Mathf.Clamp for this so it can keep spinning
			if(HorizontalAxis > 360f) HorizontalAxis -= 360f;
			else if(HorizontalAxis < -360f) HorizontalAxis += 360f;
			//Clamp Veritcal axis to prevent somersaults
			VerticalAxis = Mathf.Clamp(VerticalAxis, VerticalMin, VerticalMax);
					
			//Apply. Vertical axis spins the z axis, horizontal spins the y axis.
			VerticalRig.transform.localEulerAngles = new Vector3(VerticalAxis, 0, 0);
			//Rotate the root. Avoids potential GIMBAL lock issues and makes panning easier.
			transform.eulerAngles = new Vector3(0, HorizontalAxis, 0);
		}
	}
	
	public void Zoom(float ZoomInput, bool Override = false){
		if(IsMovingInternal) return;
		//Zoom
		if(ZoomInput != 0 || Override){
			//Add input
			ZoomAxis -= ZoomInput * ZoomSensitivity;
			//Clamp Zoom axis to setting
			ZoomAxis = Mathf.Clamp(ZoomAxis, ZoomMin, ZoomMax);
			
			//Apply. Zoom is on the x axis because the node is rotated 90 degrees Y.
			CameraObject.transform.localPosition = new Vector3(0, 0, ZoomAxis);
		}
	}

	float CalculateZoomRatio(){
		//Calculates the ratio of which the zoom is at relative to min and max zoom, taking negative numbers into account.
		float zoomLength = ZoomMax - ZoomMin;
		float currentZoomFromMin = ZoomAxis - ZoomMin;

		float ratio = currentZoomFromMin/zoomLength;

		//put a floor on the ratio
		return 0.2f + 0.8f * ratio;
	}
	
	public void Pan(Vector2 PanInput, bool Override = false){
		if(IsMovingInternal) return;
		//Pan
		if(PanInput != Vector2.zero || Override){
			//Get Mouse Values, scale with Zoom Axis
//            float HorizontalInput = PanInput.y * PanSensitivity/Mathf.Clamp((ZoomMax - ZoomAxis),20, 85);
//            float VerticalInput = PanInput.x * PanSensitivity/Mathf.Clamp((ZoomMax - ZoomAxis),20, 85);

			float HorizontalInput = PanInput.y * PanSensitivity * CalculateZoomRatio();
			float VerticalInput = PanInput.x * PanSensitivity * CalculateZoomRatio();

			//Convert to world space. X component is forward because the system is rotated 90 degrees
			Vector3 WorldPoint = transform.TransformPoint(new Vector3(VerticalInput, 0, HorizontalInput));
			
			//Apply, ignore y values to prevent accidental dolly
			transform.position = new Vector3(
				Mathf.Clamp((float)WorldPoint.x, PanMin, PanMax),
				transform.position.y, 
				Mathf.Clamp((float)WorldPoint.z, PanMin, PanMax)
			);
		}
	}
	
     public void Elevate(float UpInput, bool Override = false){
		if(IsMovingInternal) return;
		if(UpInput != 0 || Override){
			//Add Input
			ElevateAxis -= UpInput * ElevateSensitivity * CalculateZoomRatio();
			ElevateAxis = Mathf.Clamp(ElevateAxis, ElevateMin, ElevateMax);
			
			//Go up!
			transform.position = new Vector3(transform.position.x, ElevateAxis, transform.position.z);
		}
	}

	
	#region Tweens
	//Zoom Tween
	public void StartZoomTo(Vector3 Point, float Duration = 0.5f, float ElevationOffset = 0.95f, float ZoomOffset = 10f){
		if(!IsMovingInternal) StopAllCoroutines();
			
		StartCoroutine(ZoomTo(Point + new Vector3(0, ElevationOffset, 0), Duration, ZoomOffset));
	}
	
	IEnumerator ZoomTo(Vector3 Point, float Duration, float ZoomOffset){
		IsMovingInternal = true;

		StartCoroutine(
			MovementCoroutines.MoveLerpTo(gameObject, Point, Duration)	
		);
		StartCoroutine(
			MovementCoroutines.MoveLerpLocalTo(
				CameraObject, 
				new Vector3(0, 0, ZoomMin + ZoomOffset), 
				Duration
			)
		);
		
		yield return new WaitForSeconds(Duration);
		
		ZoomAxis = ZoomMin + ZoomOffset;
		ElevateAxis = transform.position.y;
		IsMovingInternal = false;
	}
	
	//Move Tween
    IEnumerator MoveTo(Vector3 Point, float Duration){
        IsMovingInternal = true;
        
        StartCoroutine(
            MovementCoroutines.MoveLerpTo(gameObject, Point, Duration)  
            );
        
        yield return new WaitForSeconds(Duration);
        IsMovingInternal = false;
    }
	
	//Reset Tween
	public void StartResetCamera(float Duration = 0.5f){
		if(!IsMovingInternal) 
            StopAllCoroutines();
		StartCoroutine(ResetCamera(Duration));
	}
	
	IEnumerator ResetCamera(float Duration, Vector3 newCamPosition = default(Vector3)){
		IsMovingInternal = true;

		StartCoroutine(
            MovementCoroutines.MoveLerpTo(gameObject, newCamPosition, Duration)	
		);
		StartCoroutine(
			MovementCoroutines.MoveLerpLocalTo(
				CameraObject, 
				new Vector3(0, 0, OriginalZoomAxis), 
				Duration
			)
		);
		
		Quaternion OriginalVerticalQuat = Quaternion.Euler(new Vector3(OriginalVerticalAxis, 0, 0));
		Quaternion OriginalHorizontalQuat = Quaternion.Euler(new Vector3(0, OriginalHorizontalAxis, 0));
		
		StartCoroutine(
			MovementCoroutines.RotateLerpLocalTo(VerticalRig, OriginalVerticalQuat, Duration)
		);
		StartCoroutine(
			MovementCoroutines.RotateLerpTo(gameObject, OriginalHorizontalQuat, Duration)
		);
		
		ZoomAxis = OriginalZoomAxis;
		VerticalAxis = OriginalVerticalAxis;
		HorizontalAxis = OriginalHorizontalAxis;
		ElevateAxis = OriginalElevateAxis;
		
		yield return new WaitForSeconds(Duration);
		
		IsMovingInternal = false;
	}
	#endregion
	
	public void InstantResetCamera(){
		//reset to original values
		HorizontalAxis = OriginalHorizontalAxis;
		VerticalAxis = OriginalVerticalAxis;
		ZoomAxis = OriginalZoomAxis;
		ElevateAxis = OriginalElevateAxis;
		//reset the pan
		transform.position = Vector3.zero;
		//Apply the orbit changes
		Orbit(Vector2.zero, true);
		Zoom(0, true);
		Pan(Vector2.zero, true);
	}
	
	void Reset(){
		//For Editor Use, these are good default values.
		RotateSensitivity = 4f;
		ZoomSensitivity = 4f;
		PanSensitivity = 4f;
		VerticalMin = -89f;
		VerticalMax = -15f;
		ZoomMin = 0f;
		ZoomMax = 7f;
		PanMin = -5f;
		PanMax = 5f;
	}
}