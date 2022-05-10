using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EpicViewTrigger : RotateCameraEvent {

    void OnTriggerEnter(Collider other)
    {
        //StartCoroutine(RotateContinous(Quaternion.Euler(0,90,0)));
        //vcam.transform.rotation = Quaternion.Euler(0, 90, 0);
        //vcam.LookAt = lookAt.transform;

        if(other.CompareTag("Player")){
            vcam2.Priority = 100;
            vcam.Priority = 50;
        }
    }
}