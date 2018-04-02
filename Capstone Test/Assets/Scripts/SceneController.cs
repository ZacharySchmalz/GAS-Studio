using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public bool isPaused = false;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
            }
            else
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
