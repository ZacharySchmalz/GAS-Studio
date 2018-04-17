using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {
    public GameObject[] waypoints;
    private GameObject[] players;
    // Use this for initialization
    //Waypoints automatically detects all waypoints named "Waypoint [#]"
    void Start () {
        waypoints = new GameObject[GameObject.FindGameObjectsWithTag("Waypoint").Length];
        for(int i = 0; i < waypoints.Length; i++)
        {
            string waypointName = "Waypoint " + (i + 1);
            waypoints[i] = GameObject.Find(waypointName);
        }
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            player.GetComponent<DestinationManager>().SendMessage("UpdateWaypoints");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
