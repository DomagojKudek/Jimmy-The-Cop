using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedRollingSounds : MonoBehaviour {
	public List <AudioClip> clips;
	private AudioSource speaker;

	public float xForce=200;
	public float yForce=80;
	// Use this for initialization
	void Start () {
		speaker = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		//periodically add random force and torque to tumbleweed
		if(Random.Range(0,10)>8 &&this.GetComponent<Rigidbody>().velocity.magnitude<0.2){
			float xF=Random.Range(xForce/2,xForce);
			this.GetComponent<Rigidbody>().AddTorque(new Vector3(0,0,-5*xF),ForceMode.Acceleration);

			this.GetComponent<Rigidbody>().AddForce(new Vector3(xF,xF*0.7f,0f),ForceMode.Acceleration);
		}
	}
	void OnCollisionEnter(Collision other){	
		if(!speaker.isPlaying ){
			//if(other.gameObject.tag.Equals("Platform") && Random.Range(0,2)==1){
			//	this.GetComponent<Rigidbody>().AddForce(new Vector3(xForce,yForce,0f), ForceMode.Force);
			//}
			//play random tumbleweed sound effect on collision
			int random_index=Random.Range(0,clips.Count);
			speaker.clip=clips[random_index];

			speaker.Play();
		}
		
		
	}
}
