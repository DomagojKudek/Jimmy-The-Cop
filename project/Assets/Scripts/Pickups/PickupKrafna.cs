using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Pickups
{
    class PickupKrafna : Pickup
    {
        public int scorePoints = 1;
        private HUDManager hudManager;
		public AudioClip biteSound;
	    private AudioSource speaker;
        public ParticleSystem particleSystem;
        /*
		void Start(){
			if(File.Exists(Application.persistentDataPath+"/saveGame.sav")){
					print("učitani rezultat je:"+LoadSystem.loadScore());
					scorePoints=LoadSystem.loadScore();
				}
            else scorePoints = 0;
		}
		*/
        void Start(){
            scorePoints=1;
            speaker=GetComponent<AudioSource>();
            speaker.clip=biteSound;
            particleSystem=this.transform.GetChild(0).GetComponent<ParticleSystem>();
        } 
        public override void OnPickup()
        {
            this.transform.GetChild(1).GetComponent<MeshRenderer>().enabled=false;
            this.gameObject.GetComponent<Collider>().enabled=false;
            
            speaker.Play();
            particleSystem.Play();
            hudManager = HUDManager.instance;
            GameManager.instance.score += scorePoints;
            //print("scorePoints = " + GameManager.instance.score.ToString());
            hudManager.RenderScore(GameManager.instance.score);

        }
    }
}
