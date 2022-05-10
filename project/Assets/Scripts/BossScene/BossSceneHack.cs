using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneHack : MonoBehaviour {

	public GameObject previousCheckPoints;
	void Start () {
		DisableCheckpointsFromPreviousScene();
	}

	private void DisableCheckpointsFromPreviousScene(){
		Destroy(previousCheckPoints);
	}
}
