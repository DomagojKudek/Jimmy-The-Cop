using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DisableIntro : MonoBehaviour{

    void Start(){
        if(GameObject.FindWithTag("IntroParent") == null){
            this.GetComponent<PlayableDirector>().enabled = false;
        }
    }
}