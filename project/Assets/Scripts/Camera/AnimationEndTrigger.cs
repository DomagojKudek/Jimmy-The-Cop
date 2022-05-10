using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.Playables;
using Respawn;

public class AnimationEndTrigger : MonoBehaviour {

    public KeyCode keyCode = KeyCode.Space;
	private PlayableDirector pd;
	public IntroController ic;

	private bool wasCalled = false;

	private void Start(){
        pd = Camera.main.GetComponent<PlayableDirector>();
	}
    private void OnDisable(){
		if(ic != null){
			ic.EndAnimation(AreaManager.GetArea("Area1"));
            Destroy(this.gameObject, 0.1f);
		}
	}
	
	private void Update(){
		if(!wasCalled){
			if(Input.GetKeyDown(keyCode) && pd.isActiveAndEnabled){
				wasCalled = true;
				pd.time = 50;
				this.gameObject.SetActive(false);
			}
		}
	}
	
}
