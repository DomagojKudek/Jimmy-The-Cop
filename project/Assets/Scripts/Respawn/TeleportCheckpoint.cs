using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;


namespace Respawn{
	public class TeleportCheckpoint : Checkpoint {
        
        //public LevelSelect levelSelect;
        public UnlockedLevels unlocked;
        public int areaNumber;
         protected void Start()
        {
           
            material = this.GetComponent<MeshRenderer>().material;
            SetColor(Color.red);

            collider = this.GetComponent<Collider>();
            collider.isTrigger = true;

			checkpointSystem = CheckpointSystem.GetInstance();
            speaker=GetComponent<AudioSource>();
            speaker.clip=checkpointSound;
        }
        void OnTriggerEnter(Collider coll)
        {
            if(!coll.CompareTag("Player")) return;
            if (isActive == true) return;

            checkpointSystem.SetActiveCheckpoint(this);
            SetColor(Color.green);

            Area next = AreaManager.instance.Next(AreaManager.instance.GetActiveArea());
            //AreaManager.instance.SetActiveArea(next);

            LoadSystem.Save("bla", GameManager.instance.score);
            print("GAME SAVED");

            //levelSelect.EnableSelectAreaButton(areaNumber);
            unlocked.unlockedLevels[areaNumber] = 1;

        }
	}
}
