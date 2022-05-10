using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
	private AudioManager audioManager;


	// Use this for initialization
	void Start () {
		//audioManager=AudioManager.instance;
	}
	/*
	// Update is called once per frame
	void Update () {
		audioManager.Play("BackgroundMusic");
		audioManager.Play("WindSound");
	}
	*/
	public static void PlayBackGroundMusic(){
        AudioManager.instance.Play("BackgroundMusic", true);
        AudioManager.instance.Play("WindSound", true);
	}
}
