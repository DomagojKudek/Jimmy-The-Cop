using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityFieldHexagonalControl : MonoBehaviour {
	public Slider slider;	
	public bool gravity_field_on=false;
	private bool shouldLerp=false;
	public float timeStartedLerping;
	public float lerpTime=1;


	public float Opaque=1;
	public float Transparent=0;
	public string noise_speedValueId;
	public float startNoiseSpeed=1;
	public float endNoiseSpeed=3;
	public float[] noiseSpeeds=new float[2];
	public float startVectorOffset=-10;
	public float endVectorOffset=1;
	public float[] vectorOffsets=new float[2];
	public string vector_offsetValueId;
	public string alphaValueId;
	public float transparency;
	private int i=0;
	private Quaternion iniRot;
	private Material material;
	public float currentNoiseSpeed;
	public float currentVectorOffest;
	public float currentTransparency;
	// Use this for initialization
	void Start () {
		
		noiseSpeeds[0]=startNoiseSpeed;
		noiseSpeeds[1]=endNoiseSpeed;
		vectorOffsets[0]=startVectorOffset;
		vectorOffsets[1]=endVectorOffset;
		iniRot = transform.rotation;
		transparency=gravity_field_on?1:0;
		material=GetComponent<Renderer>().material;

		material.SetFloat(alphaValueId,transparency);
		material.SetFloat(noise_speedValueId,startNoiseSpeed);
		material.SetFloat(vector_offsetValueId,startVectorOffset);

		currentTransparency=material.GetFloat(alphaValueId);
		currentNoiseSpeed=material.GetFloat(noise_speedValueId);
		currentVectorOffest=material.GetFloat(vector_offsetValueId);
		
	}
	 
	void LateUpdate(){
		transform.rotation = iniRot;
	}
	void Update() {
		float gravityProgress=slider.value/(slider.maxValue-slider.minValue);
		if(shouldLerp){
			//currentNoiseSpeed=material.GetFloat(noise_speedValueId);
			//currentVectorOffest=material.GetFloat(vector_offsetValueId);
			float not_transparency=1-transparency;
			//Debug.LogWarning("gravityProgress"+gravityProgress);
			//Debug.LogWarning("gravity_field_on"+gravity_field_on);
			//Debug.LogWarning("currentVectorOffest"+currentVectorOffest+"targetVectorOffest"+vectorOffsets[1-i]);
			//Debug.LogWarning("transparency"+transparency+"not_transparency"+not_transparency);
			//GetComponent<Renderer>().material.SetFloat(noise_speedValueId,Lerp(noiseSpeeds[0],noiseSpeeds[0],timeStartedLerping,lerpTime));
			
			//float[] newValues=
			LerpArray(timeStartedLerping,lerpTime);
			
		}
	}
	public void toggleGravityField(){

		timeStartedLerping=Time.time;
		if(shouldLerp){
			i=1-i;
			transparency=1-transparency;
			gravity_field_on=!gravity_field_on;
		}else{
			shouldLerp=true;
			transparency=gravity_field_on?1:0;
		}
		

	}
	public float Lerp(float start,float end,float timeStartedLerping,float lerpTime){
		float timeSinceStarted=Time.time-timeStartedLerping;

		float percentageComplete=timeSinceStarted/lerpTime;
		if(percentageComplete>1){
			shouldLerp=false;
			i=1-i;
			transparency=1-transparency;
			gravity_field_on=!gravity_field_on;
		}
		var result=Mathf.Lerp(start,end,percentageComplete);

		return result;
	}
	public void LerpArray(float timeStartedLerping,float lerpTime){
		//float[]results=new float[3];
		float timeSinceStarted=Time.time-timeStartedLerping;

		float percentageComplete=timeSinceStarted/lerpTime;

		currentTransparency=Mathf.Lerp(transparency,1-transparency,percentageComplete);
		currentNoiseSpeed=Mathf.Lerp(noiseSpeeds[i],noiseSpeeds[1-i],percentageComplete);
		currentVectorOffest=Mathf.Lerp(vectorOffsets[i],vectorOffsets[1-i],percentageComplete);

		material.SetFloat(alphaValueId,currentTransparency);
		//material.SetFloat(noise_speedValueId,currentNoiseSpeed);
		material.SetFloat(vector_offsetValueId,currentVectorOffest);
		if(percentageComplete>1){
			//Debug.LogError("TransitionComplete");
			shouldLerp=false;
			i=1-i;
			transparency=1-transparency;
			gravity_field_on=!gravity_field_on;
			return;
		}
		
		/* results[0]=currentTransparency;
		results[1]=currentNoiseSpeed;
		results[2]=currentVectorOffest;
		return results; */
	}
}
