using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daytime : MonoBehaviour
{
    public float finalRot = 160;
    public float rotSpeed = 1;
    [Range(0, 1)]
    public float rotationProgress = 0;

    //[Range(0, 1)]
    //public float currentTimeOfDay = 0;
    // Use this for initialization
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
            rotationProgress += Time.deltaTime * rotSpeed;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.transform.rotation*Quaternion.Euler(Vector3.right * finalRot), rotationProgress);
    }
}

