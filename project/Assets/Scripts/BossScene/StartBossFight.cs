using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour {

	public GameObject boss;
	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			boss.SetActive(true);
		}
	}
}
