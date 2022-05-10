using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;

namespace Assets.Scripts.Enemy
{
    public class SpikeBall : Enemy {
    private bool vibrate_on=false;
    public float forceStrength=10f;
    private float breakableFadeDuration = 5f;
    public float radius=3f;
    public string BreakSound = "Break";
    private GameObject player;
	// Use this for initialization
	void Start () {
		audioManager = AudioManager.instance;
        
        player = PlayerManager.instance;
    
	}
	private IEnumerator OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Player"))
            {
                yield return other.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-10, this.gameObject.transform);

            }
            if (other.gameObject.CompareTag("Enemy"))
            {
                //zvuk za
                this.ChangeEnemyHp(-1);
                Transform otherGo = other.gameObject.transform;
                Vector3 direction = otherGo.position - this.gameObject.transform.position;
                this.gameObject.GetComponent<Rigidbody>().AddForce(-direction.normalized * 2f, ForceMode.Impulse);
            }
            if (other.gameObject.CompareTag("Breakable")){
                audioManager.Play(BreakSound);
                {Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius); 
                foreach (Collider col in colliders){
                    if (col.gameObject.CompareTag("Breakable")){
                   // GameObject go = other.gameObject;
                    GameObject go = col.gameObject;
                    Physics.IgnoreCollision(this.GetComponent<Collider>(), go.GetComponent<Collider>());
                    Physics.IgnoreCollision(player.GetComponent<Collider>(), go.GetComponent<Collider>());
                    go.layer=9;
                    Vector3 direction = go.transform.position - this.transform.position;
                    Destroy(go, breakableFadeDuration+5);
                    go.tag = "UnCollidable";
                        
                        if (go.GetComponent<Rigidbody>() != null)
                        {
                            go.GetComponent<Rigidbody>().useGravity = true;
                            go.GetComponent<Rigidbody>().isKinematic = false;
                            go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                            vibrate_on=true;
                            col.transform.parent.parent.GetComponent<ToggleFractureVisibility>().broken=true;
                        }
                    }
                }
            }
        }
        }

}
}