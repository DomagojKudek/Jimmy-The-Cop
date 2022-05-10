using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag=="Player"){

			collider.gameObject.GetComponent<PlayerHealth>().ChangeHp(-10000);
		}else{
			Destroy(collider.gameObject);
		}
		
	}
}
