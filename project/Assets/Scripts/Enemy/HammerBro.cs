using System;
using System.Collections;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class HammerBro : Enemy{
        //baca bombe u smijeru playera s silom ovisnom o udaljnosti od player
        //pocinje bacati kada blayer uđe u detectio range

        public float detectionRange;
        private bool isThrowing = false;
        private GameObject player;
        //prebaci u private
        public GameObject throwingObject;
        private bool onGround = true;

        public string throwSound = "MushroomThrow";

        private void Start() {
            this.gameObject.layer = LayerMask.NameToLayer("Enemy");
            player = PlayerManager.instance;
            //animator = this.transform.GetChild(0).GetComponent<Animator>();
            animator = this.GetComponent<Animator>();
            audioManager = AudioManager.instance;
            sourceAttack = this.GetComponents<AudioSource>()[0];
			sourceDie= this.GetComponents<AudioSource>()[1];
        }

        private void Update() {
            //onGround = GetOnGround();
            //player.transform.position.x < this.transform.position.x ? -1 : 1;
            if(player.transform.position.x < this.transform.position.x){
                this.transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
            }
            else{
                this.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);

            }

            if(!forceIsApplyed  && onGround ){
                float dist = (player.transform.position - this.transform.position).magnitude;
                isThrowing = dist<detectionRange? true : false;
                animator.SetBool("isAttacking", isThrowing);
            }
            else{
                animator.SetBool("isAttacking", false);
            }
        }

        private bool GetOnGround()
        {
            Ray down = new Ray(this.transform.position, Vector3.down);
            return Physics.Raycast(down, 1f);
        }
        /*
        private IEnumerator throwObject()
        {                  
            while(true){
                float x = player.transform.position.x < this.transform.position.x? -1 : 1;
                GameObject instance = Instantiate(throwingObject, this.transform.position + new Vector3(x*3f,2f,0), Quaternion.identity);
                instance.GetComponent<Rigidbody>().AddForce(GetForce(x), ForceMode.Impulse);
                yield return new WaitForSeconds(AttackFrequen);
            }
        }
        */
        private Vector3 GetDirection(Vector3 pos)
        {
            //return new Vector3(direction , 0.1f,0)*15;
            return (player.transform.position - pos + Vector3.up).normalized;
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(this.transform.position, detectionRange);
        }

        public void Throw(){
            //audioManager.Play(throwSound);
            audioManager.Play(throwSound, sourceAttack);
            float x = player.transform.position.x < this.transform.position.x ? -1 : 1;
            GameObject instance = Instantiate(throwingObject, this.transform.position + new Vector3(x * 3f, 2f, 0), Quaternion.identity, this.transform.parent);
            force = 20;
            instance.GetComponent<Rigidbody>().AddForce(GetDirection(this.transform.position + new Vector3(x * 3f, 2f, 0)) * force, ForceMode.Impulse);
            //force u smjeru playera ili malo iznad njega
        }
    }
}