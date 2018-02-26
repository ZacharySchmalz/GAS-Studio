using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float timer = 120.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		string minutes = ((int)timer / 60).ToString ();
		string seconds = (timer % 60).ToString ("f0");

		timerText.text = minutes + ":" + seconds;
	}
}
