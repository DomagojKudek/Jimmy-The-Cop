using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class ToggleFractureVisibility : MonoBehaviour {
	public GameObject fracture;
	public GameObject fakefracture;
	private GameObject player;
	public int radius=20;
	public int broken_radius=20;
	private float checkRateInterval=0.5f;
	public bool broken=false;

	// Use this for initialization
	void Start () {
		//in the prefab the fracture must be the first child and the fakefracture must be the second child
		fracture = this.gameObject.transform.GetChild(0).gameObject;
		fakefracture = this.gameObject.transform.GetChild(1).gameObject;
		player = PlayerManager.instance;
		InvokeRepeating("ToggleVisibility", 0, checkRateInterval);
	}
	

	void ToggleVisibility () {
		if(!broken){
			if(Vector3.Distance(player.transform.position,this.transform.position)<radius){
				fakefracture.SetActive(false);
				fracture.SetActive(true);
			}else{
				fakefracture.SetActive(true);
				fracture.SetActive(false);
			}
		}else{
			if(Vector3.Distance(player.transform.position,this.transform.position)<radius+broken_radius){
				fakefracture.SetActive(false);
				fracture.SetActive(true);
			}else{
				fracture.SetActive(false);
			}
		}
	}
}

