using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class PauseManager : MonoBehaviour {
	//public GameManager gameManager;
	public Canvas canvas;
	public GameObject player;
	private GameObject pushScript;
	public KeyCode joystickPauseButton = KeyCode.JoystickButton2;
	public KeyCode keyCode = KeyCode.Escape;
	public bool pausable=false;
	private bool paused=false;
	// Use this for initialization
	
	private void Start(){
		player = PlayerManager.instance;
		pushScript = player.transform.GetChild(0).gameObject;
	}
	public void ResumeGame(){
		canvas.gameObject.SetActive(false);
		Time.timeScale=1;
		player.GetComponent<JimmyController1>().enabled=true;
		player.GetComponent<GravityField>().enabled=true;
		player.GetComponent<Push_Pull>().enabled=true;
		pushScript.GetComponent<Push>().enabled=true;
		pushScript.GetComponent<Pull>().enabled=true;
		paused=false;
	}
	
	public void ChangePausability(bool newValue){
			pausable=newValue;
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetKeyDown(joystickPauseButton)||Input.GetKeyDown(keyCode))&&pausable&&paused){
			ResumeGame();
		}
		else if((Input.GetKeyDown(joystickPauseButton)||Input.GetKeyDown(keyCode))&&pausable&&!paused){
			canvas.gameObject.SetActive(true);
			Time.timeScale=0;
			player.GetComponent<JimmyController1>().enabled=false;
			player.GetComponent<GravityField>().enabled=false;
			player.GetComponent<Push_Pull>().enabled=false;
			pushScript.GetComponent<Push>().enabled=false;
			pushScript.GetComponent<Pull>().enabled=false;
			paused=true;
		}
		
	}
}
