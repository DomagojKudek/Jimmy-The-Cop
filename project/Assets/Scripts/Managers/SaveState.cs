using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveState {

	public string SceneName;
	public int Points;
	
	public SaveState(string CompletedLevel,int AcquiredPoints){
		SceneName=CompletedLevel;
		Points=AcquiredPoints;
	}
	[SerializeField]
	public SerializableVector3 playerPosition;
    [SerializeField]
	public SerializableQuaternion playerRotation;

	public string areaName;

	public int playerHp;

	//private List<int> krafnaIds;

	public SaveState(string scene, Vector3 position, Quaternion rotation,
					 int Points, int playerHp){
		SceneName = scene;
		playerPosition = position;
		playerRotation = rotation;
		this.Points = Points;
		//this.areaName = areaName;
		this.playerHp = playerHp;
	}
	
}
