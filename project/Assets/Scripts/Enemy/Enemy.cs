using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ConstantForce))]
    public abstract class Enemy : MonoBehaviour
    {
        [HideInInspector]
        public bool forceIsApplyed;
        public float force = 0f;
        public int hp = 1;
        public int dmg = 1;
        protected AudioManager audioManager;
        protected AudioSource sourceDie;
		protected AudioSource sourceAttack;

        public string attackSound = "SlimeAttack";
        public string deathSound = "SlimeDeath";
        protected Animator animator;
        private IEnumerator OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Player"))
            {
                audioManager.Play(attackSound,sourceAttack);

                //other.gameObject.GetComponent<PlayerHealth>().ChangeHp(-1);// ne radi ako stavis dmg??
                if (animator != null)
                    animator.SetBool("isAttacking", true);
                yield return other.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform);
                if (animator != null)
                    animator.SetBool("isAttacking", false);

            }
            /*
            if (other.gameObject.CompareTag("Enemy"))
            {
                //zvuk za
                if (sourceAttack != null)
                {
                    //audioManager.Play(attackSound, sourceAttack, true);
                }
                this.ChangeEnemyHp(-1);
                Transform otherGo = other.gameObject.transform;
                Vector3 direction = otherGo.position - this.gameObject.transform.position;
                this.gameObject.GetComponent<Rigidbody>().AddForce(-direction.normalized * 2f, ForceMode.Impulse);
            }
            */
        }

        //Za animacije potrebno overridati u specificnom enemy-u
        public void ChangeEnemyHp(int n){
            bool isDmg = n < 0;
            if (isDmg)
            {
                if (hp + n <= 0)
                {
					sourceAttack.enabled=false;
                    // BITNO mora imati death animaciju da bi radio
                    if(animator != null)
                        animator.SetBool("isDead", true);
                    if(deathSound != null && sourceDie != null){
                        audioManager.Play(deathSound, sourceDie);
							sourceDie.Play();
                    }
                    else{
                        Debug.LogError(this.name + "no death sound or audio source");
                    }
                    // da stane na mjestu
                    this.forceIsApplyed = true;
                    this.gameObject.GetComponent<ConstantForce>().force = Vector3.zero;
                    hp = 0;
                    
                    if(this.gameObject.name=="Mummy_Mon"){
                        Destroy(this.gameObject, 4);
                        
                   }else{
                    Destroy(this.gameObject, 1);
                   }
                }
                else
                {
                    //TODO dmg animacija
                    hp += n;
                }
            }
            else
            {
                //Todo healing animacija
                hp += n;
            }
        }
    }
}
