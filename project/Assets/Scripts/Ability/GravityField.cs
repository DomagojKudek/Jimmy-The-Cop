using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum GravityMode
    {
        Low,
        High
    }

    public class GravityField : MonoBehaviour
    {
        public float radius = 0f;
        public float forceStrength = 0f;
        public int floorUsage = 80;
        public GravityMode gravityMode = GravityMode.Low;
        public KeyCode keyCode = KeyCode.E;
        public AbilityIcon icon;
		public KeyCode joystickGravityButton = KeyCode.JoystickButton2;
        [SerializeField]
        private bool on_off = false;
        public GameObject gravityBall;
        public GameObject gravityHexagonalBall;
        public GravityShaderControl gravityShaderControl;
        public GravityFieldHexagonalControl gravityFieldHexagonalControl;

		public Animator anim;

        private List<Rigidbody> rigidbodies;

        private AudioManager audioManager;

        public string gravityStartSound = "GravityStart";
        public string gravityEndSound = "GravityEnd";


        private void Action()
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius*2);
            //Collider[] colliders = Physics.OverlapBox(this.transform.position, new Vector3(radius / 2, 100, radius / 2));
            foreach (Collider col in colliders)
            {
                GameObject go = col.gameObject;
                //TODO svi tagovi na koje djeluje

                if (!(go.tag.Equals("Enemy") || go.tag.Equals("Object")|| go.tag.Equals("Debris")|| go.CompareTag("Bomb"))) continue;
                Vector3 direction = Vector3.up;
                if (gravityMode == GravityMode.High) direction *= -1;
                if (go.GetComponent<Rigidbody>() != null && go.GetComponent<ConstantForce>() != null)
                {
                    if (Vector3.Distance(go.transform.position, this.transform.position) < radius)
                    {                        
                        // Enemy unutar gravity fielda
                        go.GetComponent<ConstantForce>().force = direction * forceStrength;
                        
                        rigidbodies.Add(go.GetComponent<Rigidbody>());
                        // treba test ako je enemy
                        if(go.GetComponent<Enemy.Enemy>() != null && !go.GetComponent<Enemy.Enemy>().forceIsApplyed)
                            go.GetComponent<Enemy.Enemy>().forceIsApplyed = true;
                    }
                    else
                    {                        
                        // Enemy izvan gravity fielda
                        go.GetComponent<ConstantForce>().force = Vector3.zero;
                        rigidbodies.Remove(go.GetComponent<Rigidbody>());
                        if(go.GetComponent<Enemy.Enemy>() != null)
                            go.GetComponent<Enemy.Enemy>().forceIsApplyed = false;
                        
                    }
                }
            }
        }

        void RemoveAddedForce()
        {
            
            foreach (Rigidbody rb in rigidbodies)
            {
                if(rb != null){
                    //rb.velocity = Vector3.zero;
                    rb.gameObject.GetComponent<ConstantForce>().force = Vector3.zero;
                    if(rb.GetComponent<Enemy.Enemy>() != null)
                        rb.GetComponent<Enemy.Enemy>().forceIsApplyed = false;
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }

        void Start()
        {
            rigidbodies = new List<Rigidbody>();
            //gravityShaderControl=gravityBall.GetComponent<GravityShaderControl>();
            gravityFieldHexagonalControl=gravityHexagonalBall.GetComponent<GravityFieldHexagonalControl>();
            audioManager = AudioManager.instance;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(keyCode)||Input.GetKeyDown(joystickGravityButton))
            {
                //on_off = !on_off;
                if(!on_off){
                    if(icon.CheckIfUsable(floorUsage)){
                        on_off = true;
                        //gravityShaderControl.toggleGravityField();
						
						anim.SetBool("Gravity",true);
						
                        gravityFieldHexagonalControl.toggleGravityField();

                        icon.PauseUpdateSlider();
                        audioManager.Play(gravityStartSound);
                    }
                }
                else
                {
                    RemoveAddedForce();
                    //gravityShaderControl.toggleGravityField();
                    gravityFieldHexagonalControl.toggleGravityField();
                    on_off = false;
                    icon.ContinueSlider();
                    audioManager.Play(gravityEndSound);
                }

            }
            
            if (on_off)
            {
                if (icon.CheckIfUsable(1))
                {
                    Action();
                    icon.UseCharges(1);
                }
                else
                {
                    on_off = !on_off;
                    RemoveAddedForce();
                    icon.ContinueSlider();
                    //gravityShaderControl.toggleGravityField();
                    gravityFieldHexagonalControl.toggleGravityField();

                }
            }
        }

        void OnDisable(){
            RemoveAddedForce();
        }
    }
}
