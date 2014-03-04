using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	public LineRenderer line;
	public Rigidbody2D forceObject;
	public float forceMagnitude = 100f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetMouseButtonDown(0)){
//			CastRay(Input.mousePosition);
//		}
		
	//	if(Input.GetMouseButton(0)){
			ImpartForce(Input.mousePosition);
			
	//	}
		
		
	}
	
	void CastRay(Vector3 mousePosition){
		//Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
		//if(Physics2D.Raycast(Vector2.zero, new Vector2(worldPoint.x, worldPoint.y))){
		//if(Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition))){
		if(Physics2D.GetRayIntersectionNonAlloc(Camera.main.ScreenPointToRay(mousePosition), new RaycastHit2D[1]) > 0){
			print ("Hit");
		}
	}
	
	void ImpartForce(Vector3 mousePosition){
		
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
		
		line.SetPosition(0, new Vector3(worldPoint.x, worldPoint.y, 0));
		line.SetPosition(1, forceObject.transform.position);
		
		Vector3 forceVector = worldPoint - forceObject.transform.position;
		
		forceObject.AddForce(new Vector2(forceVector.x, forceVector.y) * forceMagnitude);
	}
}
