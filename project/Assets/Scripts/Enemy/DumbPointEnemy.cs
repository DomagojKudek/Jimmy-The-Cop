using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ConstantForce))]
    public class DumbPointEnemy : Enemy
    {
        public bool moveLeft;
        public GameObject pointLeft;
        public GameObject pointRight;
        private Vector3 movingForceVector;
        private Ray downChecker;
        private ConstantForce cf;
        public string moveSound = "SlimeMove";
		private AudioSource sourceMove;


        void Start()
        {
            forceIsApplyed = false;
            //downChecker = new Ray(this.gameObject.transform.position, Vector3.down);
            movingForceVector = moveLeft ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
            cf = this.GetComponent<ConstantForce>();
            //cf.force = movingForceVector * force;
            if(moveLeft){
                transform.Rotate(0, 180, 0, Space.Self);
            }
            animator = this.transform.GetComponent<Animator>();
            audioManager = AudioManager.instance;

            sourceAttack = this.GetComponents<AudioSource>()[0];
			sourceDie= this.GetComponents<AudioSource>()[1];
			sourceMove= this.GetComponents<AudioSource>()[2];

        }



        private void YesForce()
        {
            audioManager.Stop(moveSound);
        }
        private void NoForce()
        {
            //Idejno da se vrti yes force dok enemy pada
            downChecker = new Ray(this.gameObject.transform.position, Vector3.down);
            RaycastHit hitInfo;

            
            if (cf.force.magnitude != force)
            {
                if (Physics.Raycast(downChecker, out hitInfo, 1.5f))
                {
                    if (hitInfo.collider.CompareTag("Platform"))
                    {
                        cf.force = movingForceVector * force;
                        audioManager.Play(moveSound, sourceMove);
                    }
                }
            }
            else if(!Physics.Raycast(downChecker, out hitInfo, 1.5f)){
                cf.force = Vector3.zero;
                audioManager.Stop(moveSound);

            }
            

            if (moveLeft && pointLeft.transform.position.x > this.transform.position.x)
            {
                ChangeDirection();
            }
            if(!moveLeft && pointRight.transform.position.x < this.transform.position.x){
                ChangeDirection();
            }
        }
        private void Update()
        {
            if (forceIsApplyed)
            {
                YesForce();
            }
            else
            {
                NoForce();
            }
        }

        private void ChangeDirection()
        {
            moveLeft = !moveLeft;
            movingForceVector *= -1;
            cf.force *= -1;
            StartCoroutine(Rotate());
        }

        private IEnumerator Rotate()
        {
            float finalRot = moveLeft? 270 : 90;
            float rotationProgress = 0;
            while(rotationProgress < 1 && rotationProgress >= 0){
                float rotSpeed = 1;
                rotationProgress += Time.deltaTime * rotSpeed;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(Vector3.up * finalRot), rotationProgress);
                yield return null;
            }
        }


        
    }
}
