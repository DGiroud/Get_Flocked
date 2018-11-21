using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    CountDown,
    Main,
    Pause,
    TimesUp,
    RoundEnd
}

public class LevelManager : MonoBehaviour
{
    // singleton instance
    #region singleton

    private static LevelManager instance;

    /// <summary>
    /// getting for singleton instance of LevelManager
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    [SerializeField]
    private int gameOverLevelID;
    
    [HideInInspector]
    public GameState gameState;
    [HideInInspector]
    public float countDown;
    [HideInInspector]
    public bool gameStart = false;            //used to begin the ramspawner in RamSpawn
    [Tooltip("Set your max rounds!")]
    public int maxRounds;                     //Max rounds per game
    static int currentRound = 0;              //Show current round
    [SerializeField]
    [Range(0, 1)]
    private float timesUpSlowMotion = 0.5f;
    public float timesUpPauseDuration = 2.0f;

    [HideInInspector]
    public static bool gameIsPaused = false;  //will always be false unless toggled for pause menu ingame
    public GameObject pauseMenuUI;            //pause
    private GameObject resumeButton;          //resume

    private void Awake()
    {
        instance = this;

        // start count-down
        StartCoroutine(CountDown());
    }
    
    void Update()
    {
        //keyboard pause
        KeyboardPause();
        //controller pause
        ControllerPause();

        //scoremanager
        if (ScoreManager.Instance.CurrentSheep == ScoreManager.Instance.RequiredSheep)
        {
            if (gameState != GameState.TimesUp && gameState != GameState.RoundEnd)
            {
                StartCoroutine(TimesUp());
            }
        }
    }
    #region PAUSE
   
    /// <summary>
    /// if "Start" button is pressed
    /// </summary>
   private void ControllerPause()
   {
        //joystick button 7 = start button
       if (Input.GetKeyDown("joystick button 7"))
       {
           if (gameIsPaused)
           {
               //calls resume function
               Resume();
           }
           else
           {
               //calls pause function 
               Pause();
           }
       }
   }
    /// <summary>
    /// if "Escape" is pressed
    /// </summary>
    private void KeyboardPause()
    {
        //if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                //calls resume function
                Resume();
            }
            else
            {
                //calls pause function
                Pause();
            }
        }
    }

    /// <summary>
    /// Resumes game
    /// </summary>
    public void Resume()
    {
        //disables the pause menu
        gameState = GameState.Main;

        pauseMenuUI.SetActive(false);
        //resumes back to fulltime
        Time.timeScale = 1.0f;
        gameIsPaused = false;

        //starts countdown after resuming 3..2..1..
        StartCoroutine(CountDown());
        
    }
    /// <summary>
    /// pauses game
    /// </summary>
    public void Pause()
    {
        gameState = GameState.Pause;
        //enabling/disbaling gameobject
        pauseMenuUI.SetActive(true);
        //freezing the game speed 
        Time.timeScale = 0.0f;
        //enabling pause menu
        gameIsPaused = true;
    }
    /// <summary>
    /// loads and goes to start menu
    /// </summary>
    public void LoadMenu()
    {
        //setting the time scale back to its original value
        Time.timeScale = 1.0f;
        //loads the start menu scene
        SceneManager.LoadScene("StartMenu");
    }
    #endregion

    /// <summary>
    /// count-down subroutine which pauses the game, counts down from 3,
    /// then resumes the game
    /// </summary>
    /// <returns></returns>
    public IEnumerator CountDown()
    {
        gameState = GameState.CountDown;

        // amount of time to pause for (3 seconds)
        float pauseTime = Time.realtimeSinceStartup + 3.5f;

        // pause time
        Time.timeScale = 0.0f;

        // keep counting down until time catches up with pauseTime
        while (Time.realtimeSinceStartup < pauseTime)
        {
            // update countDown value
            countDown = pauseTime - Time.realtimeSinceStartup;
            yield return null;
        }

        // resume time
        Time.timeScale = 1.0f; 
        gameStart = true;

        gameState = GameState.Main;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimesUp()
    {
        gameState = GameState.TimesUp;
        ScoreManager.Instance.SetRoundPlacements();

        float pauseTime = Time.realtimeSinceStartup + timesUpPauseDuration;

        // pause time
        Time.timeScale = timesUpSlowMotion;
        
        while (Time.realtimeSinceStartup < pauseTime)
        {
            yield return null;
        }

        if (currentRound == maxRounds - 1)
            GameOver();
        else
            RoundEnd();
    }

    /// <summary>
    /// round ended
    /// </summary>
    private void RoundEnd()
    {
        //setting the game state to round end
        gameState = GameState.RoundEnd;
        //setting the time scale to 0 when the round has ended
        Time.timeScale = 0.0f;

    }

    /// <summary>
    /// New Round
    /// </summary>
    public void NewRound()
    {
        currentRound++;
        //gets the next scene and loads it for the next round 
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Game Over function
    /// </summary>
    private void GameOver()
    {
        //if the current round is at 0 then it will load 
        //the gameOverLevelID scene
        currentRound = 0;
        SceneManager.LoadScene(gameOverLevelID);
    }
    
    /// <summary>
    /// getting the current round
    /// </summary>
    /// <returns></returns>
    static public int GetCurrentRound()
    {
        //returning currentRound
        return currentRound;
    }

}
