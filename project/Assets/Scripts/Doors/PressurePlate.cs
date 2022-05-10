using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Doors
{
    //TODO pazi koji objekt moze triggerat vrata
    public class PressurePlate : MonoBehaviour
    {
		List<Collider> collidedObjects = new List<Collider>();
        public List<Door> doors;
        private Door firstDoor;
        //tags that interact with plate
        private HashSet<string> tags = new HashSet<string>(){"Player", "Enemy", "Object"};

        private Material unpressedMaterial;
        private Material pressedMaterial;
        public GameObject textHint;
        public AudioClip pressingSound;
        public AudioClip unpressingSound;
	    private AudioSource speaker;
        private Color startColor;
        void Start() {
            if(doors.Count>0){
                firstDoor=doors[0];
            }
            speaker = GetComponent<AudioSource>();
            unpressedMaterial=gameObject.GetComponent<Renderer>().material;
            startColor=unpressedMaterial.color;
           /*  if (pressedMaterial==null){
                pressedMaterial=new Material(gameObject.GetComponent<Renderer>().material);
                pressedMaterial.color=Color.green;            
            } */
        }
        void OnTriggerEnter(Collider other)
        {
            if (!firstDoor.open && other.tag == "Player"&& textHint!=null){
                textHint.SetActive(true);
            }
			collidedObjects.Add(other);
            if(collidedObjects.Count==1/* && door.disableChange == false*/ && tags.Contains(other.tag)){
                    //firstDoor.Open();
                    foreach(Door door in doors){
                        door.Open();
                    }
                    if(!speaker.isPlaying ){
                        speaker.clip=pressingSound;
                        speaker.Play();
                    }
                    //gameObject.GetComponent<Renderer> ().material = pressedMaterial;
                    gameObject.GetComponent<Renderer> ().material.color=Color.green;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player"&& textHint!=null){
                textHint.SetActive(false);
            }
            if(collidedObjects.Count==1/*  && door.disableChange == false*/ && tags.Contains(other.tag)){
                //firstDoor.Close();
                foreach(Door door in doors){
                        door.Close();
                }
                if(!speaker.isPlaying ){
                        speaker.clip=unpressingSound;
                        speaker.Play();
                    }
                //gameObject.GetComponent<Renderer> ().material = unpressedMaterial;
                gameObject.GetComponent<Renderer> ().material.color = startColor;
            }
			collidedObjects.Remove(other);
        }        
    }
}
