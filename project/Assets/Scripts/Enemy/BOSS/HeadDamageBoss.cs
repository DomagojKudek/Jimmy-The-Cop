using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Enemy
{
	public class HeadDamageBoss : MonoBehaviour {
		public Boss boss;
			void OnCollisionEnter(Collision other){
				if(this.enabled&&other.gameObject.CompareTag("Bomb")&&other.gameObject.GetComponent<Bomb>().activated==true){
					boss.ChangeHp(-boss.headDamage);
					other.gameObject.GetComponent<Bomb>().Explode();
					//Destroy(other.gameObject,3);
				}
			}
	}
}