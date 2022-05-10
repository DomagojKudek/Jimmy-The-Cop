using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.Assertions;
using Respawn;

public enum PlayMode{
	Intro,
	Play
}

public class MainMenu : MonoBehaviour {

	// Use this for initialization
    private AudioSource[] sources;
	private AudioManager am;

	private GameObject respawn;
	private PlayMode mode;

	void Start(){
		mode = PlayMode.Intro;
		am = AudioManager.instance;

        //DisableRespawn();
        DisablePushPull();
        DisableUI();
        MuteSounds();
        MainMenuSound();
        DisableController();
	}
	/*
	void Start () {
		DisableRespawn();
        DisablePushPull();
        DisableUI();
		MuteSounds();
        MainMenuSound();
		DisableController();
	}
	*/

	public void EnablePlayMode(Respawn.Area area){
		if(mode == PlayMode.Play) return;
        mode = PlayMode.Play;
        EnablePushPull();
		EnableUI();
		EnableSounds();
		EnableController();
		EnableRespawn();
		//disable INTRO
		print(this.transform.root + " U ManiMenu");
		//this.transform.root.gameObject.SetActive(false);
		GameObject.FindGameObjectWithTag("IntroParent").SetActive(false);
        //set game state
		AreaManager.instance.SetActiveArea(AreaManager.GetArea("Area1"));
        GameManager.instance.gameState = GameState.Game;
	}

	private void MuteSounds(){
        sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		for(int i = 0; i < sources.Length; i++){
			sources[i].mute = true;
		}
	}

	public void EnableSounds(){
        for (int i = 0; i < sources.Length; i++)
        {
			if(sources[i] != null){
            	sources[i].mute = false;
			}
        }

	}

    public void MainMenuSound()
    {

        BackgroundMusic.PlayBackGroundMusic();
    }

	private void DisableUI(){
        //if (HUDManager.instance == null) return;
        HUDManager.instance.canvas.gameObject.SetActive(false);
	}

	private void EnableUI(){
		//if(HUDManager.instance == null) return;
        HUDManager.instance.canvas.gameObject.SetActive(true);
    }

	private void DisablePushPull(){
        //if (PlayerManager.instance == null) return;
		print(PlayerManager.instance);
		print("adad" + PlayerManager.instance.transform.GetChild(0));
        PlayerManager.instance.transform.GetChild(0).gameObject.SetActive(false);
	}

    private void EnablePushPull(){
		//if(PlayerManager.instance == null) return;
        PlayerManager.instance.transform.GetChild(0).gameObject.SetActive(true);
    }

	private void DisableController(){
		PlayerManager.instance.GetComponent<JimmyController1>().enabled = false;
	}

	private void EnableController(){
        PlayerManager.instance.GetComponent<JimmyController1>().enabled = true;
    }

	private void DisableRespawn(){
        respawn = GameObject.FindWithTag("RespawnParent");
		if(respawn==null) return;
		respawn.SetActive(false);
	}

    private void EnableRespawn(){
        if (respawn == null) return;
        respawn.SetActive(true);
	}
}

