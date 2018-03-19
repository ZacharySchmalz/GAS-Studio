using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationManager : MonoBehaviour {

    public GameObject[] waypoints;
    public int currentWaypoint = 0;

    private GameObject player;
    private CarControlCS carControl;

    
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        carControl = player.GetComponent<CarControlCS>();
        carControl.Destination = waypoints[currentWaypoint].transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        CheckWaypoint();
	}

    private void CheckWaypoint()
    {
        if (currentWaypoint < waypoints.Length)
        {
            if (Vector3.Distance(player.transform.position, waypoints[currentWaypoint].transform.position) < 5)
            {
                //Move to next waypoint
                currentWaypoint++;
                carControl.Destination = waypoints[currentWaypoint].transform.position;
            }
        }
    }
}
