using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpikeBalls : MonoBehaviour {

public Transform spawnpoint;

	private void OnTriggerEnter(Collider other) {
		other.gameObject.transform.position=spawnpoint.position;
	}
}
