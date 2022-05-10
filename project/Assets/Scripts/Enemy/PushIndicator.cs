using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PushIndicator : MonoBehaviour
{
    public float forceStrength = 50f;
    public float maskRadius = 20f;

    public float maxRange = 300f;

    private GameObject hitRange;
    private GameObject safeRange;
    private Vector3 startPosition;
    private float speed;

    private Coroutine corutine;
    private Coroutine corutine2;
    private bool isInit = false;
    public float durationPush=1.2f;
    public float durationCharge=4.2f;
    public float safeDeadzoneMultiplier=0.8f;
    // Update is called once per frame
    //max range ovisi o speedu

    private void Awake(){
        hitRange = this.transform.GetChild(0).gameObject;
        hitRange.transform.localScale = new Vector3(1,1,1) * maxRange/2;
        safeRange = this.transform.GetChild(1).gameObject;
        safeRange.transform.localScale = new Vector3(1, 1, 1) * maskRadius/2; // /2 jer je localScale promjer
        //hitRange.gameObject.SetActive(false);

        hitRange.GetComponent<Collider>().enabled=false;
        hitRange.GetComponent<MeshRenderer>().enabled=false;
        //safeRange.gameObject.SetActive(false);
        safeRange.GetComponent<MeshRenderer>().enabled=false;
    }
    public void Init(Vector3 startPosition, float speed)
    {
        this.startPosition = startPosition;
        this.speed = speed;
        isInit = true;
    }

 

    //kopirano iz pusha, djeluje na playera i breakable
    public void Action(){
        //Collider[] colliders = Physics.OverlapSphere(this.transform.position, maxRange);
        //Collider[] collidersIgnored = Physics.OverlapSphere(this.transform.position, maskRadius);GetComponent<SphereCollider>().radius
        Collider[] colliders = Physics.OverlapSphere(this.startPosition, hitRange.GetComponent<MeshRenderer>().bounds.extents.magnitude);
        Collider[] collidersIgnored = Physics.OverlapSphere(this.startPosition, safeRange.GetComponent<MeshRenderer>().bounds.extents.magnitude*safeDeadzoneMultiplier*0.8f);
        GameObject player = GameObject.FindWithTag("Player");
        Collider[] impacted = colliders.Except(collidersIgnored).ToArray();
        //Debug.LogWarning("colision");
        foreach (Collider col in impacted)
        {
            GameObject go = col.gameObject;
            //Debug.LogWarning(col.name,col);
            Vector3 direction;
            if (!(go.CompareTag("Player") || go.CompareTag("Breakable"))) continue;

            if (go.CompareTag("Breakable"))
            {
                //disable collisions bettween player and debris
                Physics.IgnoreCollision(player.GetComponent<Collider>(), go.GetComponent<Collider>());
                Destroy(go, 5);
                go.tag = "UnCollidable";
                direction = go.transform.position - this.transform.position;
                if (go.GetComponent<Rigidbody>() != null)
                {
                    go.GetComponent<Rigidbody>().useGravity = true;
                    go.GetComponent<Rigidbody>().isKinematic = false;
                    go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                }
            }

            if (go.CompareTag("Player"))
            {
                direction = go.transform.position - this.transform.position;
                if (go.GetComponent<Rigidbody>() != null)
                    go.GetComponent<Rigidbody>().AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                    StartCoroutine(player.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform));
                    //player.GetComponent<PlayerHealth>().ChangeHp(-1);
            }
        }
    }

    public void End(){
        
        hitRange.transform.localScale = new Vector3(1,1,1) * maxRange/2;
        //hitRange.gameObject.SetActive(false);
        //safeRange.gameObject.SetActive(false);
        hitRange.GetComponent<Collider>().enabled=false;
        hitRange.GetComponent<MeshRenderer>().enabled=false;
        //safeRange.gameObject.SetActive(false);
        safeRange.GetComponent<MeshRenderer>().enabled=false;
        if(corutine != null)
            StopCoroutine(corutine);
        if(corutine2 != null)
            StopCoroutine(corutine2);

    }
    //siri samo x i y os, tako da se uvijek vidi
    private IEnumerator ScaleUpSphere(float speed){
        hitRange.GetComponent<Collider>().enabled=true;
        //hitRange.transform.localScale = Vector3.zero;
        Vector3 ones = new Vector3(1,1,1);
        int frame=0;
        float startTime=Time.time;
        float time=startTime;
        while(true){
            if(hitRange!=null){
                hitRange.transform.localScale=Vector3.Lerp(safeRange.transform.localScale*safeDeadzoneMultiplier,new Vector3(1,1,1) * maxRange/2,(time-startTime)/durationPush);
                //hitRange.localScale += ones * speed * Time.deltaTime /2; // /2 za radius
                //print(frame);
            }
            frame++;
            time+=Time.deltaTime;
            if((time-startTime)/durationPush>=1)
                break;
        
            yield return null;
            
        }
    }

    public void StartAnim()
    {
        corutine = StartCoroutine(ScaleDownSphere(20)); // brzina scale down
    }

    public void StartScaleUpRoutine(){
        corutine2=StartCoroutine(ScaleUpSphere(100f));
    }

    private IEnumerator ScaleDownSphere(float speed){
        hitRange.gameObject.SetActive(true);
        hitRange.GetComponent<Collider>().enabled=false;
        hitRange.GetComponent<MeshRenderer>().enabled=true;
        //safeRange.gameObject.SetActive(true);
        safeRange.GetComponent<MeshRenderer>().enabled=true;
        Vector3 ones = new Vector3(1, 1, 1);
        Vector3 startScale=hitRange.transform.localScale;
        float startTime=Time.time;
        float time=startTime;
        while (true)
        {
            if(hitRange!=null){
                hitRange.transform.localScale=Vector3.Lerp(startScale,safeRange.transform.localScale*safeDeadzoneMultiplier,(time-startTime)/durationCharge);
               // hitRange.transform.localScale -= ones * speed * Time.deltaTime / 2; // /2 za radius  
            }
            time+=Time.deltaTime;
            if((time-startTime)/durationCharge>=1)
                    break;
            yield return null;
        }
    }
}
