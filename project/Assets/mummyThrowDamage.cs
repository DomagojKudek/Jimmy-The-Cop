using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;
public class mummyThrowDamage : MonoBehaviour {

	// Use this for initialization

	void OnCollisionEnter(Collision other){
		Debug.LogWarning(other.gameObject.name,other.gameObject);
			if(other.gameObject.CompareTag("Bomb")){
				//this.transform.parent.GetComponent<MummyThrowController>().ChangeEnemyHp1(-1);
                other.gameObject.GetComponent<Bomb>().Explode();
			}
		}
		
}
