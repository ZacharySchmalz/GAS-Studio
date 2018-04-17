using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RacingGameManager : MonoBehaviour
{

    public DestinationManager destinationManager;
    public Timer timer;
    //public Text CountInText;
    //public GameObject pauseScreen;
    public GameObject inputScreen;
    //public Text gameStatusText;
    //public Text waypointsText;
    //public GameObject GUICanvas;
    //public GameObject GUICamera;

    public bool isPaused = false;


    public delegate void GameStatus();
    public static event GameStatus OnWin;
    public static event GameStatus OnLose;
    public static event GameStatus OnTimeOut;

    public GameObject[] players;
    public DestinationManager[] destinationManagers;
    public Text[] CountInTexts;
    public GameObject[] pauseScreens;
    public GameObject[] inputScreens;
    public Text[] gameStatusTexts;
    public Text[] waypointTexts;
    public GameObject[] GUICanvases;
    public GameObject[] GUICameras;
    public CarControlCS[] controlScripts;



    private float gameTime;
    public int gameState;
    private float stateTime;
    private float stateStart;
    private bool initializeState;
    private bool isGameOver;
    private bool isCountIn;

    private CarControlCS controlScript;

    


    void Awake()
    {
        DestinationManager.OnGetWaypoint += UpdateWaypointUI;
        GameManager.OnWin += UpdateWinUI;
        GameManager.OnLose += UpdateLossUI;
    }

    public void UpdatePlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            
            destinationManagers = new DestinationManager[players.Length];
            CountInTexts = new Text[players.Length];
            pauseScreens = new GameObject[players.Length];
            inputScreens = new GameObject[players.Length];
            gameStatusTexts = new Text[players.Length];
            waypointTexts = new Text[players.Length];
            GUICanvases = new GameObject[players.Length];
            GUICameras = new GameObject[players.Length];
            controlScripts = new CarControlCS[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                ComponentList playerComponents = players[i].GetComponent<ComponentList>();
                destinationManagers[i] = playerComponents.destinationManager;
                CountInTexts[i] = playerComponents.CountInText;
                pauseScreens[i] = playerComponents.pauseScreen;
                inputScreens[i] = playerComponents.inputScreen;
                gameStatusTexts[i] = playerComponents.gameStatusText;
                waypointTexts[i] = playerComponents.waypointsText;
                GUICanvases[i] = playerComponents.GUICanvas;
                GUICameras[i] = playerComponents.GUICamera;
                controlScripts[i] = players[i].GetComponent<CarControlCS>();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        UpdatePlayers();
        destinationManager = players[0].GetComponent<DestinationManager>();
        timer.isCounting = false;
        controlScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CarControlCS>();
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
                    if (players.Length > 0)
                    {
                        for (int i = 0; i < players.Length; i++)
                        {
                            pauseScreens[i].SetActive(false);
                            inputScreens[i].SetActive(true);
                        }
                    }
                            initializeState = false;
                }
                break;

            case 1: // Count Down
                if (stateTime < 3.0)
                {
                    if (players.Length > 0)
                    {
                        for (int i = 0; i < players.Length; i++)
                        {
                            CountInTexts[i].text = (3 - (int)stateTime).ToString();
                            CountInTexts[i].transform.localScale = new Vector3((stateTime - (int)stateTime), (stateTime - (int)stateTime), 1);
                        }
                    }
                }
                else
                {
                    StartState(2);
                }

                break;

            case 2: //Gameplay
                if (initializeState)
                {
                    if (players.Length > 0)
                    {
                        for (int i = 0; i < players.Length; i++)
                        {
                            CountInTexts[i].text = "Go!";
                            CountInTexts[i].transform.localScale = new Vector3(2f, 2f, 1f);
                            CountInTexts[i].color = Color.green;
                            controlScripts[i].isControlActive = true;
                            timer.isCounting = true;
                        }
                    }
                    initializeState = false;
                }



                break;
            case 3: //Waypoints Completed
                
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
                isGameOver = true;

                if (OnWin != null)
                    OnWin();
            }


        }


        //Update Waypoints Text
        if (players.Length > 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                waypointTexts[i].text = "Waypoints: " + destinationManager.currentWaypoint + "/" + destinationManager.waypoints.Length;
            }
        }


    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        if(players.Length > 0)
        {
            for(int i = 0; i < players.Length; i++)
            {
                pauseScreens[i].SetActive(false);
                GUICanvases[i].SetActive(true);
                GUICameras[i].SetActive(true);
            }
        }
        
    }

    public void PauseMenu()
    {
        PauseGame();
        if (players.Length > 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                pauseScreens[i].SetActive(true);
                GUICanvases[i].SetActive(false);
                GUICameras[i].SetActive(false);
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
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
        StartGame();
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
        //gameStatusText.text = "";
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
