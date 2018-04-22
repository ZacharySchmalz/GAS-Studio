using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkRacingGameManager : MonoBehaviour
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
    public GameObject lobbyScreen;
    public Text playerList;
    public Text gameStatusText;
    public Text waypointText;
    public GameObject GUICanvas;
    public GameObject GUICamera;
    public CarControlCS controlScript;

    [Header("Networked Players")]
    public GameObject[] players;
    public DestinationManager[] destinationManagers;




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
        
        players = GameObject.FindGameObjectsWithTag("NetworkPlayer");
        if (players.Length > 0)
        {
            
            destinationManagers = new DestinationManager[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                ComponentList playersComponents = players[i].GetComponent<ComponentList>();
                destinationManagers[i] = playersComponents.destinationManager;
            }
        }
    }

    public void InitializePlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        ComponentList playerComponents = player.GetComponent<ComponentList>();
        destinationManager = playerComponents.destinationManager;
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
        StartState(0);
        ResetUI();
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

            case 1: //Lobby
                if (initializeState)
                {
                    pauseScreen.SetActive(false);
                    inputScreen.SetActive(false);
                    GUICanvas.SetActive(false);
                    GUICamera.SetActive(false);
                    lobbyScreen.SetActive(true);
                    initializeState = false;
                }

                string playerListText = "Player 1 (you)";
                if(players.Length > 0)
                {
                    for(int i = 0; i < players.Length; i++)
                    {
                        playerListText += "\nPlayer " + (i + 2);
                    }
                }
                playerList.text = playerListText;


                break;

            case 2: // Count Down
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
                    StartState(3);
                }

                break;



            case 3: //Gameplay
                if (initializeState)
                {
                    CountInText.text = "Go!";
                    CountInText.transform.localScale = new Vector3(2f, 2f, 1f);
                    CountInText.color = Color.green;
                    controlScript.isControlActive = true;
                    timer.isCounting = true;
                    initializeState = false;
                }
                if(stateTime > 1.0)
                {
                    CountInText.text = "";
                }


                break;
            case 4: //Game Over
                
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
                    StartState(4);
                }
                Debug.Log("Got all the waypoints");
                gameStatusText.text = "You Win!";
                isGameOver = true;

                if (OnWin != null)
                    OnWin();
            }
            if (players.Length > 0)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (destinationManagers[i].currentWaypoint >= destinationManagers[i].waypoints.Length)
                    {
                        if (isGameOver == false)
                        {
                            StartState(4);
                        }
                        gameStatusText.text = "You Lose!";
                        isGameOver = true;
                    }
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
        lobbyScreen.SetActive(false);
        inputScreen.SetActive(false);
        GUICanvas.SetActive(true);
        GUICamera.SetActive(true);

    }

    public void PauseMenu()
    {
        PauseGame();
        pauseScreen.SetActive(true);
        lobbyScreen.SetActive(false);
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
        StartState(2);
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
