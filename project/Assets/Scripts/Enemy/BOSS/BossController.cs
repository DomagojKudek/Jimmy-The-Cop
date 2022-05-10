using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.Linq;
using Panda;
using XInputDotNetPure;
[RequireComponent(typeof(Rigidbody))]
public class BossController : MonoBehaviour {

	public float speed = 1.0f;
	private bool vibrate_on=false;
	private bool vibrate_on_push=false;
	public float vibration_duration=0.7f;
	public float left_vibration_rumble_strength=1.0f;
	public float right_vibration_noise_strength=0.5f;
	public float leftpush_vibration_rumble_strength=1.0f;
	public float rightpush_vibration_noise_strength=1.0f;
    public Animator animator;

    public GameObject shockwaveParentPrefab;
	public Transform bombParent;
    public GameObject bombPrefab;

	public PushIndicator pushIndicator;

	public float pushRadius = 10;

    private Transform playerTransform;
	private Vector3 destination;
	private float lastAttackTime = 0f;

	private bool faceLeft;
	private Rigidbody rigidbody;

	private int stage;

	private List<GameObject> bombList;
	public float initBlastSphereRadius=1;
	
	public List <AudioClip> walkingSounds;
	public AudioClip slamSound;
	public AudioClip pushSound;
	private int currentWalkSound=0;
	private AudioSource speaker;
	public GameObject cinemachineShakeObj;
    private CameraShakeCinemamachine cinemachineShake;

	private void Start(){
		cinemachineShake = cinemachineShakeObj.GetComponent<CameraShakeCinemamachine>();
		speaker = GetComponent<AudioSource>();
		playerTransform = PlayerManager.instance.transform;
		destination = playerTransform.position;
		rigidbody = this.GetComponent<Rigidbody>();
		bombList = new List<GameObject>();
		stage = 1;
		animator.SetBool("walk", true);
		faceLeft = true;
	}

	private float DistanceToPlayer(){
		return Mathf.Abs(playerTransform.position.x - this.transform.position.x);
	}

	[Task]
	bool CheckCanAttack(int time){
		if(Time.time - lastAttackTime > time){
			lastAttackTime = Time.time;
			return true;
		}
		else{
            return false;
        }
	}

	[Task]
	bool CheckStage(int stage){
		return stage == this.stage;
	}

	[Task]
	bool CheckPlayerClose(){
		return DistanceToPlayer() < 10f;// && playerTransform.position.y < this.transform.position.y;
	}

	[Task]
	void AttackClose(){
		print("ATTACKING CLOSE!");
		Task.current.Succeed();
	}

    [Task]
    void AttackFar()
    {
        print("ATTACKING FAR!");
        Task.current.Succeed();
    }

    [Task]
    bool SetDestination_Player()
    {
        destination = playerTransform.position;	
        return true;
    }
	
	//Todo namjestiti animacuju ovisno o brzini hodanja
    [Task]
    void MoveToDestination()
    {		 
        var task = Task.current;
		float deltaX = destination.x - transform.position.x;
		//print(deltaX);

		if(Mathf.Abs(deltaX)< 0.1){
            task.Fail();
			//tree ode na set animation idle
			return;
        }

		int directionX = deltaX > 0 ? 1 : -1;

		//face moving direction
		if (directionX == 1 && faceLeft || directionX == -1 && !faceLeft){
            //rotate if directionX != face direction
			//TODO rotacija s interpolacijom
            transform.Rotate(0, 180, 0, Space.Self);
			faceLeft = !faceLeft;
			// mirroring
			transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
		}

        Vector3 delta = new Vector3(directionX, 0, 0);
        rigidbody.MovePosition(this.transform.position + delta * Time.deltaTime * speed);
		if(!speaker.isPlaying){
			speaker.clip=walkingSounds[currentWalkSound];
			speaker.Play();
			if(currentWalkSound==3)currentWalkSound=0;
			else currentWalkSound++;
		}
        //print("MOVING");
        task.Succeed();
    }

	[Task]
	bool SetAnimation(string state){
		animator.SetTrigger(state);
		return true;
	}

	[Task]
	bool PlayAnimation(string stateName){
		animator.Play(stateName, 0);
		return true;
	}

	[Task]
	bool IsAnimationPlaying(string state){
		return animator.GetBool(state);
		//return animator.GetCurrentAnimatorStateInfo(0).IsName($"Base Layer.{state}");
	}

    [Task]
    bool IsAnimationPlaying(string state1, string state2)
    {
        return animator.GetBool(state1) || animator.GetBool(state2);
    }

	public void GroundAttackEvent(){
		switch(stage){
			case 1:
				GroundAttackStage1();
				break;
            case 2:
                GroundAttackStage2();
                break;
		}
	}

	public void ChangeStage(){
		if(stage == 1)
			stage = 2;
	}

	public void ResetStage(){
		stage = 1;
	}


