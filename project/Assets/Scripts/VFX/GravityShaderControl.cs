using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityShaderControl : MonoBehaviour {
	public bool gravity_field_on=false;
	private bool shouldLerp=false;
	public float timeStartedLerping;
	public float lerpTime=1;


	public float Opaque=1;
	public float Transparent=0;
	public string dissolveValueId;
	public float transparency;
	private Quaternion iniRot;
	// Use this for initialization
	void Start () {
		iniRot = transform.rotation;
		transparency=gravity_field_on?0:1;
		GetComponent<Renderer>().material.SetFloat(dissolveValueId,transparency);
	}
	 
	void LateUpdate(){
		transform.rotation = iniRot;
	}
	void Update() {
		if(shouldLerp){
			float not_transparency=1-transparency;
			GetComponent<Renderer>().material.SetFloat(dissolveValueId,Lerp(transparency,not_transparency,timeStartedLerping,lerpTime));
		}
	}
	public void toggleGravityField(){

		timeStartedLerping=Time.time;
		if(shouldLerp){
			transparency=1-transparency;
			gravity_field_on=!gravity_field_on;
		}else{
			shouldLerp=true;
			transparency=gravity_field_on?0:1;
		}
		

	}
	public float Lerp(float start,float end,float timeStartedLerping,float lerpTime){
		float timeSinceStarted=Time.time-timeStartedLerping;

		float percentageComplete=timeSinceStarted/lerpTime;
		if(percentageComplete>1){
			shouldLerp=false;
			gravity_field_on=!gravity_field_on;
		}
		var result=Mathf.Lerp(start,end,percentageComplete);

		return result;
	}
}
