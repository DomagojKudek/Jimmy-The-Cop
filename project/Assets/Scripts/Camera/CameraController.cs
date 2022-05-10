using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform player;       //Public variable to store a reference to the player game object


    public Vector3 offset=new Vector3(0,0,-25);         //Private variable to store the offset distance between the player and camera

    public float smoothing = 5f;

    float nextTimeToSearch = 0;
    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.

            transform.position=player.transform.position+ offset;

    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }
            
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        Vector3 targetCamPos=player.transform.position+ offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing *Time.deltaTime);
    }
    void FindPlayer()
    {
        if (nextTimeToSearch <= Time.time)
        {
            GameObject searchResult= GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
            {
                player = searchResult.transform;
            }
            nextTimeToSearch = Time.time + 0.5f;
        }
    }
}


