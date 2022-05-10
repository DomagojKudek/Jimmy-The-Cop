using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class PositionFromPlayer : MonoBehaviour
    {
        private float startDistanceFromPlayer;
        private Transform parent;
        private Vector3 pointerDirection;
        //private Push push;
        private void Start(){
            //push = this.GetComponent<Push>();
            pointerDirection = GameManager.instance.pointerDirection;
            parent = this.transform.parent;
            startDistanceFromPlayer = Vector3.Distance(this.transform.position, parent.position);
        }

        private void Update(){
            pointerDirection = GameManager.instance.pointerDirection;
            this.transform.position = FindStartPushPosition();
            this.transform.rotation = GetRotation(this.transform.rotation);
        }
        private Vector3 FindStartPushPosition()
        {
            return parent.position - pointerDirection.normalized * (startDistanceFromPlayer);
        }

        private Quaternion GetRotation(Quaternion lastRotation){
            return Quaternion.FromToRotation(new Vector3(1,0,0), pointerDirection);
        }

        void OnDrawGizmos(){
            Gizmos.DrawSphere(this.transform.position + pointerDirection, 0.2f);
        }

    }
}