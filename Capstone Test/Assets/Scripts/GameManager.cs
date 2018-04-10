using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public DestinationManager destinationManager;
	public GameManagerUI gameManagerUI;
	public Timer timer;
    public Text CountInText;
    public GameObject pauseScreen;
    public GameObject GUICanvas;
    public GameObject GUICamera;

    public bool isPaused = false;


    public delegate void GameStatus ();
	public static event GameStatus OnWin;
	public static event GameStatus OnLose;
	public static event GameStatus OnTimeOut;

	private bool isGameOver;
    private bool isCountIn;
    private CarControlCS controlScript;

    // Use this for initialization
    void Start () 
	{
		destinationManager = GetComponent<DestinationManager> ();	
		gameManagerUI = GetComponent<GameManagerUI> ();
        isCountIn = true;
        timer.isCounting = false;
        controlScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CarControlCS>();
        controlScript.isControlActive = false;
	}
	
	// Update is called once per frame
	void Update () 
	{

        //Count down start
        if(Time.timeSinceLevelLoad < 3.0)
        {
            float countInTime = Time.timeSinceLevelLoad;
            CountInText.text = (3 - (int)countInTime).ToString();
            CountInText.transform.localScale = new Vector3((countInTime - (int)countInTime), (countInTime - (int)countInTime), 1);
        } else if (isCountIn)
        {
            if (Time.timeSinceLevelLoad < 4.0)
            {
                float countInTime = Time.timeSinceLevelLoad;
                CountInText.text = "Go!";
                CountInText.transform.localScale = new Vector3((countInTime - (int)countInTime), (countInTime - (int)countInTime), 1);
                controlScript.isControlActive = true;
            }
            else
            {
                CountInText.gameObject.SetActive(false);
                isCountIn = false;
                timer.isCounting = true;
                
            }
        }
        
        //Pause controls
        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

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

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseScreen.SetActive(false);
        GUICanvas.SetActive(true);
        GUICamera.SetActive(true);
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseScreen.SetActive(true);
        GUICanvas.SetActive(false);
        GUICamera.SetActive(false);
    }

}
