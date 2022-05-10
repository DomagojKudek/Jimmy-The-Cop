using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Experimental.Rendering.LightweightPipeline;

public class IntroController : MonoBehaviour
{
    private PlayableDirector director;
    
    private MainMenu mainMenu;
    public IntroText text;
    public LightweightPipelineAsset asset;
    public void Start(){
        director = Camera.main.GetComponent<PlayableDirector>();
        mainMenu = this.GetComponent<MainMenu>();
        director.enabled = false;
        asset.shadowDistance=1000;
    }
    public void StartIntro(){
        FadeMainMenu();
    }

    public void StartFromLevelSelect(){
        CanvasGroup canvasGroup = this.transform.GetChild(0).GetComponent<CanvasGroup>();
    }

    private IEnumerator FadeLevelSelect(CanvasGroup canvasGroup)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f;
            yield return null;
        }
    }

    private void FadeMainMenu()
    {
        CanvasGroup canvasGroup = this.transform.GetChild(0).GetComponent<CanvasGroup>();
        StartCoroutine(Fade(canvasGroup));
    }
    private IEnumerator Fade(CanvasGroup canvasGroup)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f;
            yield return null;
        }

        PlayAnimation();
    }
    private void PlayAnimation(){
        if(director == null) return;//zbog nekog cudnog buga
        director.enabled = true;
        director.Play();
        text.StartText();
    }

    public void EndAnimation(Respawn.Area firstArea){
        //director.enabled = false;
        asset.shadowDistance=120;
        if(text != null){
            text.GetComponent<MeshRenderer>().enabled = false;
            text.gameObject.SetActive(false);
        }
        if(director != null){
            director.Stop();
            Destroy(director);
        }
        this.GetComponent<MainMenu>().EnablePlayMode(firstArea);
        
    }
}