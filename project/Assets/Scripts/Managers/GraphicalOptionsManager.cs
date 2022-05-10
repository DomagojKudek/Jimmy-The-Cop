using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GraphicalOptionsManager: MonoBehaviour{
	
	public GameObject[] particles;

	public Camera mainCamera;
	
	private PostProcessVolume volume;
	
	private Bloom bloom=null;
	private AmbientOcclusion ambientOcclusion=null;
	private ColorGrading colorGrading=null;
	private DepthOfField depthOfField=null;
	
	void Awake(){
		volume=mainCamera.GetComponent<PostProcessVolume>();
		volume.profile.TryGetSettings(out bloom);
		volume.profile.TryGetSettings(out ambientOcclusion);
		volume.profile.TryGetSettings(out colorGrading);
		volume.profile.TryGetSettings(out depthOfField);
	}
	
	public void setParticles(bool Status){
		foreach(GameObject ParticleGroup in particles){
			ParticleGroup.SetActive(Status);
			
		}
	}
	
	public void setBloom(bool status){
		bloom.enabled.value=status;
	}
	
	public void setAmbientOcclusion(bool status){
		ambientOcclusion.enabled.value=status;
	}
	
	public void setColorGrading(bool status){
		colorGrading.enabled.value=status;
	}
	
	public void setDepthOfField(bool status){
		depthOfField.enabled.value=status;
	}

}
