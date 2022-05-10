using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangemeshAtRuntime : MonoBehaviour {
	public GameObject prefab;

	public GameObject child;
	private bool dead=false;
	// Use this for initialization
	void Start () {
		child=this.transform.GetChild(2).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(!dead&&child.GetComponent<Animator>().GetBool("isDead")){
			GameObject deadobj=Instantiate(prefab,child.transform.position,child.transform.rotation);
			child.SetActive(false);
			dead=true;
			Destroy(this.gameObject,4);
			Destroy(deadobj,4);
			
		}
	}
}
