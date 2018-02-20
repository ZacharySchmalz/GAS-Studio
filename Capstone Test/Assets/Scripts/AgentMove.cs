using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Moves the agent towards target(s)

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMove : MonoBehaviour {

    public Transform target;
    public List<Transform> targets;
    public int currentTarget;
    public GameObject targetList;
    public float distanceThreshold;     // how far does it take until the agent switches to the next target

    private NavMeshAgent agent;
    private NavTarget[] navTargets;
    private NavMeshPath samplePath;


	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();

        // Populate with targets with children
        if (targetList)
        {
            for (int i = 0; i < targetList.transform.childCount; i++)
            {
                targets.Add(targetList.transform.GetChild(i));
            }
        }
                
// used for dynamically adding object in the nav mesh environment, not yet complete
//        navTargets = FindObjectsOfType<NavTarget>();
//        samplePath = new NavMeshPath();
//        foreach (NavTarget t in navTargets)
//        {
//            targets.Add(t.transform);
//        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (targetList)
        {
            if (!agent.pathPending && agent.remainingDistance < distanceThreshold)
            {
                GoToNextDestination();
            }
        }

    }

    // Currently not in use
    void SampleTargetPaths()
    {
        foreach (Transform t in targets)
        {
            if (NavMesh.CalculatePath(this.transform.position, t.position, NavMesh.AllAreas, samplePath))
            {
                agent.SetPath(samplePath);
                Debug.Log("Destination: " + agent.destination);
            }
            Debug.Log(samplePath);
        }
    }

    void GoToNextDestination()
    {
        if (targets.Count == 0)
            return;

        agent.SetDestination(targets[currentTarget].position);
        currentTarget = (currentTarget + 1) % targets.Count;
    }
}
