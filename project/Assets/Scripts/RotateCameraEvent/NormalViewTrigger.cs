using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NormalViewTrigger : RotateCameraEvent
{

    void OnTriggerEnter(Collider other)
    {
        //StartCoroutine(RotateContinous(Quaternion.Euler(0,90,0)));
        //vcam.transform.rotation = Quaternion.Euler(0, 0, 0);
        //vcam.LookAt = null;
        if(other.CompareTag("Player")){
            vcam2.Priority = 50;
            vcam.Priority = 100;
        }
        
    }
}