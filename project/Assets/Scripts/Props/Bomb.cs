using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;
public class Bomb : MonoBehaviour {
		
	public AudioClip fuseSound;
	
	public AudioClip bombExplosionSound;
	private AudioSource speaker;
	public float fuseduration=5;
	public float blastRadius=10;
	public bool exploded=false;
	public ParticleSystem particleExplosionSystem;
	public ParticleSystem particleSparkSystem;
	public bool activated=false;
	
	// Use this for initialization
	void Start () {
		speaker=GetComponent<AudioSource>();
		particleExplosionSystem=this.transform.GetChild(0).GetComponent<ParticleSystem>();
		particleSparkSystem=this.transform.GetChild(1).GetComponent<ParticleSystem>();
	}
	 void OnCollisionEnter(Collision other)
	{
		//other.gameObject.GetComponent<MummyThrowController>().ChangeEnemyHp1(-1);
		//Explode();
	}
	public IEnumerator ActivateBomb(){
		activated=true;
		speaker.clip=fuseSound;
		speaker.Play();
		particleSparkSystem.Play();
		yield return new WaitForSeconds(fuseduration);
		if(!exploded){
			Explode();
		}
		
	}
	public void Explode(){
		if(!exploded){
			exploded=true;
			particleSparkSystem.Stop();
			this.gameObject.GetComponent<MeshRenderer>().enabled=false;
			this.gameObject.GetComponent<Collider>().enabled=false;
			speaker.clip=bombExplosionSound;
			speaker.Play();
			particleExplosionSystem.Play();
			Collider[] colliders = Physics.OverlapSphere(this.transform.position, blastRadius);

			foreach (Collider col in colliders)
			{
			
				GameObject go = col.gameObject;
				if (go.CompareTag("Player"))
				{
					StartCoroutine(go.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform));
					Destroy(this.gameObject,2);
				}
				if (go.CompareTag("mummyThrow"))
				{
					go.transform.parent.GetComponent<MummyThrowController>().ChangeEnemyHp1(-1);
					Destroy(this.gameObject,4);
				}
				if (go.name=="Mummy_Mon")
				{
					go.GetComponent<DumbPointEnemy>().ChangeEnemyHp(-1);
					//Destroy(this.gameObject,4);
				}
				
			}
			Destroy(this.gameObject,2);
		}
	}
	
	void OnDrawGizmos()
     {
         Gizmos.DrawWireSphere(this.transform.position, blastRadius);
     }
}
