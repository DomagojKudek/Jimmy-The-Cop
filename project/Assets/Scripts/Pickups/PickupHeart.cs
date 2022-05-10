using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Pickups
{
    class PickupHeart : Pickup
    {
        private PlayerHealth playerHealthScript;
        public AudioClip heartSound;
	    private AudioSource speaker;
        public ParticleSystem particleSystem;
		
		void Start(){
            GameObject player= GameObject.FindGameObjectWithTag("Player");
            playerHealthScript = player.GetComponent<PlayerHealth>();
            speaker=GetComponent<AudioSource>();
            speaker.clip=heartSound;
            if(this.transform!=null){
                if(this.transform.GetChild(0)!=null){
                                particleSystem=this.transform.GetChild(0).GetComponent<ParticleSystem>();
                            }
            }
           
           
		}
        public override void OnPickup()
        {
            speaker.Play();
            if(particleSystem!=null){
                particleSystem.Play();
            }
            
            this.gameObject.GetComponent<MeshRenderer>().enabled=false;
            this.gameObject.GetComponent<Collider>().enabled=false;
            
            if(this.gameObject.name.Equals("HearthPickup")){
                Destroy(this.gameObject.transform.parent.gameObject,1);
            }
            playerHealthScript.ChangeHp(1);
        }
    }
}
