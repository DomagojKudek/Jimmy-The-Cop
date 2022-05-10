using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

namespace Respawn
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Checkpoint : MonoBehaviour
    {
		[HideInInspector]
        public bool isActive = false;
        //private List<GameObject> areas;
        private List<GameObject> puzzlesInArea;

        protected Material material;
        protected new Collider collider;
        public AudioClip checkpointSound;
	    protected AudioSource speaker;

		protected CheckpointSystem checkpointSystem;
        public ParticleSystem particleSystemFire;
        public GameObject eye1;
        public GameObject eye2;
		
		//public Text saveText;

        protected void Start()
        {
            particleSystemFire=this.transform.GetChild(0).GetComponent<ParticleSystem>();
            eye1=this.transform.GetChild(1).gameObject;
            eye2=this.transform.GetChild(2).gameObject;
            material = this.GetComponent<MeshRenderer>().material;
            SetColor(Color.red);

            collider = this.GetComponent<Collider>();
            collider.isTrigger = true;

			checkpointSystem = CheckpointSystem.GetInstance();
            speaker=GetComponent<AudioSource>();
            speaker.clip=checkpointSound;
			//saveText.CrossFadeAlpha(0f,0f,false);
        }

        void OnTriggerEnter(Collider coll)
        {
            if (!coll.CompareTag("Player")) return;

            if (isActive == true) return;
            isActive = true;
            checkpointSystem.SetActiveCheckpoint(this);
            TurnOnEffects();
            SetColor(Color.green);
            speaker.Play();
			
			//saveText.CrossFadeAlpha(255f,3f,false);
			//saveText.CrossFadeAlpha(0f,3f,false);

            LoadSystem.Save("bla", GameManager.instance.score);
            print("GAME SAVED");
        }

		public void SetColor(Color color){
			material.color = color;
        }
        public void TurnOnEffects(){
            particleSystemFire.Play();
            eye1.SetActive(true);
            eye2.SetActive(true);

        }
    }
}