using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
namespace Assets.Scripts.Enemy
{
	public class MummyThrowController : Enemy {

		// Use this for initialization
		public float detectionRange=20;
			private bool isThrowing = false;
			private GameObject player;
			//prebaci u private
			public GameObject bombPrefab;
			public Transform bombParent;
			private bool onGround = true;

			public string throwSound = "MushroomThrow";
			private List<GameObject> bombList;
			private bool faceLeft=false;
			private bool throwing=false;
			public float rotSpeed=1f;
			public float forceThrow=100f;
		void Start () {
			
			bombList = new List<GameObject>();

			player = PlayerManager.instance;
			//animator = this.transform.GetChild(0).GetComponent<Animator>();
			animator = this.GetComponent<Animator>();
			audioManager = AudioManager.instance;
			sourceAttack = this.GetComponents<AudioSource>()[0];
			sourceDie= this.GetComponents<AudioSource>()[1];
		}
		
		// Update is called once per frame
		void Update () {
			if(!throwing&&!faceLeft &&player.transform.position.x < this.transform.position.x){
				ChangeDirection();
					//this.transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
					faceLeft=true;
				}
				else if(!throwing&& faceLeft &&player.transform.position.x > this.transform.position.x){
					ChangeDirection();
					//this.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
					faceLeft=false;

				}

				//if(!forceIsApplyed  && onGround ){
					float dist = (player.transform.position - this.transform.position).magnitude;
					isThrowing = dist<detectionRange? true : false;
					if(isThrowing&&!throwing){
						animator.SetTrigger("isAttacking");
						throwing=true;
					}
				//}
				//else{
					//animator.SetBool("isAttacking", false);
				//}
		}
		public void ChangeEnemyHp1(int n){
            bool isDmg = n < 0;
            if (isDmg)
            {
                if (hp + n <= 0)
                {
					sourceAttack.enabled=false;
                    // BITNO mora imati death animaciju da bi radio
                    if(animator != null)
                        animator.SetTrigger("isDead");
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
                    Destroy(this.gameObject, 4);
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
	
		public void SpawnBombsEvent(){
			bombList.Clear();
			print("Spawn bombs");
			Vector3 bombInitalPosition = bombParent.transform.position;
			bombInitalPosition.z = 0;
			//GameObject bombInstance = Instantiate(bombPrefab, bombInitalPosition, Quaternion.identity, bombParent);
			GameObject bombInstance = Instantiate(bombPrefab, bombParent);
			//bombInstance.transform.localScale = new Vector3(20,20,20);
			bombList.Add(bombInstance);
			
		}

		public void ThrowBombsEvent(){
			int directionX = faceLeft ? -1 : 1;

			GameObject bomb = bombList[0];
			bomb.transform.parent = null;
			bomb.layer = 11; //IgnoreCollisions, layer ignorira samo sebe
			bomb.transform.position = new Vector3(bomb.transform.position.x, bomb.transform.position.y, 0);
			StartCoroutine(bomb.GetComponent<Bomb>().ActivateBomb());
			//TODO pravi vektor
			Vector3 normalOnHand = new Vector3(directionX , 1, 0);
			bomb.GetComponent<Rigidbody>().isKinematic = false;
			bomb.GetComponent<Rigidbody>().AddForce(normalOnHand.normalized * forceThrow* ((0+1) * 0.1f), ForceMode.Impulse);
			
			
		}
		private Vector3 GetDirection(Vector3 pos)
			{
				//return new Vector3(direction , 0.1f,0)*15;
				return (player.transform.position - pos + Vector3.up).normalized;
			}
		private void ChangeDirection()
        {
            faceLeft = !faceLeft;
            StartCoroutine(Rotate());
        }
		private IEnumerator Rotate()
        {
            float finalRot = faceLeft? 270 : 90;
            float rotationProgress = 0;
            while(rotationProgress < 1 && rotationProgress >= 0){
                //float rotSpeed = 1;
                rotationProgress += Time.deltaTime * rotSpeed;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(Vector3.up * finalRot), rotationProgress);
                yield return null;
            }
        }
		public void ThrowEnd(){
			throwing=false;
			bombList[0].layer = 1;
			
			
		}
		void OnCollisionEnter(Collision other){
		//Debug.LogWarning(other.gameObject.name,other.gameObject);
			//if(other.gameObject.CompareTag("Bomb")){
				//this.transform.parent.GetComponent<MummyThrowController>().ChangeEnemyHp1(-1);
                //other.gameObject.GetComponent<Bomb>().Explode();
			//}
		}
	}
}