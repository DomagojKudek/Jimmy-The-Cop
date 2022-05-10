using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Scripts;
using XInputDotNetPure; // Required in C#

public class PlayerHealth : MonoBehaviour
{
    public GameObject playerHat;
    public GameObject playerBadge;

    private int hp;
    private bool invulnerable;
    private bool hit=false;
    public int startHp;
	public Animator anim;
	
    //public GameObject life;
    //public Canvas canvas;

    private List<GameObject> lifeObjects;
    private HUDManager hudManager;
	private AudioManager audioManager;
	private AudioSource audioSource;
	
    void Start()
    {
		audioSource=this.GetComponent<AudioSource>();
		audioManager=AudioManager.instance;
        lifeObjects = new List<GameObject>();
        hp = startHp;
        invulnerable = false;
        this.transform.GetChild(0).gameObject.SetActive(true);
        //RenderHp();
        hudManager = HUDManager.instance;
        hudManager.RenderHp(ref lifeObjects, hp);
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetHp(int n){
        hp = n;
        hudManager.RenderHp(ref lifeObjects, hp);
    }

    public IEnumerator ChangeHpWithKnockback(int n, Transform otherGo){
        Vector3 direction= otherGo.position -this.gameObject.transform.position;
        this.gameObject.GetComponent<Rigidbody>().AddForce(-direction.normalized*10f,ForceMode.Impulse);
        //print("Inv = " + invulnerable);
        if(!invulnerable){
            //invulnerable = true;
            ChangeHp(n);
            //yield return Flicker();
            yield return null;
            if(this.gameObject.activeSelf == true){
                StartCoroutine(Flicker());
                hit=true;
            }
            else{
                hit=false;
            }
        }
    }

    public void ChangeHp(int n)
    {
        if(invulnerable) return;
        bool isDmg = n < 0;
        if (isDmg)
        {
            if (hp + n <= 0)
            {
				//anim.SetBool("Dead",true);
                //Todo Death animation
                hp = 0;
                //RenderHp();
                hudManager.RenderHp(ref lifeObjects, hp);
                //Destroy(this.gameObject);
                GameManager.KillPlayer();
				this.gameObject.GetComponent<JimmyController1>().ClearCollisionList();
            }
            else
            {
				anim.SetBool("Hurt",true);
                //TODO dmg animacija
                hp += n;
				audioManager.Play("Grunt");
            }
        }
        else
        {
            //Todo healing animacija
            hp += n;
        }

        //RenderHp();
        hudManager.RenderHp(ref lifeObjects, hp);
        //Debug.Log(hp);
    }

    public void HealToFull(){
        hp = startHp;
        hudManager.RenderHp(ref lifeObjects, hp);
    }

    public int fallBoundary = -20;

    public void Update()
    {
        //Bolje bi bilo da imamo "pod smrti"
        if (transform.position.y <= fallBoundary)
        {
            Debug.Log(this.name+transform.position.y + "DEAD");
            ChangeHp(-1000000);
        }

        if(Input.GetKeyDown(KeyCode.I)){
            ChangeHp(-1);
        }

        if(Input.GetKeyDown(KeyCode.G)){
            invulnerable = !invulnerable;

            if (invulnerable)
            {
                //mesto da se vidi da je godmode
                //this.transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.color += new Color(20, 0,0,0);
                playerBadge.GetComponent<MeshRenderer>().material.color += new Color(20, 0, 0, 0);
                playerHat.GetComponent<MeshRenderer>().material.color += new Color(20, 0, 0, 0);

            }

            if(!invulnerable){
                //vrati u staro
                //this.transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.color -= new Color(20, 0, 0, 0);
                playerBadge.GetComponent<MeshRenderer>().material.color -= new Color(20, 0, 0, 0);
                playerHat.GetComponent<MeshRenderer>().material.color -= new Color(20, 0, 0, 0);
            }
        }
    }

    public IEnumerator Flicker(){
        //if(!invulnerable){
        invulnerable = true;
        for(int i=0; i < 4; i++){
            //referenca na cop
            foreach(Transform c in this.transform.GetChild(1)){
                c.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.1f);
            //this.transform.GetChild(0).gameObject.SetActive(true);
            foreach (Transform c in this.transform.GetChild(1))
            {
                c.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.15f);
            /*
            //Mozda bi bilo fora zamijeniti avatara 
            tmp = this.transform.GetChild(0).gameObject.GetComponent<Animator>().avatar;
            this.transform.GetChild(0).gameObject.GetComponent<Animator>().avatar = null;
            yield return new WaitForSeconds(0.1f);
            this.transform.GetChild(0).gameObject.GetComponent<Animator>().avatar = tmp;
            yield return new WaitForSeconds(0.15f);
            */    
        }
        invulnerable = false;
        //}
    }

    void FixedUpdate() {
        if(GameManager.instance.joystick==true&& hit==true){
            StartCoroutine(vibrate());
        }
        //else if(GameManager.instance.joystick==true&& hit==false){
        //   GamePad.SetVibration(0,0,0);
        //}
    }
    IEnumerator vibrate(){
        GamePad.SetVibration(0, 1, 1);
        yield return new WaitForSeconds(1f);
        GamePad.SetVibration(0,0,0);
        hit=false;
        //Debug.LogWarning("Vibration");
    }
    
    public bool deal_dmg = false;
    public bool heal_dmg = false;
    /*
    void Update()
    {
        if (deal_dmg)
        {
            ChangeHp(-1);
            deal_dmg = false;
        }

        if (heal_dmg)
        {
            ChangeHp(1);
            heal_dmg = false;
        }
    }
    */
    
}

