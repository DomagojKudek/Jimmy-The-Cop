using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class AnimatorManager : MonoBehaviour {
	//public GameManager gameManager;
	public Animator anim;
	// Use this for initialization
	

	void FixedUpdate () {
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))anim.SetBool("Dying", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))anim.SetBool("Hurt", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Midair"))anim.SetBool("Jumping", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump"))anim.SetBool("DoubleJumping", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Gravity"))anim.SetBool("Gravity", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("GravityAir"))anim.SetBool("Gravity", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Push"))anim.SetBool("Push", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("PushAir"))anim.SetBool("Push", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Pull"))anim.SetBool("Pull", false);
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("PullAir"))anim.SetBool("Pull", false);
	}
	
}
