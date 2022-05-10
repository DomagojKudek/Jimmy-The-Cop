using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Doors
{
    //TODO pazi koji objekt moze triggerat vrata
    public class PresurePlateTimer : MonoBehaviour
    {
        List<Collider> collidedObjects = new List<Collider>();
        public int time;
        public float targetPitch=1.5f;
        public List<Door> doors;
        private Door firstDoor;
        //tags that interact with plate
        private HashSet<string> tags = new HashSet<string>() { "Player", "Enemy", "Object" };
        private Material unpressedMaterial;
        public Material pressedMaterial;
        public GameObject textHint;
        public AudioClip pressingSound;
        public AudioClip unpressingSound;
	    public AudioClip timer_loop_sound;
        public AudioClip timer_end_sound;
	    private AudioSource speaker;
        private Color startColor;
        void Start() {
            if(time<3){
                time=3;
            }
            if(doors.Count>0){
                firstDoor=doors[0];
            }
            speaker = GetComponent<AudioSource>();
            unpressedMaterial=gameObject.GetComponent<Renderer>().material;
            startColor=unpressedMaterial.color;
            /* if (pressedMaterial==null){
                pressedMaterial=new Material(gameObject.GetComponent<Renderer>().material);
                pressedMaterial.color=Color.green;            
            } */
        }
        void OnTriggerEnter(Collider other)
        {
            if(textHint!=null){
                if (!firstDoor.open && other.tag == "Player"&& textHint!=null){
                textHint.SetActive(true);
                }
            }
            
            collidedObjects.Add(other);
            if (collidedObjects.Count == 1 && tags.Contains(other.tag))
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
                StartCoroutine(TimedClose());
                //gameObject.GetComponent<Renderer> ().material = pressedMaterial;
                gameObject.GetComponent<Renderer> ().material.color=Color.green;

            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player"&& textHint!=null){
                textHint.SetActive(false);
            }
            /*if (collidedObjects.Count == 1 && tags.Contains(other.tag))
            {
                
                //StartCoroutine(TimedClose());
            }*/
            collidedObjects.Remove(other);
        }

        private IEnumerator TimedClose()
        {
            //timerloop sound is 1.872 seconds long
            if(time<3){
                time=3;
            }
            yield return new WaitForSecondsRealtime(time*timer_loop_sound.length);
            //firstDoor.Close();
            foreach(Door door in doors){
                door.Close();
            }
            if(!speaker.isPlaying ){
                speaker.clip=unpressingSound;
                speaker.Play();
                
            }
            //gameObject.GetComponent<Renderer> ().material = unpressedMaterial;
            gameObject.GetComponent<Renderer> ().material.color = startColor;

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
    }
}
