using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Respawn;
using Assets.Scripts;

public class MainMenuController : MonoBehaviour {

	//koristi se IntroController.StartIntro	
	public void NewGameButton () {
		//LoadSystem.Save("_DemoLvl",0);
		//SceneManager.LoadScene("_DemoLvl");
	}
	
	public void LoadGameButton(){
		//load stuff
		LoadSystem.Load();
		LoadSystem.LoadScene();
		//AreaManager.instance.SetActiveArea(AreaManager.GetArea(LoadSystem.LoadAreaName()));
		Transform pt = PlayerManager.instance.transform;
		pt.position = LoadSystem.LoadPlayerPosition();
        pt.rotation = LoadSystem.LoadPlayerRotation();
		GameManager.instance.score = LoadSystem.LoadScoreV2();
		HUDManager.instance.RenderScore(GameManager.instance.score);
        //start game
        PlayerManager.instance.GetComponent<PlayerHealth>().SetHp(LoadSystem.LoadPlayerHp());
        if(AreaManager.GetArea(LoadSystem.LoadAreaName()) != null){
        	StartGame(AreaManager.GetArea(LoadSystem.LoadAreaName()));
        }

    }
	
	public void QuitGameButton(){
		Application.Quit();
	}

    private void StartGame(Respawn.Area firstArea)
    {
        IntroController introController = this.transform.parent.parent.GetComponent<IntroController>(); // Veliki oprez

        introController.StartFromLevelSelect();
        introController.EndAnimation(firstArea);
        //StartCoroutine(test(introController));
    }
}