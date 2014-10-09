using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public CameraController cameraControl;
	
	void Awake(){
		//MAKE A NEW WORLD!
		new World();
		//AND DECLARE THIS ITS MASTER!
		World.Instance.Master = this;
		World.Instance.CameraControl = cameraControl;
	}
	
	void Update(){
		ProcessInput();
	}
	
	void ProcessInput(){
		//Check if it is over UI Element
//		CameraRay = GUICamera.ScreenPointToRay(Input.mousePosition);
//		IsTouchingGUI = Physics.Raycast(CameraRay, 10000);
//		if(IsTouchingGUI) return;
		
		if(Input.GetMouseButton(0)){
			cameraControl.Orbit(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
		}
		else if(Input.GetAxis("Mouse ScrollWheel") != 0){
			if(Input.GetKey(KeyCode.LeftAlt)){
				cameraControl.Elevate(Input.GetAxis("Mouse ScrollWheel"));
			}
			else{
				cameraControl.Zoom(Input.GetAxis("Mouse ScrollWheel"));
			}
		}
		else if(Input.GetMouseButton(1)){
			cameraControl.Elevate(Input.GetAxis("Mouse Y"));
		}
		
		if(Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftAlt)){
			cameraControl.Pan(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
		}
	}
}
