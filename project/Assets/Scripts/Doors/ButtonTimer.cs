using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Assets.Scripts.Doors
{
    class ButtonTimer : MonoBehaviour
    {
        public int time;
        public float targetPitch=1.5f;
        public List<Door> doors;
        private Door firstDoor;
        public KeyCode keyCode = KeyCode.E;
        public KeyCode joystickActivateButton = KeyCode.JoystickButton1;
        private Material unpressedMaterial;
        public Material pressedMaterial;
        public GameObject textHint;
        public bool textHintEnabled=false;
        //public string keyboardButtonMessage="Press <color=\"red\">E</color> to activate the button.";
        //public string joystickButtonMessage="Press <color=\"red\">B</color> to activate the button.";
        private string keyboardButtonMessage="[E]";
        private string joystickButtonMessage="<sprite name=XboxOne_B>";
        public AudioClip pressingSound;
        public AudioClip unpressingSound;
        public AudioClip timer_loop_sound;
        public AudioClip timer_end_sound;
	    private AudioSource speaker;
        private Color startColor;

        void Start() {
            //find buttonHint
            if (textHintEnabled==true){
                GameObject mainCamera= GameObject.FindWithTag("MainCamera");
                RectTransform[] rectTransforms=mainCamera.GetComponentsInChildren<RectTransform>(includeInactive:true);
                foreach (RectTransform item in rectTransforms){
                    if (item.tag.Equals("ButtonHint")){
                        textHint=item.gameObject;
                    }
                }
            }
            if(time<3){
                time=3;
            }
            if(doors.Count>0){
                firstDoor=doors[0];
            }
            speaker = GetComponent<AudioSource>();
            unpressedMaterial=gameObject.GetComponent<Renderer>().material;
            startColor=unpressedMaterial.color;
            /*if (pressedMaterial==null){
                pressedMaterial=new Material(gameObject.GetComponent<Renderer>().material);
                pressedMaterial.color=Color.green;            
            } */
            }
        void OnTriggerStay(Collider other)
        {
            if (textHintEnabled==true){
                if (textHint!=null&&other.gameObject.CompareTag("Player") && firstDoor.disableChange == false && !firstDoor.open){
                textHint.SetActive(true);
                }
            }            
            

            if (other.gameObject.CompareTag("Player") && firstDoor.disableChange == false && (Input.GetKeyDown(keyCode) || Input.GetKey(joystickActivateButton)))
            {
                if (!firstDoor.open)
                {
                    //firstDoor.Open();
                    foreach(Door door in doors){
                        door.Open();
                    }
                     if(!speaker.isPlaying ){
                        speaker.clip=pressingSound;
                        speaker.Play();
                        StartCoroutine(PlaySoundNTimes(time));
                    }
                    //gameObject.GetComponent<Renderer> ().material = pressedMaterial;
                    gameObject.GetComponent<Renderer> ().material.color=Color.green;
                    
                    if(textHint!=null&&textHintEnabled==true){
                        textHint.SetActive(false);
                    }
                    StartCoroutine(AutoClose());
                }else{
                    //textHint.SetActive(true);
                }
            }
        }

        private IEnumerator AutoClose()
        {
            if(time<3){
                time=3;
            }
            //timerloop sound is 1.872 seconds long
            yield return new WaitForSecondsRealtime(time*timer_loop_sound.length);
            foreach(Door door in doors){
                door.Close();
            }
            //firstDoor.Close();
            if(!speaker.isPlaying ){
                speaker.clip=unpressingSound;
                speaker.Play();
            }
            //gameObject.GetComponent<Renderer> ().material = unpressedMaterial;
            gameObject.GetComponent<Renderer> ().material.color = startColor;
            
            if(textHint!=null&&textHintEnabled==true){
                if(GameManager.instance.joystick==true){
                    TextMeshProUGUI valueField = textHint.GetComponentInChildren<TextMeshProUGUI>();
                    valueField.text = joystickButtonMessage;
                }else{
                    TextMeshProUGUI valueField = textHint.GetComponentInChildren<TextMeshProUGUI>();
                    valueField.text = keyboardButtonMessage;
                }
                //textHint.SetActive(true);
            }
            
        }
        void OnTriggerEnter(Collider other){
            if (textHintEnabled==true&&textHint!=null&&!firstDoor.open && other.gameObject.CompareTag("Player")){
                if(GameManager.instance.joystick==true){
                        TextMeshProUGUI valueField = textHint.GetComponentInChildren<TextMeshProUGUI>();
                        valueField.text = joystickButtonMessage;
                }else{
                        TextMeshProUGUI valueField = textHint.GetComponentInChildren<TextMeshProUGUI>();
                        valueField.text = keyboardButtonMessage;
                    }
                textHint.SetActive(true);
            }
        }

        void OnTriggerExit(Collider other){
            if (textHintEnabled==true&&textHint!=null&&other.tag == "Player"){
                textHint.SetActive(false);
            }
        }
        IEnumerator  PlaySoundNTimes(float N){
            if(N<3){
                N=3;
            }
            speaker.loop = true;
            speaker.clip=timer_loop_sound;
            speaker.pitch=1;
            speaker.Play();
            yield return new WaitForSecondsRealtime((N-2)*speaker.clip.length);
            PitchTransition(targetPitch,2*speaker.clip.length);
            yield return new WaitForSecondsRealtime(2*speaker.clip.length);
            speaker.Stop();
            speaker.loop = false;
            speaker.pitch=1;
            speaker.PlayOneShot(timer_end_sound);
        }
            // change pitch to target frequency over duration seconds
        public void PitchTransition(float target, float duration)
        {
            // set targetPitch variable to allow access to desired pitch to any object modifying it while coroutine is active
            // targetPitch = target;

            // stop any pitch transitions before setting a new one
           /*  if (pitchCoroutine != null)
            {
                StopCoroutine(pitchCoroutine);
            } */

            // if duration is a very small number just set pitch directly
            // avoids potential division by 0
            /* if (duration <= Mathf.Epsilon && duration >= -Mathf.Epsilon)
            {
                SetPitch(target);
            }
            else
            { */
                // assign transition to variable, then start the coroutine
                StartCoroutine(PitchTransitionCoroutine(target, duration));
            //}
        }


        // change volume to target frequency over duration seconds
        /* public void VolumeTransition(float target, float duration)
        {
            // set targetPitch variable to allow access to desired pitch to any object modifying it while coroutine is active
            targetVolume = target;

            // stop any volume transitions before starting a new one
            if (volumeCoroutine != null)
            {
                StopCoroutine(volumeCoroutine);
            }

            // if duration is a very small number just set volume directly
            // avoids potential division by 0
            if (duration <= Mathf.Epsilon && duration > 0)
            {
                SetVolume(target);
            }
            else
            {
                // assign transition to variable, then start the coroutine
                volumeCoroutine = VolumeTransitionCoroutine(target, duration);
                StartCoroutine(volumeCoroutine);
            }
        }
*/

        // Coroutine for transitioning the pitch, run by calling PitchTransition
        IEnumerator PitchTransitionCoroutine(float target, float duration)
        {
            float from = speaker.pitch;
            float invDuration = 1.0f / duration;

            // the "counter" variable to track position within Lerp
            float progress = Time.unscaledDeltaTime * invDuration;

            while (Mathf.Abs(speaker.pitch - target) > 0.0f)
            {
                speaker.pitch = Mathf.Lerp(from, target, progress);
                progress += Time.unscaledDeltaTime * invDuration;
                yield return new WaitForSecondsRealtime(0.01f);
                //yield return null;
            }
        } 


        // Coroutine for transitioning volume, run by calling VolumeTransition
        /* IEnumerator VolumeTransitionCoroutine(float target, float duration)
        {
            float from = audio.volume;
            float invDuration = 1.0f / duration;

            // the "counter" variable to track position within Lerp
            float progress = Time.unscaledDeltaTime * invDuration;

            while (Mathf.Abs(audio.volume - target) > 0.0f)
            {
                audio.volume = Mathf.Lerp(from, target, progress);
                progress += Time.unscaledDeltaTime * invDuration;
                yield return null;
            }
        }        */
    }
}
    

 
