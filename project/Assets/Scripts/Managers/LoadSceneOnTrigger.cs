using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;


public class LoadSceneOnTrigger : MonoBehaviour {
    
    public string sceneToLoad;
	public GameManager gameManager;
	// Use this for initialization
	void Start () {
		gameManager=GameManager.instance;
	}
	void OnTriggerEnter(Collider other){
        if ((other.gameObject.tag == "Player")){
            //LoadSystem.Save(sceneToLoad,gameManager.score);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    
}