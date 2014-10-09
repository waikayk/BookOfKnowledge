using UnityEngine;
using System.Collections;

public class GyroAlignAxis : MonoBehaviour {
	
	private Gyroscope gyroscope;
	private RemoteGyroscope remoteGyroscope;
	
	private Quaternion rotaFix;
	
	// Use this for initialization
	void Start () {
		gyroscope = Input.gyro;
		rotaFix = Quaternion.Euler(new Vector3(0, 0, 180f));
	}
	
	// Update is called once per frame
	void Update () {
		if (SystemInfo.supportsGyroscope) {
			transform.rotation = gyroscope.attitude * rotaFix;
		}
		else{
			if(remoteGyroscope == null){
				remoteGyroscope = GyroMote.gyro();
			}
			
			if(remoteGyroscope){
				transform.rotation = remoteGyroscope.attitude * rotaFix;
			}
		}
	}
}
