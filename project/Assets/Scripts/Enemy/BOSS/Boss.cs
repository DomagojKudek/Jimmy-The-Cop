using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Enemy
{
	public class Boss : Enemy {
		private AudioManager audioManager;
		public Animator anim;
		public int headDamage=3;
		public int bodyDamage=1;

		public Vector3 startPosition;

		private int maxHp;

		public Slider hpBar;

		private BossController bossController;

		void Start(){
			startPosition = this.transform.position;
			audioManager = AudioManager.instance;
			audioManager.Play("BossStart");
			audioManager.Play("BossBackground");
			hpBar.minValue = 0;
            hpBar.maxValue = hp;
			hpBar.value = hp;
			maxHp = hp;
            bossController = this.gameObject.GetComponent<BossController>();
			//EnableBossCanvas();
        }

		void OnCollisionEnter(Collision other){
			if(this.enabled&&other.gameObject.CompareTag("Bomb")&&other.gameObject.GetComponent<Bomb>().activated==true){
				ChangeHp(-bodyDamage);
                other.gameObject.GetComponent<Bomb>().Explode();
			}
		}

		public void ChangeHp(int n){
			bool isDmg = n < 0;
			if (isDmg)
			{
				if (hp + n <= 0)
				{
					audioManager.Play("BossDying");
					//Todo Death animation
					hp = 0;
					RenderHp();
					Death();
				}
				else
				{
					anim.SetBool("hit", true);
					//TODO dmg animacija
					audioManager.Play("BossHurt");
					hp += n;
					if(hp <= maxHp/2){
                        //stage 2
                        bossController.ChangeStage();
						audioManager.Stop("BossBackground");
						audioManager.Play("BossBackground2");
						//TODO animacija i zvuk
					}
                    //audioManager.Play("Grunt");
                    RenderHp();
                }
			}
			else
			{
				//Todo healing animacija
				hp += n;
                RenderHp();
            }
    	}

		private void RenderHp(){
			hpBar.value = hp;
		}

		public void EnableBossCanvas(){
			hpBar.transform.parent.gameObject.SetActive(true);
		}

        public void DisableBossCanvas()
        {
            hpBar.transform.parent.gameObject.SetActive(false);
        }

		private void Death(){
			VictoryScreen();
			anim.Play("Death", 0);
			Destroy(this.gameObject, 8);
			
		}

		private void Update(){
			if(Input.GetKeyDown(KeyCode.K)){
				this.ChangeHp(-1);
				print("Boss hp: " + hp);
			}
		}

		private void VictoryScreen(){
            hpBar.transform.parent.GetChild(1).gameObject.SetActive(true);
			//Todo victory sound
		}

		private void OnDestroy(){
            hpBar.transform.parent.GetChild(1).gameObject.SetActive(false);
		}

		public void Restart(){
			hpBar.value = maxHp;
			this.hp = maxHp;
			DisableBossCanvas();
			this.transform.position = startPosition;
			bossController.ResetStage();
			//reset muziku
			audioManager.Stop("BossBackground2");
			audioManager.Play("BossBackground");
			bossController.GarbageCollectAll();



			//this.gameObject.SetActive(false);
		}

		private void OnEnable(){
			EnableBossCanvas();
		}
	}
}