	private void GroundAttackStage1(){
        Vector3 contactPoint = pushIndicator.transform.position + new Vector3(0, 3, 0);
        GameObject shockwave = Instantiate(shockwaveParentPrefab, contactPoint, Quaternion.identity);
		speaker.clip=slamSound;
		speaker.Play();
		cinemachineShake.shake_on=true;
        cinemachineShake.start=true;
		vibrate_on=true;
        shockwave.GetComponent<ShockwaveParent>().Init(speed: 20f, startPosition: contactPoint);
		Collider[] colliders = Physics.OverlapSphere(shockwave.transform.position, initBlastSphereRadius);

		foreach (Collider col in colliders)
		{
		
			GameObject go = col.gameObject;
			if (go.CompareTag("Player"))
			{
				StartCoroutine(go.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, shockwave.transform));

			}
		}
	}

    private void GroundAttackStage2()
    {
        //u toclki kontakta stvori shockwave koji se siri
        Vector3 contactPoint = pushIndicator.transform.position + new Vector3(0,3,0);
		speaker.clip=slamSound;
		speaker.Play();
		cinemachineShake.shake_on=true;
        cinemachineShake.start=true;
		vibrate_on=true;
        for (int i = 0; i < 3; i++)
        {
            //3 s razlicitim brzinama, ostavi i stari
            GameObject shockwave = Instantiate(shockwaveParentPrefab, contactPoint, Quaternion.identity);
            shockwave.GetComponent<ShockwaveParent>().Init(speed: 7f * (i+1), startPosition: contactPoint);
			if(i==0){
				Collider[] colliders = Physics.OverlapSphere(shockwave.transform.position, initBlastSphereRadius);

				foreach (Collider col in colliders)
				{
				
					GameObject go = col.gameObject;
					if (go.CompareTag("Player"))
					{
						StartCoroutine(go.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, shockwave.transform));

					}
				}
			}
        }
    }

	public void SpawnBombsEvent(){
		bombList.Clear();
		for(int i = 0; i < 3; i++){
			print("Spawn bombs");
			Vector3 bombInitalPosition = bombParent.transform.position;
			bombInitalPosition.z = 0;
            //GameObject bombInstance = Instantiate(bombPrefab, bombInitalPosition, Quaternion.identity, bombParent);
            GameObject bombInstance = Instantiate(bombPrefab, bombParent);
            bombInstance.transform.localScale = new Vector3(30,30,30);
			bombList.Add(bombInstance);
		}
	}

	public void ThrowBombsEvent(){
        int directionX = faceLeft ? -1 : 1;
        for(int i = 0; i < 3; i++){
            float force = 40f;
            GameObject bomb = bombList[i];
			bomb.transform.parent = null;
			bomb.layer = 11; //IgnoreCollisions, layer ignorira samo sebe
			bomb.transform.position = new Vector3(bomb.transform.position.x, bomb.transform.position.y, 0);
            StartCoroutine(bomb.GetComponent<Bomb>().ActivateBomb());
			//TODO pravi vektor
			Vector3 normalOnHand = new Vector3(directionX , 1, 0);
            bomb.GetComponent<Rigidbody>().isKinematic = false;
            bomb.GetComponent<Rigidbody>().AddForce(normalOnHand.normalized * force * ((i+1) * 0.1f), ForceMode.Impulse);
		}
        //bombList.Clear();
    }

	public void PushIndicatorEvent(){
		speaker.clip=pushSound;
		speaker.Play();
		pushIndicator.Init(this.transform.position, 20);
		pushIndicator.StartAnim();
		
		//pushIndicator.transform.GetChild(0).GetComponent<BossPushActionCollide>().enabled = false;
	}

	public void PushEvent(){
		cinemachineShake.shake_on=true;
        cinemachineShake.start=true;
		vibrate_on=true;
		vibrate_on_push=true;
		pushIndicator.StartScaleUpRoutine();
        //pushIndicator.transform.GetChild(0).GetComponent<BossPushActionCollide>().enabled = true;
    }

	public void EndPushEvent(){
        pushIndicator.End();
		vibrate_on_push=false;
	}

	public void GarbageCollectPush(){
		pushIndicator.End();
		//TODO ugasi zvuk
	}

	public void GarbageCollectThrow(){
		if(bombList.Any()){//ako lista nije prazna
			for(int i = bombList.Count - 1; i >= 0; i--){
				Destroy(bombList[i]); 
			}
			bombList.Clear();
		}
	}

	public void GarbageCollectShockwave(){
		GameObject[] shockwaves = GameObject.FindGameObjectsWithTag("Shockwave");
		for( int i = shockwaves.Length - 1; i >= 0; i--){
			Destroy(shockwaves[i]);
		}
	}

	public void GarbageCollectAll(){
		GarbageCollectThrow();
		GarbageCollectPush();
		GarbageCollectShockwave();
	}
	IEnumerator vibrate(){
		if(vibrate_on_push){
			GamePad.SetVibration(0, leftpush_vibration_rumble_strength, rightpush_vibration_noise_strength);		
		}else{
            GamePad.SetVibration(0, left_vibration_rumble_strength, right_vibration_noise_strength);
		}
			vibrate_on=false;
            yield return new WaitForSeconds(vibration_duration);
            GamePad.SetVibration(0,0,0);
            
        }
	void FixedUpdate() {
		if(GameManager.instance.joystick==true&& vibrate_on==true){
			StartCoroutine(vibrate());
		}
	}
}
