using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
public class LaunchPad : MonoBehaviour {

	// Use this for initialization
	public int offDuration=15;
	public Material material;
	public string shaderOnString="Vector1_858832AD";
	public bool on=true;
	void Start(){
		material=this.GetComponent<MeshRenderer>().material;
	}

	void Update(){
		if(on && material.GetFloat(shaderOnString)<1){
			this.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat(shaderOnString,0);
			this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("New Trigger");
			StartCoroutine(LateCall());
			on=false;

		}
	}
	IEnumerator LateCall(){
        yield return new WaitForSeconds(offDuration);
		material.SetFloat(shaderOnString,1);
		this.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat(shaderOnString,1);

		on=true;
     }
}
}
