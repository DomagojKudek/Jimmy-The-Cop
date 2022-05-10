using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformStick : MonoBehaviour {
    public enum moveDirection
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Back
    }
    Dictionary<moveDirection, Vector3> directions = new Dictionary<moveDirection, Vector3>()
        {
            {moveDirection.Left, Vector3.left},
            {moveDirection.Right, Vector3.right},
            {moveDirection.Up, Vector3.up},
            {moveDirection.Down, Vector3.down},
            {moveDirection.Forward, Vector3.forward},
            {moveDirection.Back, Vector3.back}
        };
    public float speed = 5.5f;
    public float distance = 5f;
    public moveDirection direction;
    private Vector3 moveDirectionVector;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        moveDirectionVector = directions[direction];
        startPosition = this.transform.position;
        endPosition = this.transform.position + directions[direction] * distance;
        
    }
    void FixedUpdate()
    {
        if(!StopClause(moveDirectionVector, endPosition)){
            rb.MovePosition(this.transform.position + moveDirectionVector * Time.deltaTime * speed);
        }else{
            //swap direction
            moveDirectionVector *= -1;
            var temp = startPosition;
            startPosition = endPosition;
            endPosition = temp;
            rb.MovePosition(this.transform.position + moveDirectionVector * Time.deltaTime * speed);
        }
    }
    private bool StopClause(Vector3 vec, Vector3 endPosition)
    {
        //Vector3 vec = moveDirection;
        if (vec == Vector3.up)
        {
            return this.transform.position.y > endPosition.y;
        }
        else if (vec == Vector3.down)
        {
            return this.transform.position.y < endPosition.y;
        }
        else if (vec == Vector3.right)
        {
            return this.transform.position.x > endPosition.x;
        }
        else if (vec == Vector3.left)
        {
            return this.transform.position.x < endPosition.x;
        }
        else if (vec == Vector3.forward)
        {
            return this.transform.position.z > endPosition.z;
        }
        else if (vec == Vector3.back)
        {
            return this.transform.position.z < endPosition.z;
        }
        return false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = rb.velocity;

        }
    }
}
