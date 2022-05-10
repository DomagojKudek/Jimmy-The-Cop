using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{

    public class Friction : MonoBehaviour
    {
        public bool on_off = false;
        public KeyCode keyCode = KeyCode.R;
		public KeyCode joystickFrictionButton = KeyCode.JoystickButton3;

        public float maxFriction = 1;
        public float minFriction = 0;
        public AbilityIcon icon;

        private bool stopChange = false;

        void ChangeFriction(float staticFric, float dynamicFric, Color color)
        {
            Ray ray = new Ray(this.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 2))
            {
                print(hit.collider.gameObject.name);
                GameObject hitGo = hit.collider.gameObject;
                if (hitGo.CompareTag("Platform"))
                {
                    PhysicMaterial material = hitGo.GetComponent<Collider>().material;
                    if(!stopChange){
                        stopChange = true;
                        StartCoroutine(ReturnToInitial(material, hitGo, hitGo.GetComponent<MeshRenderer>().material.color));
                    }
                    hitGo.GetComponent<MeshRenderer>().material.color = color;
                    print(material.name);

                    material.staticFriction = staticFric;
                    material.dynamicFriction = dynamicFric;
                }
            }
        }

        private IEnumerator ReturnToInitial(PhysicMaterial material, GameObject go, Color color)
        {
            float sf = material.staticFriction;
            float df = material.dynamicFriction;
            yield return new WaitForSecondsRealtime(5);
            material.staticFriction = sf;
            material.dynamicFriction = df;          
            go.GetComponent<MeshRenderer>().material.color = color;
            stopChange = false;
        }

        void Update()
        {
            if (icon.CheckIfUsable(1) && Input.GetKeyDown(keyCode)||Input.GetKeyDown(joystickFrictionButton))
            {
                on_off = !on_off;
                icon.UseCharges(1);
                if (on_off)
                {
                    ChangeFriction(maxFriction, maxFriction, Color.black);
                }
                else
                {
                    ChangeFriction(minFriction, minFriction, Color.white);
                }
            }
        }   
    }
}
