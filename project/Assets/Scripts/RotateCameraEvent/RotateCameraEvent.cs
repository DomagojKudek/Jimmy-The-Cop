using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RotateCameraEvent : MonoBehaviour {

	public GameObject lookAt;
	public float rotationTime = 2f;
	public CinemachineVirtualCamera vcam;

	public CinemachineVirtualCamera vcam2;
	/*
	void OnTriggerEnter(){
		//StartCoroutine(RotateContinous(Quaternion.Euler(0,90,0)));
		vcam.transform.rotation = Quaternion.Euler(0, 90, 0);
		vcam.LookAt = lookAt.transform;
	}
	*/
	
	
	private IEnumerator RotateContinous(Quaternion finalRotation){
		Transform cameraTrans = vcam.transform;
		Quaternion startRotation = cameraTrans.rotation;//vcam.transform.rotation;
		//vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth *=20;
		vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 5;
		float t = 0;
		while(cameraTrans.rotation != finalRotation){
            cameraTrans.rotation = Quaternion.Lerp(startRotation, finalRotation, t*Time.deltaTime/rotationTime);
			t++;
			yield return null;
		}
		yield return null;
	}
	

}

