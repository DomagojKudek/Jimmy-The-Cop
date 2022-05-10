using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
//using Cinemachine;
using Respawn;
using Assets.Scripts;
using UnityEngine.SceneManagement;



public class LevelSelect : MonoBehaviour
{
    public UnlockedLevels unlocked;
    public void LoadArea1(){
        GameObject respawn = GameObject.FindGameObjectWithTag("RespawnParent");
        respawn.SetActive(true);
        Area area = AreaManager.GetArea("Area1");
        SetPlayerPosition(area);
        StartGame(area);
    }

    public void LoadArea2(){
        GameObject respawn = GameObject.FindGameObjectWithTag("RespawnParent");
        respawn.SetActive(true);
        Area area = AreaManager.GetArea("Area2");
        SetPlayerPosition(area);
        StartGame(area);
    }

    public void LoadArea3(){
        GameObject respawn = GameObject.FindGameObjectWithTag("RespawnParent");
        respawn.SetActive(true);
        Area area = AreaManager.GetArea("Area3");
        SetPlayerPosition(area);
        StartGame(area);
    }

    public void LoadBoss(){
        SceneManager.LoadScene("NEW BOSS ROOM SCENE 2");
    }

    private void SetPlayerPosition(Respawn.Area area){
        PlayerManager.instance.gameObject.transform.position = area.areaStart.position;
        PlayerManager.instance.gameObject.transform.rotation = area.areaStart.rotation;
    }

    private void StartGame(Respawn.Area firstArea){
        IntroController introController = this.transform.parent.parent.GetComponent<IntroController>(); // Veliki oprez

        introController.StartFromLevelSelect();
        introController.EndAnimation(firstArea);
        //StartCoroutine(test(introController));
    }

    private void Start(){
        for(int i = 1; i<= 4; i++){
            if(unlocked.unlockedLevels[i] == 1)
                EnableDisableSelectAreaButton(i,true);
            else{
                EnableDisableSelectAreaButton(i, false);
            }
        }
    }

    public void EnableDisableSelectAreaButton(int areaNum, bool enable){
        if(areaNum == 0 || areaNum > 4) {
            Debug.LogError("Ne postoji Area " + areaNum);
            return;
        }
        this.transform.GetChild(areaNum).gameObject.SetActive(enable);
    }
}