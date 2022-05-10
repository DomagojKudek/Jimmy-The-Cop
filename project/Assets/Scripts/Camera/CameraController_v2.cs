using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_v2 : MonoBehaviour {

    public Transform player;       //Public variable to store a reference to the player game object
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;
    public float yPosRestriction = -1;

    public float offsetZ;
    Vector3 lastTargetPosition;
    Vector3 currentVelocity;
    Vector3 lookAheadPos;

    public Vector3 offset;         //Private variable to store the offset distance between the player and camera

    float nextTimeToSearch = -20;
    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        lastTargetPosition = player.position;
        offset = transform.position - player.transform.position;
        transform.parent = null;

    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }
        float xMoveDelta = (player.position - lastTargetPosition).x;
        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
        if (updateLookAheadTarget)
        {
            lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);

        }
        else
        {
            lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);

        }
        Vector3 aheadTargetPos = player.position + lookAheadPos + Vector3.forward * offsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

        newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, yPosRestriction, Mathf.Infinity), newPos.z);

        transform.position = newPos;
        lastTargetPosition = player.position;
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        //transform.position = player.transform.position + offset;
    }
    void FindPlayer()
    {
        if (nextTimeToSearch <= Time.time)
        {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
            {
                player = searchResult.transform;
            }
            nextTimeToSearch = Time.time + 0.5f;
        }
    }
}
