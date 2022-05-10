using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Respawn{
	public class Area : MonoBehaviour {

		public Transform areaStart;
		public GameObject puzzles;
		public GameObject puzzlesPrefab;

		private List<GameObject> activePuzzles;

		public bool isActive = false;

		//restartaj sve pazle - neefikasno
		public void ReloadArea(){
			Debug.LogWarning("p",puzzles);
			Destroy(puzzles);
			puzzles = GameObject.Instantiate(puzzlesPrefab, puzzlesPrefab.transform.position, puzzlesPrefab.transform.rotation);
			Debug.LogWarning("p",puzzlesPrefab);
		}
	}
}
