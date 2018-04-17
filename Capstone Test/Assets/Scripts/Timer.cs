using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
    public bool isCountingUp;
    public bool isCounting = true;
    public float timeInSeconds;
	public bool hasTimedOut;

	private float timer = 120.0f;
	// Use this for initialization
	void Start () 
    {
		hasTimedOut = false;
        if (isCountingUp)
        {
            timer = 0;
        }
        else
        {
            if (timeInSeconds > 0)
                timer = timeInSeconds;
        }
	}
	
	// Update is called once per frame
	void Update () {

		if (!hasTimedOut) 
		{
            if(isCounting)
            {
                if(isCountingUp)
                {
                    timer += Time.deltaTime;
                } else
                {
                    timer -= Time.deltaTime;
                }
            }
			
			
			string minutes = ((int)timer / 60).ToString ();
			string seconds = (timer % 60).ToString ("f0");
			
			timerText.text = minutes + ":" + seconds;

			if (timer < 0)
				hasTimedOut = true;
		}
	}

}
