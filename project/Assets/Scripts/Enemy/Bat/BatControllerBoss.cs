using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;
namespace Assets.Scripts.Enemy
{
public class BatControllerBoss : Enemy {
	public Transform player;
	private bool direction=true;
	public Vector3 destination;
	public float offsetY=1.4f;
	public float step=180;
	public NavMeshAgent batNavMeshAgent;
	private AudioSource sourceMove;
	public GameObject hearthPrefab;
	public GameObject bombPrefab;
	public GameObject currentPayload;
	public float minTargetDistance=1.5f;

	public string moveSound = "BatMove";

	// Use this for initialization	
	void Start () {
		foreach (Transform child in transform.parent) {
			if (child.name=="BatNavMeshAgent"){
				batNavMeshAgent=child.gameObject.GetComponent<NavMeshAgent>();
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
		transform.position=new Vector3(batNavMeshAgent.transform.position.x,batNavMeshAgent.transform.position.y-offsetY,batNavMeshAgent.transform.position.z );
		
		if(destination.x < this.transform.position.x){
			transform.localRotation=Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 270,0), step * Time.deltaTime);
        }else if(destination.x > this.transform.position.x){
			transform.localRotation=Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 90,0), step * Time.deltaTime);
		}
	}

	

	void OnDestroy () {
    	
		Destroy(transform.parent.gameObject);
 	}
	 public IEnumerator DamagePlayer()
	 {
		audioManager.Play(attackSound);
		animator.SetBool("isAttacking", true);
		yield return new WaitForSecondsRealtime(0.4f);
		yield return player.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform);
		animator.SetBool("isAttacking", false);
	 }
	 public void DamageEnemy(Collider other){
		 		audioManager.Play(deathSound,sourceDie,true);
				if(animator != null)
                	animator.SetBool("isDead", true);
                this.ChangeEnemyHp(-1);
                Transform otherGo = other.gameObject.transform;
                Vector3 direction = otherGo.position - this.gameObject.transform.position;
                this.gameObject.GetComponent<Rigidbody>().AddForce(-direction.normalized * 5f, ForceMode.Impulse);
	 }
	 public void SpawnBomb(){
		if(currentPayload==null||currentPayload.transform.parent==null){
			currentPayload = Instantiate(bombPrefab, transform.position - new Vector3(0,-1,0), bombPrefab.transform.rotation);
			currentPayload.transform.parent =transform.GetChild(1).GetChild(0);
		}
	 }
	 public void SpawnHearth(){
		if(currentPayload==null||currentPayload.transform.parent==null){
			currentPayload = Instantiate(hearthPrefab, transform.position - new Vector3(0,-1,0), hearthPrefab.transform.rotation);
			currentPayload.transform.parent =transform.GetChild(1).GetChild(0);
		}
	 }
[Task]
	public bool SetDestination(Vector3 p)
	{
		destination =  p;
		batNavMeshAgent.SetDestination(destination);

		if( Task.isInspected )
			Task.current.debugInfo = string.Format("({0}, {1})", destination.x, destination.y);
		return true;
	}

[Task]
	public void WaitArrival()
	{
		var task = Task.current;
		float d = batNavMeshAgent.remainingDistance;
		if (!task.isStarting && batNavMeshAgent.remainingDistance <= minTargetDistance)
		{
			task.Succeed();
			d = 0.0f;
		}
		
		if( Task.isInspected )
			task.debugInfo = string.Format("d-{0:0.00}", d );
	}
}
/* 
	void OnTriggerEnter(Collider other)
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
		if(other.gameObject.name.Equals("Bombs")){
			if(currentPayload==null||currentPayload.transform.parent==null){
				currentPayload = Instantiate(bombPrefab, transform.position - new Vector3(0,-1,0), bombPrefab.transform.rotation);
				currentPayload.transform.parent =transform.GetChild(1).GetChild(0);
			}
			
		} 
		if(other.gameObject.name.Equals("Hearts")){
			if(currentPayload==null||currentPayload.transform.parent==null){
				currentPayload = Instantiate(hearthPrefab, transform.position - new Vector3(0,-1,0), hearthPrefab.transform.rotation);
				currentPayload.transform.parent =transform.GetChild(1).GetChild(0);
			}
		}
	}
	*/
}