using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {
    public int totalLaps = 3;
    public GameObject[] waypoints;
    private GameObject player;
    private GameObject AIPlayer;
    // Use this for initialization
    //Waypoints automatically detects all waypoints named "Waypoint [#]"
    void Start () {
        waypoints = new GameObject[GameObject.FindGameObjectsWithTag("Waypoint").Length];
        for(int i = 0; i < waypoints.Length; i++)
        {
            string waypointName = "Waypoint " + (i + 1);
            waypoints[i] = GameObject.Find(waypointName);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<DestinationManager>().SendMessage("UpdateWaypoints");
		if (GameObject.FindGameObjectWithTag ("AIPlayer") != null) {
			AIPlayer = GameObject.FindGameObjectWithTag ("AIPlayer");
			AIPlayer.GetComponent<DestinationManager> ().SendMessage ("UpdateWaypoints");
		}

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
