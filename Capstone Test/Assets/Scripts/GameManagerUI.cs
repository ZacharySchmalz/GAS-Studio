using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour {

	public Text gameStatusText;

	private string[] comments = new string[] { "Good job!", "Great!", "You got it!" };

	// Use this for initialization
	void Awake()
	{
		DestinationManager.OnGetWaypoint += UpdateWaypointUI;
		GameManager.OnWin += UpdateWinUI;
		GameManager.OnLose += UpdateLossUI;
	}
	void Start () 
	{
		ResetUI ();	
	}

	public void UpdateWinUI()
	{
		gameStatusText.text = "Great job! Delivery completed.";
	}

	public void UpdateLossUI()
	{
		gameStatusText.text = "You ran out of time.\nBetter luck next time!";
	}

	// ui for when you get a waypoint 
	public void UpdateWaypointUI()
	{
		gameStatusText.text = comments[Random.Range (0, comments.Length)];
	}

	public void ResetUI()
	{
		gameStatusText.text = "";
	}
}
