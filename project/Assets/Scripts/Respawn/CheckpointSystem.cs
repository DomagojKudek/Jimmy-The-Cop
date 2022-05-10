using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets.Scripts;

namespace Respawn
{
	public class CheckpointSystem : MonoBehaviour {

        public Cinemachine.CinemachineVirtualCamera cinemachineCamera;

        private CinemachineFramingTransposer framingTransposer;

        private float lookaheadTime = 0.5f;
        private float lookaheadSmoothing = 0.5f;
        public float lookaheadRestartDelay = 0.5f;

        public GameObject checkpointParent;
		private List<Checkpoint> checkpoints;
		private Checkpoint activeCheckpoint;

		public static CheckpointSystem instance;
		private int activeCheckpointIndex;

        void Awake()
        {
            if (instance == null)
            {
				Init();
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

		private void Init(){
			checkpoints = new List<Checkpoint>();
			activeCheckpoint = null;//TODO promijeniti u prvi prilikom spawna
			framingTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
			foreach(Transform checkpointTrans in checkpointParent.transform){
				if(checkpointTrans.gameObject.active)
					checkpoints.Add(checkpointTrans.GetComponent<Checkpoint>());
			}
			print("Broj Checkpointova = " + checkpoints.Count);
		}

		//Returns transform of last Checkpoint
		public Transform GetActiveCheckpointTransform(){
			return activeCheckpoint.transform;
		}

		public static CheckpointSystem GetInstance(){
			return instance;
		}
		
		public void SetActiveCheckpoint(Checkpoint checkpoint){
			if(activeCheckpoint != null){
                activeCheckpoint.isActive = false;
				activeCheckpoint.SetColor(Color.red);
			}
            activeCheckpoint = checkpoint;

			activeCheckpointIndex = checkpoints.FindIndex(c => c == activeCheckpoint);//reference equals
		}

		public Checkpoint GetActiveCheckpoint(){
			return activeCheckpoint;
		}

		private Checkpoint SetStartCheckpoint(){
			return null;
		}

		public void MoveToNextCheckpoint(){
			if(activeCheckpointIndex == checkpoints.Count - 1) return;
            //cinemachineCamera.Follow = null;

            activeCheckpointIndex++;
			PlayerManager.instance.transform.position = checkpoints[activeCheckpointIndex].transform.position;
            PlayerManager.instance.transform.position = new Vector3(PlayerManager.instance.transform.position.x, PlayerManager.instance.transform.position.y, 0);
            //cinemachineCamera.Follow = PlayerManager.instance.transform;

            framingTransposer.OnTargetObjectWarped(PlayerManager.instance.gameObject.transform, Vector3.zero);
            PlayerManager.instance.GetComponent<JimmyController1>().ClearCollisionList();
            StartCoroutine(lookaheadRestart());

        }

        public void MoveToPreviousCheckpoint()
        {
            if (activeCheckpointIndex == 0) return;
            //cinemachineCamera.Follow = null;

            activeCheckpointIndex--;
            PlayerManager.instance.transform.position = checkpoints[activeCheckpointIndex].transform.position;
            PlayerManager.instance.transform.position = new Vector3(PlayerManager.instance.transform.position.x, PlayerManager.instance.transform.position.y, 0);
            //cinemachineCamera.Follow = PlayerManager.instance.transform;

            framingTransposer.OnTargetObjectWarped(PlayerManager.instance.gameObject.transform, Vector3.zero);
            PlayerManager.instance.GetComponent<JimmyController1>().ClearCollisionList();
            StartCoroutine(lookaheadRestart());

        }

        IEnumerator lookaheadRestart()
        {
            framingTransposer.m_LookaheadTime = 0;
            framingTransposer.m_LookaheadSmoothing = 0;
            yield return new WaitForSeconds(lookaheadRestartDelay);
            framingTransposer.m_LookaheadTime = lookaheadTime;
            framingTransposer.m_LookaheadSmoothing = lookaheadSmoothing;
        }


		void Update(){
			if(checkpointParent!= null){
				if(Input.GetKeyDown(KeyCode.Period)){
					MoveToNextCheckpoint();
				}
				if (Input.GetKeyDown(KeyCode.Comma))
				{
					MoveToPreviousCheckpoint();
				}
			}

			//print("Active checkpoint index = " + activeCheckpointIndex);
		}
	}
}
