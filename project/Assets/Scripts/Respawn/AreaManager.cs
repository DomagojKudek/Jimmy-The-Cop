using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

namespace Respawn{
	public class AreaManager : MonoBehaviour {
		public List<Respawn.Area> areas;

		private Respawn.Area activeArea;

		[HideInInspector]
		public static AreaManager instance;

		private void Awake(){
			if(instance == null){
				instance = this;
				//activeArea = AreaManager.GetArea("Area1");
				/*
				//TODO promijeni
                areas.ForEach(a => a.gameObject.SetActive(false));
                activeArea = AreaManager.GetArea("Area1");
				activeArea.gameObject.SetActive(true);
				*/
			}
			else{
				Destroy(this.gameObject);
			}
		}

        public static Respawn.Area GetArea(string name)
        {
            switch (name)
            {
                case "Area1":
					//print("Area1 = " + instance.areas[0]);
                    return instance.areas[0];
                case "Area2":
					//print("Area2 = " + instance.areas[1]);

                    return instance.areas[1];
                case "Area3":
					//print("Area3 = " + instance.areas[2]);

                    return instance.areas[2];
                default:
                    return null;
            }
        }

		public void SetActiveArea(Respawn.Area area){
			if(activeArea == null){
				activeArea = area;
				//disable sve osim active area
				/*
				for(int i = 0; i < areas.Count; i++){
					if(areas[i].name == activeArea.name) continue;
					areas[i].gameObject.SetActive(false);
                    areas[i].puzzles.SetActive(false);
				}
				*/
			}
			else{
                //activeArea.gameObject.SetActive(false);
                //activeArea.puzzles.SetActive(false);
                activeArea = area;
                //area.gameObject.SetActive(true);
                //activeArea.puzzles.SetActive(true);
			}
		}

		public Respawn.Area GetActiveArea(){
			return activeArea;
		}

		public Respawn.Area Next(Respawn.Area area){

            int index = areas.FindIndex(a => a.Equals(area));
			print("index = " + index);
			switch(index){
				case 0:
					return areas[1];
				case 1:
					return areas[2];
				case 2:
					return areas[0];
				default:
					return areas[0];
			}
		}

	}
}