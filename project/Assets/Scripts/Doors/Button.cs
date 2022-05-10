using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
namespace Assets.Scripts.Doors
{
    class Button : MonoBehaviour
    {
        public List<Door> doors;
        private Door firstDoor;
        public KeyCode keyCode = KeyCode.E;
        public KeyCode joystickActivateButton = KeyCode.JoystickButton1;
        private Material unpressedMaterial;
        private Material pressedMaterial;
        public GameObject textHint;
        public bool textHintEnabled=false;
        //public string keyboardButtonMessage="Press <color=\"red\">E</color> to activate the button.";
        //public string joystickButtonMessage="Press <color=\"red\">B</color> to activate the button.";
        private string keyboardButtonMessage="[E]";
        private string joystickButtonMessage="<sprite name=XboxOne_B>";
        public AudioClip pressingSound;
        public AudioClip unpressingSound;
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
        void OnTriggerStay(Collider other){
            if (other.gameObject.CompareTag("Player") && firstDoor.disableChange == false && (Input.GetKeyDown(keyCode) || Input.GetKey(joystickActivateButton)))
            {
                if (firstDoor.open)
                {
                    //door.Close();
                    foreach(Door door in doors){
                        door.Close();
                    }
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
                        textHint.SetActive(true);
                    }
                    
                }
                else
                {
                    //door.Open();
                    foreach(Door door in doors){
                        door.Open();
                    }
                    if(!speaker.isPlaying ){
                        speaker.clip=pressingSound;
                        speaker.Play();
                    }
                    //gameObject.GetComponent<Renderer> ().material = pressedMaterial;
                    gameObject.GetComponent<Renderer> ().material.color=Color.green;
                    
                    if(textHint!=null&&textHintEnabled==true){
                        textHint.SetActive(false);
                    }
                }
            }
        }
        void OnTriggerEnter(Collider other){
            if (textHintEnabled==true&& textHint!=null&&!firstDoor.open && other.gameObject.CompareTag("Player")){
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
            if (textHintEnabled==true&& textHint!=null&&other.tag == "Player"){
                textHint.SetActive(false);
            }
        }        
    }
}
