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
    public GameObject inputScreen;
    public Text gameStatusText;
    public GameObject GUICanvas;
    public GameObject GUICamera;

    public bool isPaused = false;


    public delegate void GameStatus ();
	public static event GameStatus OnWin;
	public static event GameStatus OnLose;
	public static event GameStatus OnTimeOut;

    private float gameTime;
    private int gameState;
    private float stateTime;
    private float stateStart;
    private bool initializeState;
	private bool isGameOver;
    private bool isCountIn;
    private CarControlCS controlScript;

    private string[] comments = new string[] { "Good job!", "Great!", "You got it!" };

    void Awake()
    {
        DestinationManager.OnGetWaypoint += UpdateWaypointUI;
        GameManager.OnWin += UpdateWinUI;
        GameManager.OnLose += UpdateLossUI;
    }

    // Use this for initialization
    void Start () 
	{
		destinationManager = GetComponent<DestinationManager> ();	
		gameManagerUI = GetComponent<GameManagerUI> ();
        timer.isCounting = false;
        controlScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CarControlCS>();
        controlScript.isControlActive = false;
        StartState(0);
        ResetUI();
    }
	
	// Update is called once per frame
	void Update () 
	{
        //Update gameTime
        gameTime = Time.timeSinceLevelLoad;
        stateTime = gameTime - stateStart;
        switch(gameState)
        {
            case 0: // Select Input Type
                if(initializeState)
                {
                    PauseGame();
                    pauseScreen.SetActive(false);
                    inputScreen.SetActive(true);
                    initializeState = false;
                }
                break;

            case 1: // Count Down
                if (stateTime < 3.0)
                {
                    CountInText.text = (3 - (int)stateTime).ToString();
                    CountInText.transform.localScale = new Vector3((stateTime - (int)stateTime), (stateTime - (int)stateTime), 1);
                }
                else
                {
                    StartState(2);
                }

                break;

            case 2: //Gameplay
                if(initializeState)
                {
                    CountInText.text = "Go!";
                    CountInText.transform.localScale = new Vector3(2f,2f,1f);
                    CountInText.color = Color.green;
                    controlScript.isControlActive = true;
                    timer.isCounting = true;
                }
                if(stateTime > 1.0)
                {
                    CountInText.gameObject.SetActive(false);
                }
                break;

            default:
                break;
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

    public void StartGame()
    {
        ResumeGame();
        StartState(1);
        isCountIn = true;    
    }

    public void StartState(int state)
    {
        stateStart = gameTime;
        gameState = state;
        initializeState = true;
    }

    public void SetInputMethod(int method)
    {
        switch(method) //0 = Wheel, 1 = Keyboard, 2 = AI
        {
            case 0: //Wheel
                controlScript.isKeyboard = false;
                controlScript.isAI = false;
                break;
            case 1: //Keyboard
                controlScript.isKeyboard = true;
                controlScript.isAI = false;
                break;
            case 2://AI
                controlScript.isKeyboard = false;
                controlScript.isAI = true;
                break;
            default:
                break;
        }
        inputScreen.SetActive(false);
    }

    //GameManagerUI
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
        gameStatusText.text = comments[Random.Range(0, comments.Length)];
    }

    public void ResetUI()
    {
        gameStatusText.text = "";
    }
}
