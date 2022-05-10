using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure; // Required in C#

namespace Assets.Scripts
{
    public class Pull : MonoBehaviour
    {
        public float radius = 5f;
        public float ForceMin = 5f;
        public float ForceMid = 10f;
        public float forceHigh = 20f;
        public float forceChargeTimeMid = 0.6f;
        public float forceChargeTimeHigh = 1.2f;

        public float launchForce=0.25f;

        public float angle = 30f;
        public AbilityIcon icon;
        //public float forceStrength = 0f;
        public KeyCode keyCodePull = KeyCode.Alpha2;
        public KeyCode joystickPullButton = KeyCode.JoystickButton4;

        private float breakableFadeDuration = 5f;
        public Material fade_material;

        [HideInInspector]
        public float forceChargeTimerPull = 0f;

        //public GameObject fracturedVersion;

        //[HideInInspector]
        //public Vector3 pointerDirection;

        //private Slider chargeMeter;
        private ChargeMeter chargeMeter;

        private bool vibrate_on=false;
        public float vibration_duration=0.7f;
        public float left_vibration_rumble_strength=1.0f;
        public float right_vibration_noise_strength=0.5f;
        public GameObject cinemachineShakeObj;
        private CameraShakeCinemamachine cinemachineShake;
        private AudioManager audioManager;

        public string PullSound = "Pull";
        public string BreakSound = "Break";
        public ParticleSystem particleSystem;

        private Camera mainCamera;
		
		public Animator anim;

        private void Start()
        {
            cinemachineShake = cinemachineShakeObj.GetComponent<CameraShakeCinemamachine>();
            //Canvas canvas = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetComponent<Canvas>();

            chargeMeter = this.GetComponent<ChargeMeter>();
            float scaledMid = forceChargeTimeMid / forceChargeTimeHigh;
            float scaledMax = 1f;
            chargeMeter.Initialize(scaledMid, scaledMax);
            audioManager = AudioManager.instance;

            mainCamera = Camera.main;
        }

