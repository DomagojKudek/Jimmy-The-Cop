using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForceDirection
{
    Push,
    Pull
}

public class Push_Pull : MonoBehaviour
{
    public float radius = 0f;
    public ForceMode forceMode;
    //public float forceStrength = 0f;
    public float breakForceStrength = 0f;
    public KeyCode keyCodePull = KeyCode.Alpha2;
    public KeyCode keyCodePush = KeyCode.Alpha1;
	public KeyCode joystickPullButton = KeyCode.JoystickButton1;
    public KeyCode joystickPushButton = KeyCode.JoystickButton3;


    private float forceChargeTimerPush = 0f;
    private float forceChargeTimerPull = 0f;

    //public GameObject fracturedVersion;

    [HideInInspector]
    public Vector3 pointerDirection;
    [HideInInspector]
    public float angle;

    private void Action(ForceDirection forceDirection, float forceStrength)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
        GameObject player=GameObject.FindWithTag("Player");
        //float forceStrength = GetForceStrength(forceDirection);

        foreach (Collider col in colliders)
        {
            GameObject go = col.gameObject;
            Vector3 direction;

            if (go.tag.Equals("Breakable"))
            {
                //transform.GetChild(0).gameObject.SetActive(true);
                //prefabObject.GetComponentInChildren<Transform>().find("Child_name");
                //GameObject goNew=Instantiate(fracturedVersion, go.transform.position,go.transform.rotation);
                Destroy(go, 5);
				//disable collisions bettween player and debris
                Physics.IgnoreCollision(player.GetComponent<Collider>(), go.GetComponent<Collider>());
				go.tag="UnCollidable";
                direction = go.transform.position - this.transform.position;
                if (forceDirection == ForceDirection.Pull) direction *= -1;
                if (go.GetComponent<Rigidbody>() != null)
                {
                    go.GetComponent<Rigidbody>().useGravity = true;
                    go.GetComponent<Rigidbody>().isKinematic = false;
                    go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, forceMode);

                }
                //col.enabled = false;
            }
            if (!(go.tag.Equals("Enemy") || go.tag.Equals("Object"))) continue;
            direction = go.transform.position - this.transform.position;
            if (forceDirection == ForceDirection.Pull) direction *= -1;
            if (go.GetComponent<Rigidbody>() != null)
                go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, forceMode);
        }
    }

    private void Action(ForceDirection forceDirection, float forceStrength, Vector3 targetDirection, float angle)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
        //float forceStrength = GetForceStrength(forceDirection);

        foreach (Collider col in colliders)
        {
            GameObject go = col.gameObject;
            if (!(go.tag.Equals("Enemy") || go.tag.Equals("Object"))) continue;
            Vector3 direction = go.transform.position - this.transform.position;

            //Vector3.Dot(targetDirection, direction)/ targetDirection.magnitude * direction.ma
            if (Vector3.Angle(targetDirection, direction) > angle/2) continue;

            if (forceDirection == ForceDirection.Pull) direction *= -1;
            if (go.GetComponent<Rigidbody>() != null)
                go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, forceMode);
        }
    }

    //TODO
    private Vector3 GetVecToMouse()
    {   
        //Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0,0,1));

        //return Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        Vector3 VScreen = new Vector3();
        Vector3 VWold = new Vector3();

        VScreen.x = Input.mousePosition.x;
        VScreen.y = Input.mousePosition.y;
        VScreen.z = Camera.main.transform.position.z;
        VWold = Camera.main.ScreenToWorldPoint(VScreen);

        print("######");
        print("mouse " + Input.mousePosition);
        print("screeen to world" + VWold);
        VWold.z = 0;
        pointerDirection = VWold - this.transform.position;
        return VWold - this.transform.position;
    }

    private Vector3 GetVecFromControler()
    {
        return new Vector3();
    }

    /*
    void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(this.transform.position, radius);

        //ako koristimo directional

        //pointerDirection = new Vector3(-23,1,12) - this.transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.transform.position, pointerDirection);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(this.transform.position, Quaternion.Euler(0,0, -15) * pointerDirection);
        Gizmos.DrawRay(this.transform.position, Quaternion.Euler(0, 0, 15) * pointerDirection);
    }
    */
    // Update is called once per frame

    private float GetForce(float time){
        if (time > 2f) return 20;
        if (time > 1f) return 10;
        return 5;
    }
    void Update()
    {
        if (Input.GetKey(keyCodePush) || Input.GetKey(joystickPushButton))
        {
            forceChargeTimerPush += Time.deltaTime;
            print("forceChargePush = " + forceChargeTimerPush);
        }

        if (Input.GetKeyUp(keyCodePush) || Input.GetKeyUp(joystickPushButton))
        {
            float force = GetForce(forceChargeTimerPush);
            
            Action(ForceDirection.Push, force);
            print("chargeTime : " + forceChargeTimerPush);
            print("Release : " + force);
            forceChargeTimerPush = 0f;
        }
        
        if (Input.GetKey(keyCodePull) || Input.GetKey(joystickPullButton))
        {
            forceChargeTimerPull += Time.deltaTime;
            print("forceChargePull = " + forceChargeTimerPull);
        }

        if (Input.GetKeyUp(keyCodePull) || Input.GetKeyUp(joystickPullButton))
        {
            float force = GetForce(forceChargeTimerPull);

            Action(ForceDirection.Pull, force);
            print("chargeTime : " + forceChargeTimerPull);
            print("Release : " + force);
            forceChargeTimerPull = 0f;
        }

        /*
        if (Input.GetKeyDown(keyCodePull)||Input.GetKeyDown(joystickPullButton))
        {
            //Action(ForceDirection.Pull);
            GetForceStrength(ForceDirection.Pull);
        }
        if (Input.GetKeyDown(keyCodePush)||Input.GetKeyDown(joystickPushButton))
        {
            //Action(ForceDirection.Push);
            GetForceStrength(ForceDirection.Push);

        }*/
        
        /*
        if (Input.GetKeyDown(keyCodePull))
        {
            Action(ForceDirection.Pull, GetVecToMouse(), 30);
        }
        if (Input.GetKeyDown(keyCodePush))
        {
            Action(ForceDirection.Push, GetVecToMouse(), 30);
        }
        */
    }
}

