using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class Spiketrap1 : MonoBehaviour {
		public bool open = false;
        public bool disableChange = false;
		public float speed = 5.5f;
        public float distance = 5f;
		public Vector3 direction=Vector3.up;

        private Vector3 openLocation;
        private Vector3 closedLocation;

        private Vector3 moveDirection;

        private Coroutine coroutine = null;
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
        {
			this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            //yield return other.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-100, this.gameObject.transform);
			StartCoroutine(OpenDoor());
        }
	}
	IEnumerator OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Player"))
        {
            yield return other.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-100, this.gameObject.transform);
        }
	}
	private IEnumerator OpenDoor()
        {

            Rigidbody rb = this.GetComponent<Rigidbody>();
            Vector3 endPosition = openLocation;//this.transform.position + openLocation *distance;

            while (!StopClause(moveDirection, endPosition))
            {
                //this.transform.position = Vector3.MoveTowards(this.transform.position, endPosition, Time.deltaTime * speed);
                rb.MovePosition(this.transform.position + moveDirection *Time.deltaTime * speed);
                yield return new WaitForFixedUpdate();
            }
			yield return new WaitForFixedUpdate();
            open = true;
            disableChange = false;
        }
	private bool StopClause(Vector3 vec, Vector3 endPosition)
        {
            //Vector3 vec = moveDirection;
            if(vec == Vector3.up){
                return this.transform.position.y > endPosition.y;
            }
            return false;
        }
	
}
