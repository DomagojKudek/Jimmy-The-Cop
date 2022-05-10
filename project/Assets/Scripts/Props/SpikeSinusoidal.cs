using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSinusoidal : MonoBehaviour {
	public int spikeRowcount=40;
	public float gapBetweenSpikes=0.6f;
	public float speed=2f;
	public float amplitude=1;
	private GameObject spikes2;
	private GameObject spikes3;
	private List<GameObject> spikes=new List<GameObject>();
	public float startHeight;
	public enum moveDirection
     {
         Left,
		 Right
    }
	public moveDirection direction=moveDirection.Right;
	public float neighborDelay=0.2f;
	public float heightDifference=0.2f;
	public float amplitudeDifference=0.5f;
	// Use this for initialization
	void Start () {
		//spikes2 must be first the child, spikes3 the second child
		spikes2=this.gameObject.transform.GetChild(0).gameObject;
		startHeight=spikes2.transform.position.y;
		spikes3=this.gameObject.transform.GetChild(1).gameObject;

		for (int i = 0; i <= spikeRowcount; i+=2){
			GameObject spike2i=Instantiate(spikes2,new Vector3(spikes2.transform.position.x+gapBetweenSpikes*i,spikes2.transform.position.y,spikes2.transform.position.z),spikes2.transform.rotation);
			spikes.Add(spike2i);
			spike2i.transform.parent = gameObject.transform;
			GameObject spike3i=Instantiate(spikes3,new Vector3(spikes2.transform.position.x+gapBetweenSpikes*(i+1),spikes3.transform.position.y,spikes3.transform.position.z),spikes3.transform.rotation);
			spikes.Add(spike3i);
			spike3i.transform.parent = gameObject.transform;
		}
		spikes2.SetActive(false);
		spikes3.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		int dir=direction==moveDirection.Left?1:-1;
		for (int i = 0; i < spikes.Count; i++){
			float delay=i%2==0?neighborDelay:0;
			float heightDiff=i%2==0?heightDifference:0;
			float amplitudeDiff=i%2==0?amplitudeDifference:0;
    		spikes[i].transform.position=new Vector3(spikes[i].transform.position.x,startHeight+heightDiff+(amplitude+amplitudeDiff)*Mathf.Sin(((float)i/spikes.Count)*Mathf.PI*2+Time.time*speed*dir+delay),spikes[i].transform.position.z);
		}

	}
}
