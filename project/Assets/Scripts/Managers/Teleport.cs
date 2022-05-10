using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Respawn;

namespace Assets.Scripts
{
	public class Teleport : MonoBehaviour {
		public Transform locationToTeleport;
		private AudioManager audioManager;
		public CinemachineVirtualCamera vcam;
		private CinemachineFramingTransposer framingTransposer;
		private float lookaheadTime=0.5f;
		private float lookaheadSmoothing=0.5f;
		public float lookaheadRestartDelay=0.5f;

		// Use this for initialization
		void Start () {
			audioManager=AudioManager.instance;
			GameObject vCamGameObject=GameObject.FindWithTag("CinemachineVirtualCamera");
			vcam=vCamGameObject.GetComponent<CinemachineVirtualCamera>();
			framingTransposer=vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
			lookaheadTime=framingTransposer.m_LookaheadTime;
			lookaheadSmoothing=framingTransposer.m_LookaheadSmoothing;

		}
		
		void OnTriggerEnter(Collider other){
			if ((other.gameObject.tag == "Player")){
                print("TRIGGERED");
                audioManager.Play("Teleport");
				other.gameObject.transform.position=locationToTeleport.position;

				Vector3 posDelta = locationToTeleport.transform.position - other.gameObject.transform.position;
				framingTransposer.OnTargetObjectWarped(other.gameObject.transform,posDelta);
				other.GetComponent<JimmyController1>().ClearCollisionList();
				StartCoroutine(lookaheadRestart());
                //int index=GameManager.instance.spawnPointIndex+=1;
                //GameManager.instance.spawnPoints.Add(locationToTeleport);
				//Set next area
                Area next = AreaManager.instance.Next(AreaManager.instance.GetActiveArea());
                //AreaManager.instance.SetActiveArea(next);
            }
		}
		IEnumerator lookaheadRestart(){
			
			framingTransposer.m_LookaheadTime=0;
			framingTransposer.m_LookaheadSmoothing=0;
			yield return new WaitForSeconds(lookaheadRestartDelay);
			framingTransposer.m_LookaheadTime=lookaheadTime;
			framingTransposer.m_LookaheadSmoothing=lookaheadSmoothing;
		}
	}
 
}
