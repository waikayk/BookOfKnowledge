using UnityEngine;
using System.Collections;

public class IKTest : MonoBehaviour {
	public Animator anim;
	public Transform ikTarget;
	
	bool isIKTracking;
	
	void Start(){
		
	}
	
	void Update(){
		if(Input.GetKeyDown("t")){
			if(isIKTracking){
				StartCoroutine(IKTarget());
			}
			else{
				isIKTracking = false;
			}
		}
	}
	
	IEnumerator IKTarget(){
		isIKTracking = true;
		while(isIKTracking){
			
			yield return new WaitForEndOfFrame();
		}
	}
}
