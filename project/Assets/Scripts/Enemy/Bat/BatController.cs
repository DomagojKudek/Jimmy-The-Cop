using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Enemy
{
public class BatController : Enemy {
	public Transform player;
	private bool direction=true;
	public float offsetY=1.4f;
	public float step=180;
	public Transform batNavMeshAgent;
	private AudioSource sourceMove;

	public string moveSound = "BatMove";

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform.parent) {
			if (child.name=="BatNavMeshAgent"){
				batNavMeshAgent=child.gameObject.transform;
			}
		}
		player = PlayerManager.instance.transform;
		animator = this.GetComponent<Animator>();
		audioManager = AudioManager.instance;
		sourceAttack = this.GetComponents<AudioSource>()[0];
		sourceDie= this.GetComponents<AudioSource>()[1];
		sourceMove= this.GetComponents<AudioSource>()[2];
		audioManager.Play(moveSound, sourceMove, true);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position=new Vector3(batNavMeshAgent.position.x,batNavMeshAgent.position.y-offsetY,batNavMeshAgent.position.z );
		
		if(player.transform.position.x < this.transform.position.x){
			transform.localRotation=Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 270,0), step * Time.deltaTime);
        }else if(player.transform.position.x > this.transform.position.x){
			transform.localRotation=Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 90,0), step * Time.deltaTime);
        }
	}

	IEnumerator OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player")){
			audioManager.Play(attackSound);
			animator.SetBool("isAttacking", true);
		
        	yield return new WaitForSecondsRealtime(0.4f);
			yield return other.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform);
        	animator.SetBool("isAttacking", false);
			
		}
		if (other.gameObject.CompareTag("Enemy"))
            {
				audioManager.Play(deathSound,sourceDie,true);
				if(animator != null)
                    animator.SetBool("isDead", true);
                this.ChangeEnemyHp(-1);
                Transform otherGo = other.gameObject.transform;
                Vector3 direction = otherGo.position - this.gameObject.transform.position;
                this.gameObject.GetComponent<Rigidbody>().AddForce(-direction.normalized * 5f, ForceMode.Impulse);
            }
	}
	void OnDestroy () {
    	Destroy(transform.parent.gameObject);
 	}
}
}