using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using Respawn;
using Assets.Scripts;

public static class LoadSystem{
	
	private static SaveState saveState;
	public static void Save(string levelName,int acquiredPoints){
		BinaryFormatter formatter=new BinaryFormatter();
		FileStream saveStream=new FileStream(Application.persistentDataPath+"/saveGame.sav",FileMode.Create);
		//SaveState saveState=new SaveState(levelName,acquiredPoints);
		Transform checkpointTransform = CheckpointSystem.GetInstance().GetActiveCheckpointTransform();
		//ime postavlja na trenutnu scenu, TODO ako ima vise scena u projektu - koristi levelName
		PlayerHealth ph = PlayerManager.instance.GetComponent<PlayerHealth>();
		SaveState saveState = new SaveState(SceneManager.GetActiveScene().name, 
											checkpointTransform.position, checkpointTransform.rotation, 
											acquiredPoints, 
											//AreaManager.instance.GetActiveArea().name,
											ph.GetHp());
		formatter.Serialize(saveStream,saveState);
		saveStream.Close();
	}

	//mora se prvo pozvati ako se ista zeli loadat!
	public static void Load(){
		if(File.Exists(Application.persistentDataPath+"/saveGame.sav")){
			BinaryFormatter formatter=new BinaryFormatter();
			FileStream loadStream=new FileStream(Application.persistentDataPath+"/saveGame.sav",FileMode.Open);
			SaveState loadData=formatter.Deserialize(loadStream) as SaveState;
			SetSaveState(loadData);
			loadStream.Close();
			//SceneManager.LoadScene(loadData.SceneName);
		}
	}
	
	public static int LoadScore(){
		BinaryFormatter formatter=new BinaryFormatter();
		FileStream loadStream=new FileStream(Application.persistentDataPath+"/saveGame.sav",FileMode.Open);
		SaveState loadData=formatter.Deserialize(loadStream) as SaveState;
		loadStream.Close();
		return loadData.Points;
	}

	public static void LoadScene(){
        //SceneManager.LoadScene(GetSaveState().SceneName);
	}

	public static int LoadScoreV2(){
        return GetSaveState().Points;
	}

	public static Vector3 LoadPlayerPosition(){
		return GetSaveState().playerPosition;
	}

	public static Quaternion LoadPlayerRotation(){
		return GetSaveState().playerRotation;
	}
	
	public static string LoadAreaName(){
		return GetSaveState().areaName;
	}
	

	public static int LoadPlayerHp(){
		return GetSaveState().playerHp;
	}

	public static void SetSaveState(SaveState ss){
		LoadSystem.saveState = ss;
	}

	public static SaveState GetSaveState(){
		return LoadSystem.saveState;
	}
}
