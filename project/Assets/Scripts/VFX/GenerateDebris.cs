using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDebris : MonoBehaviour {
    public GameObject debrisObject;
    public GameObject debrisObjects;
    public int numberOfDebris=20; // number of objects per platform
    // Use this for initialization
    void Start () {
        generateDebris();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void generateDebris()
    {
        GameObject player=GameObject.FindWithTag("Player");
        foreach (Transform platformTransform in this.transform)
        {
            GameObject platform = platformTransform.gameObject;
            if (platform.tag.Equals("Platform")) {
                int debrisId = 0;
                //ramps dont get any debris
                if(platformTransform.rotation.z!=0){
                    continue;
                }
                Collider platform_Collider = platform.GetComponent<Collider>();

                //Fetch the size of the Collider volume
                Vector3 platform_Size = platform_Collider.bounds.size;
                float platform_xSize = platform_Size.x;
                float platform_ySize = platform_Size.y;
                float platform_zSize = platform_Size.z;

                GameObject debrisParent = new GameObject(name: "Debris");

                foreach (Transform debrisTransform in debrisObjects.transform)
                {
                    GameObject debris = debrisTransform.gameObject;
                    if (debris.tag.Equals("Debris"))
                    {
                        if (debrisId > numberOfDebris)
                        {
                            break;
                        }
                        // generate random x position
                        float posx = Random.Range(platform.transform.position.x - platform_xSize / 2, platform.transform.position.x + platform_xSize / 2);
                        // generate random z position
                        float posz = Random.Range(platform.transform.position.z - platform_zSize / 2, platform.transform.position.z + platform_zSize / 2);

                        debris.layer = LayerMask.NameToLayer("UnCollidable");
                        debris.tag = "Debris";
                        GameObject debrisInScene=Instantiate(debris, parent: debrisParent.transform);
                        //disable collisions bettween player and debris
                        Physics.IgnoreCollision(player.GetComponent<Collider>(), debrisInScene.GetComponent<Collider>());

                        debrisInScene.AddComponent<ConstantForce>();
                        debrisInScene.GetComponent<Rigidbody>().isKinematic = false;
                        debrisInScene.GetComponent<Rigidbody>().useGravity = true;
                        //debrisInScene.GetComponent<Rigidbody>().mass = 0.1f;
                        debrisInScene.GetComponent<Rigidbody>().drag = 1;
                        debrisInScene.transform.localScale -= new Vector3(0.7f, 0.7f, 0.7f);
                        debrisInScene.transform.position = new Vector3(posx, platform.transform.position.y + platform_ySize, posz);

                        //print(debrisInScene.name + " Pos : " + debrisInScene.transform.position);
                        debrisId += 1;
                    }
                }          
            }
        }

    }
}
