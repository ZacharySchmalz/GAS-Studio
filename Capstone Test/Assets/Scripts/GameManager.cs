using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public DestinationManager destinationManager;
	public GameManagerUI gameManagerUI;
	public Timer timer;

	public delegate void GameStatus ();
	public static event GameStatus OnWin;
	public static event GameStatus OnLose;
	public static event GameStatus OnTimeOut;

	private bool isGameOver;

	// Use this for initialization
	void Start () 
	{
		destinationManager = GetComponent<DestinationManager> ();	
		gameManagerUI = GetComponent<GameManagerUI> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (!isGameOver) 
		{
			if (timer.hasTimedOut) 
			{
				Debug.Log ("Timed out");

				isGameOver = true;

				if (OnLose != null)
					OnLose ();	
			}
			else if (destinationManager.currentWaypoint >= destinationManager.waypoints.Length) 
			{
				Debug.Log ("Got all the waypoints");
				isGameOver = true;

				if (OnWin != null)
					OnWin ();
			}


		}
	}

}
