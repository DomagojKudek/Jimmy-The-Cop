using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimmyController1 : MonoBehaviour
{
	public float jumpManouverability = 3;//10
    public float moveSpeed=17;//10
    public float jumpVelocity=10;//15
	public float stopForce=2;
	public float jumpStartForce=9;
	public float startForce=2;
	public float maxVelocity=6;
	public float floatiness = 3.5f;
	public float rotationSpeed=720;
	public bool leftRotation=false;
	public bool rightRotation=false;




    public KeyCode joystickJumpButton = KeyCode.JoystickButton0;
    public KeyCode jumpButton = KeyCode.Space;
	
	
	private Vector3 gravity;
    private Vector3 moveDirection;
	private Vector3 currentSpeed;
    private bool doubleJump = false;
	
	
    public Animator anim;
	
	public List <AudioClip> clips;
	private AudioSource speaker;
	public AudioClip jumpSound;
	public AudioClip[] runSound;
	
	private int currentRunSound;
	private Rigidbody rigidBody;
	private List<Collider> m_collisions = new List<Collider>();
	private List<Collider> v_collisions = new List<Collider>();
	private float horizontalInput;
	
	
	
    // Use this for initialization
	
	private bool isGrounded;
	private bool verticalWall;
	
    void Start()
    {
		gravity=new Vector3(0,-floatiness,0);
		currentRunSound=0;
		rigidBody= GetComponent<Rigidbody>();
		speaker = GetComponent<AudioSource>();
		//runningSound = GetComponent<AudioSource>();
    }

	public void ClearCollisionList(){
		m_collisions.Clear();
		v_collisions.Clear();
	}
	

	private void OnCollisionEnter(Collision collision)
    {

		
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
		bool ceiling = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

		for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) < -0.5f)
            {
                ceiling = true; break;
            }
        }
		
        if(validSurfaceNormal)
        {
            
			if(horizontalInput!=0 && !isGrounded){
				rigidBody.AddForce(new Vector3(horizontalInput* moveSpeed, 0, 0)*jumpStartForce,ForceMode.Force);
			}
			isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        } else
        {	
			if(!isGrounded && !verticalWall && !ceiling){
				rigidBody.AddForce(Vector3.up*7,ForceMode.Impulse);
				rigidBody.velocity=new Vector3(0,rigidBody.velocity.y,0);
			}
			verticalWall=true;
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { isGrounded = false; }
			
			if (!v_collisions.Contains(collision.collider))
            {
                v_collisions.Add(collision.collider);
            }
		}
    }

    private void OnCollisionExit(Collision collision)
    {	
		
        if(m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { isGrounded = false; }
		
		if(v_collisions.Contains(collision.collider))
        {
            v_collisions.Remove(collision.collider);
        }
        if (v_collisions.Count == 0) { verticalWall = false; }
    }
    // Update is called once per frame
    void FixedUpdate()
    {	
		if (m_collisions.Count == 0) { isGrounded = false; }
		//print(onLeftSlopedPlatform);
		horizontalInput=Input.GetAxis("Horizontal");
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, 0);
		

		if(!speaker.isPlaying&&anim.GetCurrentAnimatorStateInfo(0).IsName("Run")){
			speaker.clip=runSound[currentRunSound];
			speaker.Play();
			if(currentRunSound==6)currentRunSound=0;
			else currentRunSound++;
		}
		
		
		
		if((moveDirection.x<0 && transform.forward.x>0)){
			rigidBody.velocity=new Vector3(0,rigidBody.velocity.y,0);
			leftRotation=true;
			rightRotation=false;
		}
		else if((moveDirection.x>0 && transform.forward.x<0)){

			rigidBody.velocity=new Vector3(0,rigidBody.velocity.y,0);
			leftRotation=false;
			rightRotation=true;

		}

		if(rightRotation){
			transform.rotation=Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 90,0), rotationSpeed * Time.deltaTime);
		}else if(leftRotation){
			transform.rotation=Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 270,0), rotationSpeed * Time.deltaTime);

		}
		
		if(moveDirection.x==0)rigidBody.AddForce(new Vector3(-rigidBody.velocity.x*stopForce,0,0),ForceMode.Force);
		else if(rigidBody.velocity.x==0) rigidBody.AddForce(moveDirection.normalized*startForce,ForceMode.Impulse);
		if(rigidBody.velocity.x>-maxVelocity && rigidBody.velocity.x<maxVelocity){
			if(isGrounded) rigidBody.AddForce(moveDirection.normalized*moveSpeed,ForceMode.Force);
			else  if(!verticalWall) rigidBody.AddForce(moveDirection.normalized*jumpManouverability,ForceMode.Force);
		}
		if(!isGrounded) rigidBody.AddForce(gravity,ForceMode.Force);
		//print(isGrounded);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("Speed",(Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }
	
	private void Update(){
		//if(v_collisions.Count==0&&m_collisions.Count==0)print("u zraku sam");
		if (isGrounded)
        {

            //if (Input.GetButtonDown("Jump"))
            if (Input.GetKeyDown(jumpButton) || Input.GetKeyDown(joystickJumpButton))
            {
				rigidBody.velocity=Vector3.zero;
				if(moveDirection.x!=0)rigidBody.AddForce(Vector3.up * jumpVelocity*(float)1.2, ForceMode.Impulse);
				else rigidBody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
                doubleJump = true;
				speaker.clip=jumpSound;
				speaker.Play();
				anim.SetBool("Jumping",true);
            }
			
        }
        else
        {
            //if (Input.GetButtonDown("Jump") && doubleJump == true)
            if ((Input.GetKeyDown(jumpButton) || Input.GetKeyDown(joystickJumpButton))&& doubleJump == true)
            {
				rigidBody.velocity=Vector3.zero;
				rigidBody.AddForce(jumpStartForce*moveDirection);
				if(moveDirection.x!=0)rigidBody.AddForce(Vector3.up * jumpVelocity*(float)1.25, ForceMode.Impulse);
				else rigidBody.AddForce(Vector3.up * jumpVelocity*(float)1.5, ForceMode.Impulse);
                doubleJump = false;
				speaker.clip=jumpSound;
				speaker.Play();
				anim.SetBool("DoubleJumping",true);

            }
            moveDirection.y += Physics.gravity.y * 5 * Time.deltaTime;
        }
	}
}