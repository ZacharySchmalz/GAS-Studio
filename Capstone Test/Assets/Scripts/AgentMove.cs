using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMove : MonoBehaviour {

    public Transform target;

    private NavMeshAgent agent;
    
	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (target)
        {
            agent.SetDestination(target.position);
        }
	}
}
