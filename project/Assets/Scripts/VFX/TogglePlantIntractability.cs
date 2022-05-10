using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class TogglePlantIntractability : MonoBehaviour {
private GameObject player;
	public int radius=10;
	private float checkRateInterval=0.5f;
	GameObject armature;
	// Use this for initialization
	void Start () {
		radius=50;
		armature = this.gameObject.transform.GetChild(0).gameObject;
		player = PlayerManager.instance;
		InvokeRepeating("ToggleIntractability", 0, checkRateInterval);
	}
	
	void ToggleIntractability () {

		if(!armature.activeSelf&&Vector3.Distance(player.transform.position,this.transform.position)<radius){
			armature.SetActive(true);
		}else if(armature.activeSelf&&Vector3.Distance(player.transform.position,this.transform.position)>=radius){
			armature.SetActive(false);
		}
	}
}
