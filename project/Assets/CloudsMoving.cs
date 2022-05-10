using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsMoving : MonoBehaviour {

public float speed =10;
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward *speed* Time.deltaTime);
	}
}
