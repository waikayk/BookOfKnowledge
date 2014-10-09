using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class ObliqueCamera : MonoBehaviour {
	public float horizontalOblique;
	public float verticalOblique;
	
	void Start(){
		SetObliqueness(horizontalOblique, verticalOblique);
	}
	
//	void Update(){
//		SetObliqueness(horizontalOblique, verticalOblique);
//	}
	
	void SetObliqueness(float horizObl, float vertObl) {
		Matrix4x4 matrix = camera.projectionMatrix;
		matrix[0, 2] = horizObl;
		matrix[1, 2] = vertObl;
		camera.projectionMatrix = matrix;
	}
}
