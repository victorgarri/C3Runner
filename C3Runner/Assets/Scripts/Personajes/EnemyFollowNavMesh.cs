using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyFollowNavMesh : MonoBehaviour
{
	private NavMeshAgent agent;
	public GameObject player;
	private bool activado;

	void Update(){
		
		if (player.transform.position.x - transform.position.x > 20f && activado)
		{
			Destroy(gameObject);
		}
		
		if (transform.position.x - player.transform.position.x < 50f)
		{
			activado = true;
		}

		if (activado)
		{
			GetComponent<NavMeshAgent>().destination = player.transform.position;
		}
	}
}
