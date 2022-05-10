using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrubSound : MonoBehaviour {
	public List <AudioClip> clips;
	private AudioSource speaker;

	// Use this for initialization
	void Start () {
		speaker = GetComponent<AudioSource>();
	}
	


	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.Equals("Player")||other.gameObject.tag.Equals("Object")||other.gameObject.tag.Equals("Enemy")){			
			if(!speaker.isPlaying ){
				int random_index=Random.Range(0,clips.Count);
				speaker.clip=clips[random_index];
				speaker.Play();
			}
		}
		
	}
}
