using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Pickups
{
    class PickupRefreshCD : Pickup
    {
        private Canvas canvas;
        public AudioClip refreshCooldownSound;
	    private AudioSource speaker;

        private void Start(){
            canvas = HUDManager.instance.canvas;
            speaker=GetComponent<AudioSource>();
            speaker.clip=refreshCooldownSound;
        }
        public override void OnPickup()
        {
            speaker.Play();
            this.gameObject.GetComponent<MeshRenderer>().enabled=false;
            this.gameObject.GetComponent<Collider>().enabled=false;
            foreach(Transform child in canvas.transform){
                if(child.CompareTag("AbilityIcon")){
                    Slider slider = child.GetComponent<Slider>();
                    slider.value = slider.maxValue;
                }
            }
        }
    }
}