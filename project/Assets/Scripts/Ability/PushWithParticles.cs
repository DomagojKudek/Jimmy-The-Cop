using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using XInputDotNetPure; // Required in C#
using static UnityEngine.ParticleSystem;

namespace Assets.Scripts
{
    public class Pushv2 : MonoBehaviour
    {
        public float radius = 5f;
        public float ForceMin = 5f;
        public float ForceMid = 10f;
        public float forceHigh = 20f;

        public float forceChargeTimeMid = 0.6f;
        public float forceChargeTimeHigh = 1.2f;

        public float angle = 30f;

        //public float forceStrength = 0f;
        public KeyCode keyCodePush = KeyCode.Alpha2;
        public KeyCode joystickPushButton = KeyCode.JoystickButton4;

        private float breakableFadeDuration = 5f;
        public Material fade_material;

        [HideInInspector]
        public float forceChargeTimerPush = 0f;

        public AbilityIcon icon;

        private AudioManager audioManager;

        //public GameObject fracturedVersion;

        [HideInInspector]
        public Vector3 pointerDirection;

        //private Slider chargeMeter;

        private ChargeMeter chargeMeter;
        private bool vibrate_on=false;
        public float vibration_duration=0.7f;
        public float left_vibration_rumble_strength=1.0f;
        public float right_vibration_noise_strength=0.5f;
        public GameObject cinemachineShakeObj;
        private CameraShakeCinemamachine cinemachineShake;

        public string pushAudio = "Push";
        public string BreakSound = "Break";

        private ParticleSystem particleSystem;

        private ParticleSystem particleBurst;

        public Material trailMaterial;

        //public float startDistanceFromPlayer = 0; //novo
        //private Vector3 startPushPosition; //novo

