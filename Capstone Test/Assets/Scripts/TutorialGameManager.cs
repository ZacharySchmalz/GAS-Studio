using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialGameManager : MonoBehaviour
{

    public DestinationManager destinationManager;
    public Timer timer;
    public Text CountInText;
    public GameObject pauseScreen;
    public GameObject inputScreen;
    public GameObject tutorialScreen;
    public Text tutorialText;
    public Text gameStatusText;
    public Text waypointsText;
    public GameObject GUICanvas;
    public GameObject GUICamera;

    public bool isPaused = false;




    public delegate void GameStatus();
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
    private int tutorialNumber;

    private CarControlCS controlScript;

    private string[] comments = new string[] { "Good job!", "Great!", "You got it!" };
    private string[] tutorialTexts = {
        "Welcome to the tutorial level. \n\nYour job is to deliver packages to people around the neighborhood. Follow the arrow to each drop off point to complete your mission!",
        "If you are using a driving wheel, use the gear shift to switch between accelerating forward and reverse. If you are using a keyboard, use W for forward and S for reverse.",
        "Enter the pause menu by pressing Start <ESC> in game to see the full list of controls.",
        "The bottom left of the screen shows the three channel AI view. Press keys 1-3 (or X, Square, Circle) to see the Depth (1), Segment (2), and Standard (3) shader views which are sent to the RGB channels of the final image.",
        "Remember, the better and more consistently you drive, the better trained your AI will be. \n\nTry to follow the rules of the road!",
        "Congratulations! \n\n You've successfully delivered all of the packages. You are now ready to train and race against your own AI!"};
    private bool isTutorialOn;


    void Awake()
    {
        DestinationManager.OnGetWaypoint += UpdateWaypointUI;
        GameManager.OnWin += UpdateWinUI;
        GameManager.OnLose += UpdateLossUI;
    }

    // Use this for initialization
    void Start()
    {
        destinationManager = GameObject.FindGameObjectWithTag("Player").GetComponent<DestinationManager>();
        timer.isCounting = false;
        controlScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CarControlCS>();
        controlScript.isControlActive = false;
        StartState(0);
        isTutorialOn = false;
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
                if (initializeState)
                {
                    CountInText.text = "Go!";
                    CountInText.transform.localScale = new Vector3(2f, 2f, 1f);
                    CountInText.color = Color.green;
                    controlScript.isControlActive = true;
                    timer.isCounting = true;
                    initializeState = false;
                    SetupTutorialScreen(0);
                }
                if (tutorialNumber < 3 && !isTutorialOn)
                {
                    SetupTutorialScreen(tutorialNumber);
                }
                if (stateTime > 1.0)
                {
                    CountInText.gameObject.SetActive(false);
                }
                if (tutorialNumber == 3 && stateTime > 7.0)
                {
                    SetupTutorialScreen(tutorialNumber);
                }
                if (tutorialNumber == 4 && destinationManager.currentWaypoint == 1)
                {
                    SetupTutorialScreen(tutorialNumber);
                }



                break;
            case 3: //Waypoints Completed
                if(initializeState)
                {
                    SetupTutorialScreen(tutorialNumber);
                    initializeState = false;
                }
                if(Input.GetButtonDown("Pause"))
                {
                    GoToMainMenu(); 
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
                PauseMenu();
            }
            else
            {
                ResumeGame();
                if(isTutorialOn)
                {
                    tutorialScreen.SetActive(false);
                    isTutorialOn = false;
                }
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
        waypointsText.text = "Waypoints: " + destinationManager.currentWaypoint + "/" + destinationManager.waypoints.Length;


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

    public void PauseMenu()
    {
        PauseGame();
        pauseScreen.SetActive(true);
        GUICanvas.SetActive(false);
        GUICamera.SetActive(false);
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

    public void SetupTutorialScreen(int index)
    {
        if (tutorialTexts.Length > index)
        {
            tutorialText.text = tutorialTexts[index];
        }
        tutorialScreen.SetActive(true);
        PauseGame();
        isTutorialOn = true;
        tutorialNumber = index + 1;
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

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdatePlayers()
    {

    }
}
