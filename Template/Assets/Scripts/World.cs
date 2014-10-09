using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class World{
	//Singleton
	private static World InstanceInternal;
	public static World Instance{
		get{
			if (InstanceInternal == null){
				new World();
			}
			return InstanceInternal;
		}
	}

	//Initialize these in their respective scripts in Awake(). Alternatively, Initialize these all in GameController.Awake().
	//Access only in or after Start().
	public GameController Master;
	public CameraController CameraControl;

	public World(){
		if (InstanceInternal != null){
			Debug.Log ("Cannot have two instances of singleton. Call Reset() instead.");
			return;
		}
		
		InstanceInternal = this;
		
		//Initialize here

	}
	
	public void Reset(){
		Master = null;
		CameraControl = null;
	}
}

