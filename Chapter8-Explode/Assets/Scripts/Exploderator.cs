using UnityEngine;
using System.Collections;

public class Exploderator : MonoBehaviour {
	
	public Transform boomBall;
	public GameObject wall;
	public float explodePowerStorageRate = 0.1f;
	public float explodeTimeLimit = 1f;
	public float logBaseValue = 10f;
	public TextMesh text;
	public GameObject explosionParticles;
	
	private Rigidbody[] rigidBodies;
	private float explodePower;
	private float explodeTimer;
	private bool startTimer;
	private GameObject currentWall;
	private int clickCount;
	
	void Start(){
		RemakeWall();
	}
	
	void Update(){
		if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)){
			StoreExplode();
		}
		
		if(startTimer){
			if(explodeTimer > explodeTimeLimit){
				Explode();
			}
			else{
				explodeTimer += Time.deltaTime;
			}
		}
		
		if(Input.GetKeyDown("0")){
			RemakeWall();
		}
	}
	
	void StoreExplode(){
		startTimer = true;
		clickCount++;
		explodeTimer = 0;
		explodePower += explodePowerStorageRate/Mathf.Log(logBaseValue + explodePower, logBaseValue);
		text.text = "Power: " + explodePower.ToString("N2") + System.Environment.NewLine + "Click Count (For Reference): " + clickCount;
	}
	
	void Explode(){
		foreach(Rigidbody rigid in rigidBodies){
			rigid.isKinematic = false;
			rigid.AddExplosionForce(explodePower, boomBall.position, 100f, 0, ForceMode.Impulse);
		}
		StartCoroutine(PlayExplosionParticles());
		
		clickCount = 0;
		explodePower = 0;
		startTimer = false;
		text.text = "Power: 0.00 \nClick Count (For Reference): 0";
	}
	
	void RemakeWall(){
		if(currentWall != null) Destroy(currentWall);
		currentWall = Instantiate(wall, new Vector3(2, 0, 0), Quaternion.identity) as GameObject;
		rigidBodies = currentWall.GetComponentsInChildren<Rigidbody>();
		
		clickCount = 0;
		explodePower = 0;
		startTimer = false;
		text.text = "Power: 0.00 \nClick Count (For Reference): 0";
	}

	IEnumerator PlayExplosionParticles(){
		GameObject particles = Instantiate(explosionParticles, boomBall.position, Quaternion.identity) as GameObject;
		yield return new WaitForSeconds(5.0f);
		Destroy(particles);
	}
}
