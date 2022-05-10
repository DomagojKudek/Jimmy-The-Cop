using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Respawn;

public class ChangeArea : MonoBehaviour {

	public string areaName;
	void OnTriggerEnter(Collider other){
        //Area next = AreaManager.instance.Next(AreaManager.instance.GetActiveArea());
		if(other.CompareTag("Player")){
			AreaManager.instance.SetActiveArea(AreaManager.GetArea(areaName));

		}	
	}
}
