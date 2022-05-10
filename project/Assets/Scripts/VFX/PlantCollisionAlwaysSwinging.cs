using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCollisionAlwaysSwinging : MonoBehaviour {
	public List <AudioClip> clips;
	private AudioSource speaker;
	public float onDuration=0.3f;
	public float offDuration=1.7f;
	public float forceWind=5;
	public float forceHit=30;
	// Use this for initialization
	void Start () {
		speaker = GetComponentInParent<AudioSource>();
		StartCoroutine(SwingForever());
	}
	 void OnEnable()
    {
        StartCoroutine(SwingForever());
	}
	IEnumerator OnTriggerEnter(Collider other){	
		if(other.gameObject.tag.Equals("Player")||other.gameObject.tag.Equals("Object")||other.gameObject.tag.Equals("Enemy")){		
		//if(!other.gameObject.tag.Equals("Platform")&&!other.gameObject.tag.Equals("Plant")&&!other.gameObject.tag.Equals("Debris")){
			//Debug.LogWarning(other.gameObject.tag);
			HingeJoint joint=this.GetComponent<HingeJoint>();
			var motor=joint.motor;
			motor.force=forceHit;
			motor.targetVelocity=forceHit;
			joint.useMotor = true;
			joint.motor=motor;
			yield return new WaitForSeconds (0.2f);
			this.GetComponent<HingeJoint>().useMotor = false;
			//this.GetComponent<Rigidbody>().AddForce(new Vector3(zForce,zForce,zForce), ForceMode.Impulse);
			
			if(!speaker.isPlaying ){
				int random_index=Random.Range(0,clips.Count);
				speaker.clip=clips[random_index];
				speaker.Play();
			}
		}
		
		
	}
	IEnumerator SwingForever(){
		while (true){
			HingeJoint joint=this.GetComponent<HingeJoint>();

			var motor=joint.motor;
			motor.force=forceWind;
			motor.targetVelocity=forceWind;
			joint.useMotor = true;
			joint.motor=motor;
			yield return new WaitForSeconds (onDuration);
			this.GetComponent<HingeJoint>().useMotor = false;
			yield return new WaitForSeconds (Random.Range(1.0f, 3.0f)*offDuration);
		}
	}
}
