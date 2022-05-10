using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10;//10
    public float jumpVelocity = 22;//15
    public CharacterController controller;
    private Vector3 moveDirection;
    public float gravityScale = 5;//5
    private bool doubleJump = false;
	public KeyCode joystickJumpButton = KeyCode.JoystickButton0;
	public KeyCode jumpButton = KeyCode.Space;
	public List <AudioClip> clips;
    public Animator anim;
	private AudioSource speaker;
	public AudioClip jumpSound;
	public AudioClip runSound;
    // Use this for initialization
    void Start()
    {	
		anim=GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
		speaker = GetComponent<AudioSource>();
		//runningSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, 0);
        GameObject.FindWithTag("Player").GetComponent<Push_Pull>().pointerDirection =
            new Vector3(Input.GetAxis("HorizontalAim"), Input.GetAxis("VerticalAim"), 0);
        if (controller.isGrounded)
        {

            if (Input.GetKeyDown(jumpButton)||Input.GetKeyDown(joystickJumpButton))
            {
                moveDirection.y = jumpVelocity;
                doubleJump = true;
				speaker.clip=jumpSound;
				speaker.Play();

            }
			
        }
        else
        {
            if ((Input.GetKeyDown(jumpButton)||Input.GetKeyDown(joystickJumpButton)) && doubleJump == true)
            {
                moveDirection.y = jumpVelocity;
                doubleJump = false;
				speaker.clip=jumpSound;
				speaker.Play();

            }
            moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;
            //If we hit something above us AND we are moving up, reverse vertical movement
            if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            {
                if (moveDirection.y > 0)
                {
                    moveDirection.y = 0;
                }
            }

        }

		if(!speaker.isPlaying&&anim.GetCurrentAnimatorStateInfo(0).IsName("Run")){
			speaker.clip=runSound;
			speaker.Play();
		}
        controller.Move(moveDirection * Time.deltaTime);
		if(moveDirection.x<0){
			this.transform.rotation= Quaternion.Euler(0,270,0);
			
		}else if(moveDirection.x>0){
			this.transform.rotation= Quaternion.Euler(0,90,0);
			
		}
        this.transform.position=new Vector3(this.transform.position.x,this.transform.position.y,0);
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("Speed",Mathf.Abs(Input.GetAxis("Horizontal")));
    }
}
