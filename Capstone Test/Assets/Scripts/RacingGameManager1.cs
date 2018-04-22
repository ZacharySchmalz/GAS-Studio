using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RacingGameManager1 : MonoBehaviour
{



    public bool isPaused = false;


    public delegate void GameStatus();
    public static event GameStatus OnWin;
    public static event GameStatus OnLose;
    public static event GameStatus OnTimeOut;

    [Header("Player")]
    public GameObject player;
    public DestinationManager destinationManager;
    public Timer timer;
    public Text CountInText;
    public GameObject pauseScreen;
    public GameObject inputScreen;
    public Text gameStatusText;
    public Text waypointText;
    public GameObject GUICanvas;
    public GameObject GUICamera;
    public CarControlCS controlScript;

    [Header("AI")]
    public GameObject AI;
    public DestinationManager AIdestinationManager;
    public CarControlCS AIcontrolScript;




    private float gameTime;
    public int gameState;
    private float stateTime;
    private float stateStart;
    private bool initializeState;
    private bool isGameOver;
    private bool isCountIn;

    


    void Awake()
    {
        DestinationManager.OnGetWaypoint += UpdateWaypointUI;
        GameManager.OnWin += UpdateWinUI;
        GameManager.OnLose += UpdateLossUI;
    }

    public void UpdatePlayers()
    { 
		if (GameObject.FindGameObjectWithTag ("AIPlayer") != null) {
			AI = GameObject.FindGameObjectWithTag ("AIPlayer");
			AIdestinationManager = AI.GetComponent<DestinationManager> ();
			AIcontrolScript = AI.GetComponent<CarControlCS> ();
		}
    }

    public void InitializePlayer()
    {

            player = GameObject.FindGameObjectWithTag("Player");
            ComponentList playerComponents = player.GetComponent<ComponentList>();
            destinationManager = playerComponents.destinationManager;
            CountInText = playerComponents.CountInText;
            pauseScreen = playerComponents.pauseScreen;
            inputScreen = playerComponents.inputScreen;
            gameStatusText = playerComponents.gameStatusText;
            waypointText = playerComponents.waypointsText;
            GUICanvas = playerComponents.GUICanvas;
            GUICamera = playerComponents.GUICamera;
            controlScript = playerComponents.carControl;
            timer = playerComponents.timer;
    }

    // Use this for initialization
    void Start()
    {
        InitializePlayer();
        UpdatePlayers();
        timer.isCounting = false;
        controlScript.isControlActive = false;
		if(AIcontrolScript != null) 
        	AIcontrolScript.isControlActive = false;
        StartState(0);
        ResetUI();
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Update gameTime
        gameTime = Time.timeSinceLevelLoad;
        stateTime = gameTime - stateStart;
        switch (gameState)
        {
            case 0: // Select Input Type
                if (initializeState)
                {
                    PauseMenu();
                    pauseScreen.SetActive(false);
                    inputScreen.SetActive(true);
                    initializeState = false;
                }
                break;

           

            case 1: // Count Down
                if (initializeState)
                {
                    ResumeGame();
                    isCountIn = true;
                    initializeState = false;
                }
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
                if (initializeState)
                {
                    CountInText.text = "Go!";
                    CountInText.transform.localScale = new Vector3(2f, 2f, 1f);
                    CountInText.color = Color.green;
                    controlScript.isControlActive = true;
				if(AIcontrolScript != null)
                    AIcontrolScript.isControlActive = true;
                    timer.isCounting = true;
                    initializeState = false;
                }
                if(stateTime > 1.0)
                {
                    CountInText.text = "";
                }


                break;
            case 3: //Game Over
                
                break;
            default:
                break;
        }

        //Pause controls
        if (Input.GetButtonDown("Pause") && gameState > 1)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                PauseMenu();
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
                Debug.Log("Timed out");

                isGameOver = true;

                if (OnLose != null)
                    OnLose();
            }
            else if (destinationManager.currentWaypoint >= destinationManager.waypoints.Length)
            {
                if(isGameOver == false)
                {
                    StartState(3);
                }
                Debug.Log("Got all the waypoints");
                gameStatusText.text = "You Win!";
                isGameOver = true;

                if (OnWin != null)
                    OnWin();
            }

			if (AIdestinationManager != null) {
				if (AIdestinationManager.currentWaypoint >= AIdestinationManager.waypoints.Length) {
					if (isGameOver == false) {
						StartState (3);
					}
					gameStatusText.text = "You Lose!";
					isGameOver = true;
				}
			}


        }


        //Update Waypoints Text

        waypointText.text = "Waypoints: " + destinationManager.currentWaypoint + "/" + destinationManager.waypoints.Length;


    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseScreen.SetActive(false);
        inputScreen.SetActive(false);
        GUICanvas.SetActive(true);
        GUICamera.SetActive(true);

    }

    public void PauseMenu()
    {
        PauseGame();
        pauseScreen.SetActive(true);
        inputScreen.SetActive(false);
        GUICanvas.SetActive(false);
        GUICamera.SetActive(false);

    }

    public void PauseGame()
    {
        isPaused = true;
    }

    public void StartRace()
    {
        StartState(1);
    }

    public void StartState(int state)
    {
        stateStart = gameTime;
        gameState = state;
        initializeState = true;
    }

    /*public void SetInputMethod(int method)
    {
        switch (method) //0 = Wheel, 1 = Keyboard, 2 = AI
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
    }*/

    public void InputSelected()
    {
        inputScreen.SetActive(false);
        StartState(1);
    }


    //GameManagerUI
    public void UpdateWinUI()
    {
        //gameStatusText.text = "Great job! Delivery completed.";
    }

    public void UpdateLossUI()
    {
        //gameStatusText.text = "You ran out of time.\nBetter luck next time!";
    }

    // ui for when you get a waypoint 
    public void UpdateWaypointUI()
    {
        //Update
    }

    public void ResetUI()
    {
        gameStatusText.text = "";
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
