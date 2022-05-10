using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour {

	public float lifetime = 10;
	public float startTime;
	private Vector3 startPosition;
	private float speed;
	public float speedscale=0.07f;
	private bool isInit = false;
	public float startAlpha=0.7f;
	public float speedscaleAlpha=0.995f;
	
	// Update is called once per frame

	public void Init(float speed, Vector3 startPosition){
		this.speed = speed;
		this.startPosition = startPosition;
		startTime = Time.time;
		isInit = true;
		
	}

	//ima lifetime
	void Update () {
		if(!isInit) return;
		if(Time.time > startTime + lifetime){
            Destroy(this.gameObject);
        }
		transform.localScale += new Vector3(1, 1,0) * speed *speedscale* Time.deltaTime;		
		startAlpha*=speedscaleAlpha;
		//GetComponent<MeshRenderer>().material.SetFloat("Vector1_BCBF5CD9",startAlpha);
		//GetComponent<MeshRenderer>().material.SetFloat("Vector1_3EF0D060",startAlpha);
		GetComponent<MeshRenderer>().material.SetFloat("Vector1_157D2EFE",startAlpha);
		
	}

	/* private IEnumerator OnCollisionEnter(Collision other){
        if (other.gameObject.CompareTag("Player"))
        {
            print("SHOCKWAVE HIT!");
            yield return other.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform);
        }
	}	
	*/
}
