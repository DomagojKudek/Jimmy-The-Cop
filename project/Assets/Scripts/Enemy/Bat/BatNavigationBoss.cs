using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;
namespace Assets.Scripts.Enemy
{
public class BatNavigationBoss : MonoBehaviour {
	public GameObject player;
	public NavMeshAgent agent;

	public GameObject NavMeshPlane;
	private Mesh NavMeshMesh;

	public Vector3 destination; 

	public float detectionRadius=15;

	// Use this for initialization
	void Start () {
		agent=GetComponent<NavMeshAgent>();
		player = PlayerManager.instance;
		NavMeshMesh=NavMeshPlane.GetComponent<MeshFilter>().mesh;
	}
	
	// Update is called once per frame
	void Update () {
		float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
		if(distanceToPlayer>detectionRadius){
			Vector3 destination=NavMeshPlane.transform.position;
			//print(destination);
			//print(agent.SetDestination(new Vector3(destination.x,destination.y,0)));
		}else{
			agent.SetDestination(player.transform.position);
		}
		
		
	}

}
}