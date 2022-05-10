using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;
public class BatColliderControl : MonoBehaviour {
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	private BatControllerBoss controller;
	void Start()
	{
		controller=this.transform.parent.parent.GetComponent<BatControllerBoss>();
	}
	// Use this for initialization
	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")){
			StartCoroutine(controller.DamagePlayer());
		}
		if (other.gameObject.CompareTag("Enemy"))
            {
				controller.DamageEnemy(other);
            }
		if(other.gameObject.name.Equals("Bombs")){
			controller.SpawnBomb();
		} 
		if(other.gameObject.name.Equals("Hearts")){
			controller.SpawnHearth();
		}
	}
	
}
