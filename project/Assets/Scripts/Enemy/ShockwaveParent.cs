using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveParent : MonoBehaviour
{
    public GameObject shockwavePrefab;
    public GameObject ringPrefab;
    private bool moveLeft;
    private Vector3 startPosition;
    private float speed;
    
    // Update is called once per frame

    //Marko za iscrtavalje koristi radius = speed * Time.deltaTime
    public void Init(float speed, Vector3 startPosition)
    {
        this.speed = speed;
        this.startPosition = startPosition;
        
        GameObject ring = Instantiate(ringPrefab, startPosition,ringPrefab.transform.rotation);
        ring.GetComponent<RingController>().Init(speed: speed, startPosition: startPosition);
        /*GameObject shockwave = Instantiate(shockwavePrefab, startPosition, Quaternion.identity);
        shockwave.GetComponent<Shockwave>().Init(moveLeft: true, speed: speed, startPosition: startPosition);
        GameObject shockwave2 = Instantiate(shockwavePrefab, startPosition, Quaternion.identity);
        shockwave2.GetComponent<Shockwave>().Init(moveLeft: false, speed: speed, startPosition: startPosition);
       */
    }
    
}
