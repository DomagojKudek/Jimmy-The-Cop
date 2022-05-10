using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Doors
{
 
	public enum moveDirection
     {
         Left,
		 Right,
		 Up,
		 Down,
         Forward,
         Back
    }

    

    //TODO animations
    [RequireComponent(typeof(Rigidbody))]
    public class SlidingDoor : Door
    {	Dictionary<moveDirection,Vector3> directions = new Dictionary<moveDirection, Vector3>()
        {
            {Doors.moveDirection.Left, Vector3.left},
            {Doors.moveDirection.Right, Vector3.right},
            {Doors.moveDirection.Up, Vector3.up},
            {Doors.moveDirection.Down, Vector3.down},
            {Doors.moveDirection.Forward, Vector3.forward},
            {Doors.moveDirection.Back, Vector3.back}
        };

        public float speed = 5.5f;
        public float distance = 5f;
		public moveDirection direction;

        private Vector3 openLocation;
        private Vector3 closedLocation;

        private Vector3 moveDirection;

        private Coroutine coroutine = null;

        private void Start()
        {
            openLocation = this.transform.position + directions[direction] * distance;//directions[direction];
            closedLocation = this.transform.position;
            moveDirection = directions[direction];
        }
        public override void Open()
        {
            disableChange = true;
            if(coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(OpenDoor());       
        }

        public override void Close()
        {
            disableChange = true;
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine =  StartCoroutine(CloseDoor());
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
            open = true;
            disableChange = false;
        }

        private IEnumerator CloseDoor()
        {
            
            Rigidbody rb = this.GetComponent<Rigidbody>();
            Vector3 endPosition = closedLocation;//this.transform.position + closedLocation * distance;

            while (!StopClause(moveDirection * -1, endPosition))
            {
                //this.transform.position = Vector3.MoveTowards(this.transform.position, endPosition, Time.deltaTime * speed);
                rb.MovePosition(this.transform.position + moveDirection * -1 * Time.deltaTime * speed);
                yield return new WaitForFixedUpdate();
            }
            open = false;
            disableChange = false;
        }

        private bool StopClause(Vector3 vec, Vector3 endPosition)
        {
            //Vector3 vec = moveDirection;
            if(vec == Vector3.up){
                return this.transform.position.y > endPosition.y;
            }
            else if(vec == Vector3.down){
                return this.transform.position.y < endPosition.y;
            }
			else if(vec == Vector3.right){
                return this.transform.position.x > endPosition.x;
            }
			else if(vec == Vector3.left){
                return this.transform.position.x < endPosition.x;
            }
            else if (vec == Vector3.forward)
            {
                return this.transform.position.z > endPosition.z;
            }
            else if (vec == Vector3.back)
            {
                return this.transform.position.z < endPosition.z;
            }
            return false;
        }
    }
}
