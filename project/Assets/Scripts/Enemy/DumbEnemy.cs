using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class DumbEnemy : Enemy
    {
        public bool moveLeft = true;

        private Ray diagonalChecker;
        private Ray forwardChecker;
        private Ray downChecker;
        private Vector3 movingForceVector;

        //pomocnici za smjer diagonalCheckera
        private Vector3 checkLeftDiagonal;
        private Vector3 checkRightDiagonal;
        private Vector3 checkLeft;
        private Vector3 checkRight;

        private ConstantForce cf;
        public string moveSound = "SlimeMove";

        void Start()
        {
            movingForceVector = moveLeft ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
            checkLeftDiagonal = new Vector3(-1, -1, 0);
            checkRightDiagonal = new Vector3(1, -1, 0);
            checkLeft = new Vector3(-1,0,0);
            checkRight = new Vector3(1,0,0);
            diagonalChecker = new Ray(this.gameObject.transform.position, checkLeftDiagonal);
            forwardChecker = new Ray(this.gameObject.transform.position, checkLeft);
            downChecker = new Ray(this.gameObject.transform.position, Vector3.down);
            cf = this.GetComponent<ConstantForce>();      
            forceIsApplyed = false;
            if (!moveLeft)
            {
                transform.Rotate(0, 180, 0, Space.Self);
            }
            animator = this.transform.GetChild(0).GetComponent<Animator>();
            audioManager = AudioManager.instance;
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

        private void YesForce()
        {
            animator.SetBool("isMoving", false);
            audioManager.Stop(moveSound);
        }

        //Prvo gleda vektor naprijed, a ako se nije zabio o zid onda diagonalni vektor prema podu
        private void NoForce()
        {
            RaycastHit hitInfo;
            animator.SetBool("isMoving",true);

            // gledaj naprijed
            Vector3 checkDiagonal = moveLeft ? checkLeftDiagonal : checkRightDiagonal;
            Vector3 checkForward = moveLeft ? checkLeft : checkRight;
            diagonalChecker = new Ray(this.gameObject.transform.position - new Vector3(0,0.8f,0), checkDiagonal);
            forwardChecker = new Ray(this.transform.position, checkForward);
            downChecker = new Ray(this.gameObject.transform.position, Vector3.down);

            if (cf.force.magnitude != force)
            {
                if (Physics.Raycast(downChecker, out hitInfo, 1.5f))
                {
                    if (hitInfo.collider.CompareTag("Platform"))
                    {
                        cf.force = movingForceVector * force;
                        audioManager.Play(moveSound);
                    }
                }
            }

            if (Physics.Raycast(forwardChecker, out hitInfo, 1.1f))
            {
                print("forward vektor se zabio u:" + hitInfo.collider.tag);
                if (hitInfo.collider.CompareTag("Platform"))
                {
                    ChangeDirection();
                }
            }
            // pazi da ne padnes
            if (Physics.Raycast(diagonalChecker, out hitInfo, 2))
            {
                print("diagonalni vektor se zabio u:" + hitInfo.collider.tag);
                if (hitInfo.collider.CompareTag("Platform"))
                {
                    //if (this.GetComponent<ConstantForce>().force == Vector3.zero)
                    cf.force = movingForceVector * force;
                    audioManager.Play(moveSound);
                }            
            }
            else
            {
                ChangeDirection();
            }
        }

        private void ChangeDirection()
        {
            RaycastHit hitInfo;
            if(!Physics.Raycast(downChecker, out hitInfo, 1.5f)){
                return;
            }
            moveLeft = !moveLeft;
            movingForceVector *= -1;
            StartCoroutine(Rotate());
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(diagonalChecker);
            //Gizmos.color = Color.white;
            Gizmos.DrawRay(forwardChecker);
            Gizmos.DrawRay(downChecker);        
        }


        private IEnumerator Rotate()
        {
            float finalRot = moveLeft ? 0 : 180;
            float rotationProgress = 0;
            while (rotationProgress < 1 && rotationProgress >= 0)
            {
                float rotSpeed = 1;
                rotationProgress += Time.deltaTime * rotSpeed;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(Vector3.up * finalRot), rotationProgress);
                yield return null;
            }
        }
    }
}
