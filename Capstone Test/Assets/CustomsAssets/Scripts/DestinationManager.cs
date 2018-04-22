using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationManager : MonoBehaviour {

    public GameObject[] waypoints;
    public int totalLaps;
    public int currentWaypoint = 0;
    public int currentLap = 0;
    private CarControlCS carControl;

	public delegate void UpdateWaypoint();
	public static event UpdateWaypoint OnGetWaypoint;

    public void UpdateWaypoints()
    {
        waypoints = GameObject.FindGameObjectWithTag("GameController").GetComponent<Waypoints>().waypoints;
        if (waypoints.Length > currentWaypoint)
        {
            carControl = GetComponent<CarControlCS>();
            carControl.Destination = waypoints[currentWaypoint].transform.position;
        }
    }
    
	// Use this for initialization
	void Start () 
	{
        Waypoints waypointManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Waypoints>();
        waypoints = waypointManager.waypoints;
        totalLaps = waypointManager.totalLaps;
        
        /*if (waypoints.Length > currentWaypoint)
        {
            carControl = GetComponent<CarControlCS>();
            carControl.Destination = waypoints[currentWaypoint].transform.position;
        }*/
    }
	
	// Update is called once per frame
	void Update () 
	{
        CheckWaypoint();
	}

    private void CheckWaypoint()
    {
        if (currentWaypoint < waypoints.Length && waypoints.Length > 0)
        {
            if (Vector3.Distance(this.transform.position, waypoints[currentWaypoint].transform.position) < 5)
            {
                //Move to next waypoint
				if (OnGetWaypoint != null) 
				{
					OnGetWaypoint ();
				}

                currentWaypoint++;
				Debug.Log("Current Waypoint " + currentWaypoint);
                if (currentWaypoint < waypoints.Length)
                {
                    carControl.Destination = waypoints[currentWaypoint].transform.position;
                }
                else if (currentLap < totalLaps)
                {
                    currentWaypoint = 0;
                    carControl.Destination = waypoints[currentWaypoint].transform.position;
                    currentLap++;
                }

            }
        }

    }
}
