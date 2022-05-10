using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPushActionCollide : MonoBehaviour {
	private GameObject player;
	public float forceStrength=50f;
	void Start(){
		player = GameObject.FindWithTag("Player");
	}
	/*private void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player") ){
				Vector3 direction = other.transform.position - this.transform.position;
                if (other.GetComponent<Rigidbody>() != null)
                    other.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                StartCoroutine(other.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform));
			this.transform.parent.GetComponent<PushIndicator>().Action();
		}	
		else if (other.CompareTag("Breakable"))
            {
                //disable collisions bettween player and debris
                Physics.IgnoreCollision(player.GetComponent<Collider>(), other.GetComponent<Collider>());
                Destroy(other, 5);
                other.tag = "UnCollidable";
                Vector3 direction = other.transform.position - this.transform.position;
                if (other.GetComponent<Rigidbody>() != null)
                {
                    other.GetComponent<Rigidbody>().useGravity = true;
                    other.GetComponent<Rigidbody>().isKinematic = false;
                    other.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                }
            }	
	}*/

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player")||other.CompareTag("Breakable") ){
			this.transform.parent.GetComponent<PushIndicator>().Action();
		}
	}
}