        private void Action(ForceDirection forceDirection, float forceStrength)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
            GameObject player=GameObject.FindWithTag("Player");
            foreach (Collider col in colliders)
            {
                GameObject go = col.gameObject;
                Vector3 direction;

                if (go.CompareTag("Breakable") && forceStrength >= forceHigh)
                {
                    //disable collisions bettween player and debris
                    Physics.IgnoreCollision(player.GetComponent<Collider>(), go.GetComponent<Collider>());
                    Destroy(go, breakableFadeDuration);
                    //fade object
                   // Color  transparent = go.GetComponent<MeshRenderer>().material.color;
                    //transparent.a=0;
                   StartCoroutine(Lerp_MeshRenderer_Color(go.GetComponent<MeshRenderer>(), breakableFadeDuration));


                    go.tag = "UnCollidable";
                    direction = go.transform.position - this.transform.position;
                    direction *= -1;
                    if (go.GetComponent<Rigidbody>() != null)
                    {
                        go.GetComponent<Rigidbody>().useGravity = true;
                        go.GetComponent<Rigidbody>().isKinematic = false;
                        go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);

                    }
                }
                if (!(go.CompareTag("Enemy") || go.CompareTag("Object")|| go.CompareTag("Debris"))) continue;
                direction = go.transform.position - this.transform.position;
                direction *= -1;
                if (go.GetComponent<Rigidbody>() != null)
                    go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
            }
        }

        private void Action(float forceStrength, Vector3 targetDirection)
        {
			anim.SetBool("Pull",true);
            audioManager.Play(PullSound);
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
            GameObject player = PlayerManager.instance;//GameObject.FindWithTag("Player");
            List<GameObject> breakables=new List<GameObject>();
            Vector3 sum=Vector3.zero; 
            int breakableCount=0;
            foreach (Collider col in colliders)
            {
                GameObject go = col.gameObject;
                if (!(go.CompareTag("Enemy") || go.CompareTag("Object") || go.CompareTag("Breakable")|| go.CompareTag("Debris")|| go.CompareTag("Bomb")|| go.CompareTag("LaunchPad")|| go.CompareTag("HealthPayload"))) continue;
                
                Vector3 direction = go.transform.position - this.transform.position;
                if (forceStrength >= ForceMid&&go.CompareTag("Bomb")&&go.transform.parent!=null)
                {
                    StartCoroutine(go.GetComponent<Bomb>().ActivateBomb());
                    col.gameObject.GetComponent<Rigidbody>().isKinematic=false;
                    col.gameObject.transform.parent=null;
                    forceStrength=1;

                }
                if (forceStrength >= ForceMid&&go.CompareTag("HealthPayload")&&go.transform.parent!=null)
                {
                    col.gameObject.GetComponent<Rigidbody>().isKinematic=false;
                    col.gameObject.transform.parent.parent=null;
                    forceStrength=1;

                }
                if (forceStrength >= ForceMid&&go.CompareTag("LaunchPad")&&Vector3.Distance(this.transform.position,go.transform.position)<5)
                {

                    go.GetComponent<MeshRenderer>().material.SetFloat("Vector1_858832AD",0);
                    LaunchPad lp=go.GetComponent<LaunchPad>();
                    if(lp.on==true){
                        player.GetComponent<Rigidbody>().AddForce(targetDirection *-1*launchForce*forceStrength, ForceMode.VelocityChange);//ForceMode.Impulse);
                    }
                    
                    continue;
                }
                if (forceStrength >= forceHigh&&go.CompareTag("Breakable"))
                {
                    //direction = go.transform.position - this.transform.position;
                    if (Math.Abs(Vector3.Angle(targetDirection, direction)) > angle/2 + angle / 4) continue;
                    sum += go.transform.position;
                    breakableCount+=1;
                    //breakablesPositions.Add(go.transform.position);
                    //disable collisions bettween player and debris
                    Physics.IgnoreCollision(player.GetComponent<Collider>(),col);
                    //Destroy(go, breakableFadeDuration+5);
                    go.layer=9;
                    //fade object
                    //Material[] intMaterials = new Material[go.GetComponent<MeshRenderer>().materials.Length];
                    //for(int i=0; i<go.GetComponent<MeshRenderer>().materials.Length;i++){
                    //    intMaterials[i] = fade_material;
                    // }
                    //go.GetComponent<MeshRenderer>().materials = intMaterials;

                    //Color  transparent = go.GetComponent<MeshRenderer>().material.color;
                    //transparent.a=0;
                    //go.GetComponent<MeshRenderer>().material=fade_material;
                    //StartCoroutine(Lerp_MeshRenderer_Color(go.GetComponent<MeshRenderer>(), breakableFadeDuration));

                    go.tag = "UnCollidable";

                    if (go.GetComponent<Rigidbody>() != null)
                    {
                        go.GetComponent<Rigidbody>().useGravity = true;
                        go.GetComponent<Rigidbody>().isKinematic = false;
                        go.GetComponent<Rigidbody>().AddForce(direction.normalized *-1 * forceStrength, ForceMode.Impulse);
                        vibrate_on=true;
                        breakables.Add(go);
                    }
                    
                    continue;
                }
                //logika pulla
                if (Math.Abs(Vector3.Angle(targetDirection, direction)) > angle / 2) continue;
                if (go.GetComponent<Rigidbody>() != null){
                    //nuliranje prijasnjih sila
                    go.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    go.GetComponent<Rigidbody>().AddForce(direction.normalized *-1 * forceStrength, ForceMode.Impulse);
                }
            }
            if(vibrate_on){
                if(breakableCount!=0){
                    /*
                    Vector3 sum=Vector3.zero;   
                    foreach( Vector3 vec in breakablesPositions ){
                        sum += vec;
                    } 
                    */
                    StartCoroutine(Lerp_MeshRenderer_Color_Multiple(breakables, breakableFadeDuration));
                    breakables[0].transform.parent.parent.GetComponent<ToggleFractureVisibility>().broken=true;
                    Vector3 centroid= sum/breakableCount;
                    //print ("centroid"+centroid);
                    particleSystem.transform.position=centroid;
                    particleSystem.transform.LookAt(this.transform.position);
                    particleSystem.Play();
                    audioManager.Play(BreakSound);
                    cinemachineShake.shake_on=true;
                    cinemachineShake.start=true;
                }  
                
               
            }
        }
        
        // Update is called once per frame

        private float[] GetForce(float time)
        {
            if (time > forceChargeTimeHigh) return new[] { 3, forceHigh };
            if (time > forceChargeTimeMid) return new[] { 2, ForceMid };
            return new[] { 1, ForceMin };
        }
        void Update()
        {
            if (Input.GetKey(keyCodePull) || Input.GetKey(joystickPullButton))
            {
                if (chargeMeter == null) Debug.LogError("Error u Push, charge metar nije pronaden");
                chargeMeter.enabled = true;
                chargeMeter.SetValue(forceChargeTimerPull / forceChargeTimeHigh);

                forceChargeTimerPull += Time.deltaTime;
                //print("forceChargePull = " + forceChargeTimerPull);
                this.GetComponent<Push>().enabled = false;
            }

             if (GameManager.instance.joystick==true){
                Vector3 joystickAimVector=GameManager.instance.GetVecFromControler();
            }else{
                Vector3 mouseAimVector=GameManager.instance.GetVecToMouse(); // daodaj i za kontroler s INput.horizontal/vertical aim, ako je veci pokreni za controller
            }
            if (Input.GetKeyUp(keyCodePull) || Input.GetKeyUp(joystickPullButton))
            {
                chargeMeter.ResetMask();
                chargeMeter.enabled = false;

                //usteda 1 poziva funkcije
                float[]res=GetForce(forceChargeTimerPull);
                int chargesUsed=(int)res[0];    
                float force = res[1];
                //int chargesUsed = (int)GetForce(forceChargeTimerPush)[0];
                //float force = GetForce(forceChargeTimerPush)[1];

                if (!icon.CheckIfUsable(chargesUsed))
                {
                    //print("Ability not usable");

                }
                else
                {
                    icon.UseCharges(chargesUsed);
                    Action(force, GameManager.instance.pointerDirection);
                }
                forceChargeTimerPull = 0f;
                this.GetComponent<Push>().enabled = true;
            }
        }
        private IEnumerator Lerp_MeshRenderer_Color(MeshRenderer target_MeshRender, float lerpDuration)
        {   
            //MeshRenderer target_MeshRender=go.GetComponent<MeshRenderer>();

            yield return new WaitForSeconds(5);
            Color startLerp=target_MeshRender.material.color;
            Color targetLerp=startLerp;
            targetLerp.a=0;
            float lerpStart_Time = Time.time;
            float lerpProgress;
            bool lerping = true;
            
            while (lerping)
            {
                yield return new WaitForEndOfFrame();
                lerpProgress = Time.time - lerpStart_Time;
                if (target_MeshRender != null)
                {
                    //for(int i=0; i<target_MeshRender.materials.Length;i++){
                    //    target_MeshRender.materials[i].color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
                    // }
                    target_MeshRender.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
                    
                }
                else
                {
                    lerping = false;
                }
                
                
                if (lerpProgress >= lerpDuration)
                {
                    lerping = false;
                }
            }
            
            yield break;
        }
        private IEnumerator Lerp_MeshRenderer_Color_Multiple(List<GameObject> breakables, float lerpDuration)
        {   
            //MeshRenderer target_MeshRender=go.GetComponent<MeshRenderer>();
            List<MeshRenderer> renderers=new List<MeshRenderer>(breakables.Count);
            foreach (GameObject go in breakables)
            {
            
                go.GetComponent<MeshRenderer>().material=fade_material;
                renderers.Add(go.GetComponent<MeshRenderer>());
                
            } 

            yield return new WaitForSeconds(5);
            //print(renderers.Count);
            //Color startLerp=renderers[0].material.color;
            Color startLerp=fade_material.color;
            Color targetLerp=startLerp;
            targetLerp.a=0;
            float lerpStart_Time = Time.time;
            float lerpProgress;
            bool lerping = true;
            
            while (lerping)
            {
                yield return new WaitForEndOfFrame();
                lerpProgress = Time.time - lerpStart_Time;
                /* if (target_MeshRender != null)
                {
                    //for(int i=0; i<target_MeshRender.materials.Length;i++){
                    //    target_MeshRender.materials[i].color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
                    // }
                    target_MeshRender.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
                }
                else
                {
                    lerping = false;
                } */
                foreach (MeshRenderer renderer in renderers)
                {
                    if (renderer != null){
                        renderer.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
                    }
                }
                
                if (lerpProgress >= lerpDuration)
                {
                    lerping = false;
                }
            }
            foreach (GameObject go in breakables)
            {
                Destroy(go);
            } 
            yield break;
        }
        IEnumerator vibrate(){
            GamePad.SetVibration(0, left_vibration_rumble_strength, right_vibration_noise_strength);
            yield return new WaitForSeconds(vibration_duration);
            GamePad.SetVibration(0,0,0);
            vibrate_on=false;
        }
        void FixedUpdate() {
            if(GameManager.instance.joystick==true&& vibrate_on==true){
                StartCoroutine(vibrate());
            }
        }
    }
}