        private void Start(){
            cinemachineShake = cinemachineShakeObj.GetComponent<CameraShakeCinemamachine>();
            //Canvas canvas = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetComponent<Canvas>();

            //chargeMeter = canvas.transform.GetChild(0).GetComponent<Slider>();
            chargeMeter = this.GetComponent<ChargeMeter>();

            float scaledMid = forceChargeTimeMid / forceChargeTimeHigh;
            float scaledMax = 1f;
            chargeMeter.Initialize(scaledMid, scaledMax);
            audioManager = AudioManager.instance;
            particleSystem = this.transform.GetChild(2).GetComponent<ParticleSystem>();
            particleBurst = this.transform.GetChild(3).GetComponent<ParticleSystem>();

            //InitializeIfNeeded();

        }
        private void Action(float forceStrength)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius); 
            GameObject player=GameObject.FindWithTag("Player");
            foreach (Collider col in colliders)
            {
                GameObject go = col.gameObject;
                
                Vector3 direction;

                if (go.tag.Equals("Breakable") && forceStrength >= forceHigh)
                {
                    //disable collisions bettween player and debris
                    Physics.IgnoreCollision(player.GetComponent<Collider>(), go.GetComponent<Collider>());
                    Destroy(go, 5);
                    go.tag = "UnCollidable";
                    direction = go.transform.position - this.transform.position;
                    if (go.GetComponent<Rigidbody>() != null)
                    {
                        go.GetComponent<Rigidbody>().useGravity = true;
                        go.GetComponent<Rigidbody>().isKinematic = false;
                        go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                    }
                    
                }
                if (!(go.tag.Equals("Enemy") || go.tag.Equals("Object")|| go.tag.Equals("Debris"))) continue;
                direction = go.transform.position - this.transform.position;
                if (go.GetComponent<Rigidbody>() != null)
                    go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
            }
        }

        private void Action(float forceStrength, Vector3 targetDirection)
        {
            //startPushPosition = FindStartPushPosition(targetDirection);//novo

            audioManager.Play(pushAudio);

            Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
            GameObject player = PlayerManager.instance;//GameObject.FindWithTag("Player");
            foreach (Collider col in colliders)
            {
                GameObject go = col.gameObject;

                if (!(go.tag.Equals("Enemy") || go.tag.Equals("Object") || go.tag.Equals("Breakable")|| go.tag.Equals("Debris"))) continue;
                Vector3 direction = go.transform.position - this.transform.position;
                if (go.tag.Equals("Breakable") && forceStrength >= forceHigh)
                {
                    if (Math.Abs(Vector3.Angle(targetDirection, direction)) > angle/2 + angle/4) continue;
                    //disable collisions bettween player and debris
                    audioManager.Play(BreakSound);

                    Physics.IgnoreCollision(player.GetComponent<Collider>(), go.GetComponent<Collider>());
                   
                    Destroy(go, breakableFadeDuration+5);
                    //fade object
                    Material[] intMaterials = new Material[go.GetComponent<MeshRenderer>().materials.Length];
                    for(int i=0; i<go.GetComponent<MeshRenderer>().materials.Length;i++){
                        intMaterials[i] = fade_material;
                     }
                    go.GetComponent<MeshRenderer>().materials = intMaterials;

                    Color  transparent = go.GetComponent<MeshRenderer>().material.color;
                    transparent.a=0;
                    StartCoroutine(Lerp_MeshRenderer_Color(go.GetComponent<MeshRenderer>(), breakableFadeDuration, go.GetComponent<MeshRenderer>().material.color, transparent));

                    

                    
                    go.tag = "UnCollidable";
                    
                    if (go.GetComponent<Rigidbody>() != null)
                    {
                        go.GetComponent<Rigidbody>().useGravity = true;
                        go.GetComponent<Rigidbody>().isKinematic = false;
                        go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                        vibrate_on=true;
                    }
                    
                }

                if (Math.Abs(Vector3.Angle(targetDirection, direction)) > angle / 2) continue;
                if (go.GetComponent<Rigidbody>() != null)
                    go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
            }
            if(vibrate_on){        
                cinemachineShake.shake_on=true;
                cinemachineShake.start=true;
            }
        }

        private Vector3 GetVecToMouse()
        {
            Vector3 VScreen = new Vector3();
            Vector3 VWold = new Vector3();

            VScreen.x = Input.mousePosition.x;
            VScreen.y = Input.mousePosition.y;
            VScreen.z = Camera.main.transform.position.z * -1;//Nemam pojma zasto
            VWold = Camera.main.ScreenToWorldPoint(VScreen);
            VWold.z = 0;       
            pointerDirection = VWold - this.transform.position;
            return pointerDirection;
        }

        private Vector3 GetVecFromControler()
        {
            pointerDirection =new Vector3(Input.GetAxis("HorizontalAim")*radius, Input.GetAxis("VerticalAim")*radius, 0);
            return pointerDirection;
        }

        
        void OnDrawGizmos()
        {
            //Gizmos.DrawWireSphere(this.transform.position, radius);

            //ako koristimo directional

            //pointerDirection = new Vector3(-23,1,12) - this.transform.position;
            if (GameManager.instance!=null && GameManager.instance.joystick==true){
                Vector3 joystickAimVector=GetVecFromControler();
            }else{
                Vector3 mouseAimVector=GetVecToMouse(); // daodaj i za kontroler s INput.horizontal/vertical aim, ako je veci pokreni za controller
            }
            Gizmos.color = Color.red;

            //startPushPosition = FindStartPushPosition(pointerDirection);
            Vector3 startPushPosition = this.transform.position;
            //print ("Start push position = " + startPushPosition);
            Gizmos.DrawRay(startPushPosition, pointerDirection);
            Gizmos.color = Color.black;
            Gizmos.DrawRay(startPushPosition, Quaternion.Euler(0,0, -angle/2) * pointerDirection);
            Gizmos.DrawRay(startPushPosition, Quaternion.Euler(0, 0, angle/2) * pointerDirection);
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
        
        
        // Update is called once per frame

        private float[] GetForce(float time)
        {
            if (time > forceChargeTimeHigh) return new[]{ 3, forceHigh};
            if (time > forceChargeTimeMid) return new[] { 2, ForceMid};
            return new[] { 1, ForceMin};
        }
        void Update()
        {            
            if (Input.GetKey(keyCodePush) || Input.GetKey(joystickPushButton))
            {                                
                if (chargeMeter == null) Debug.LogError("Error u Push, charge metar nije pronaden");
                //PARTICLES
                if(!particleSystem.isPlaying){
                    var col = particleSystem.colorOverLifetime;
                    //col.color = ChargeMeter.colorGradient();
                    //trailMaterial.color = Color.blue;
                    //var trailcol = particleSystem.trails.colorOverTrail;
                    //trailcol = ChargeMeter.colorGradient();
                    particleSystem.Play();
                }
                //var trailCol = particleSystem.trails.colorOverTrail.color;
                //trailCol = Color.green;
                //chargeMeter.enabled = true;

                //var trailColor = particleSystem.trails

                //particleSystem.startColor = chargeMeter.SetValue(forceChargeTimerPush / forceChargeTimeHigh);


                forceChargeTimerPush += Time.deltaTime;
                //print("forceChargePush = " + forceChargeTimerPush);
                this.GetComponent<Pull>().enabled = false;
            }
            if (GameManager.instance.joystick==true){
                Vector3 joystickAimVector=GetVecFromControler();
            }else{
                Vector3 mouseAimVector=GetVecToMouse(); // daodaj i za kontroler s INput.horizontal/vertical aim, ako je veci pokreni za controller
            }
            if (Input.GetKeyUp(keyCodePush) || Input.GetKeyUp(joystickPushButton))
            {
                //PARTICLES
                //int numParticlesAlive = particleSystem.GetParticles(m_Particles);
                particleSystem.Stop();
                /*
                int numParticlesAlive = particleSystem.GetParticles(particles);
                print(numParticlesAlive);
                for(int i = 0; i < numParticlesAlive; i++){
                    particles[i].startColor = Color.blue;
                    print(particles[i].position);
                    print(new Vector3(particles[i].velocity.x * 5, particles[i].velocity.y, particles[i].velocity.z));
                    particles[i].velocity = new Vector3(particles[i].velocity.x * 5, particles[i].velocity.y, particles[i].velocity.z);
                }
                */
                
                //foreach(var p in )
                
                //particleSystem.Clear();
                //particleBurst.
                //particleBurst.Play();
                //particleSystem.gameObjec
                chargeMeter.enabled = false;
                //TODO ugasi sprite mask

                int chargesUsed = (int)GetForce(forceChargeTimerPush)[0];
                float force = GetForce(forceChargeTimerPush)[1];

                if(!icon.CheckIfUsable(chargesUsed)){
                    print("Ability not usable");
                    
                }
                else{
                    icon.UseCharges(chargesUsed);
                    Action(force, pointerDirection);
                }
                forceChargeTimerPush = 0f;
                this.GetComponent<Pull>().enabled = true;
            }
        }
        private IEnumerator Lerp_MeshRenderer_Color(MeshRenderer target_MeshRender, float lerpDuration, Color startLerp, Color targetLerp)
        {   
            yield return new WaitForSeconds(5);
            float lerpStart_Time = Time.time;
            float lerpProgress;
            bool lerping = true;
            
            while (lerping)
            {
                yield return new WaitForEndOfFrame();
                lerpProgress = Time.time - lerpStart_Time;
                if (target_MeshRender != null)
                {
                    for(int i=0; i<target_MeshRender.materials.Length;i++){
                        target_MeshRender.materials[i].color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
                     }
                    
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
        /*
        void InitializeIfNeeded()
        {
            if (particleSystem == null)
                particleSystem = GetComponent<ParticleSystem>();

            if (particles == null || particles.Length < particleSystem.main.maxParticles)
                particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        }
        */
    }
}

