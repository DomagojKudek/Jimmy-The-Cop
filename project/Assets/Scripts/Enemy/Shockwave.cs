using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour {
	
	private bool moveLeft;
	//private int maxDistance = 50;

	public float lifetime = 10;
	public float startTime;
	private Vector3 startPosition;
	public float speedscale=0.1f;
	public float speed1=1.2f;
	private float speed;
	
	private bool isInit = false;
	// Update is called once per frame

	public void Init(bool moveLeft, float speed, Vector3 startPosition){
		this.moveLeft = moveLeft;
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
		int direction = moveLeft ? -1 : 1;
		//transform.position += new Vector3(direction, 0, 0) * speed*speed1 * Time.deltaTime;
		transform.localScale += new Vector3(1, 0,0) * speed *speedscale* Time.deltaTime;		
	}

	private IEnumerator OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player"))
        {
            print("SHOCKWAVE HIT!");
            yield return other.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform);
        }
	}	
}
