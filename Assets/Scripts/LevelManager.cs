using System.Collections;
using System.Collections.Generic;
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
    public float roundTimer;
    
    public GameState gameState;
    [HideInInspector]
    public float countDown;
    [HideInInspector]
    public bool gameStart = false; //used to begin the ramspawner in RamSpawn

    public int maxRounds; //Max rounds per game
    public float roundLength; //Max amount of time for the rounds
    static int currentRound = 0; //Show current round
    [SerializeField]
    [Range(0, 1)]
    private float timesUpSlowMotion = 0.5f;
    public float timesUpPauseDuration = 2.0f;


    private void Awake()
    {
        instance = this;
        roundTimer = roundLength;

        // start count-down
        StartCoroutine("CountDown");
    }
    
    void Update()
    {
        roundTimer -= Time.deltaTime;

        if (roundTimer <= 0.5f)
        {
            if (gameState != GameState.TimesUp && gameState != GameState.RoundEnd)
            {
                StartCoroutine(TimesUp());
            }
        }
    }
    
    /// <summary>
    /// count-down subroutine which pauses the game, counts down from 3,
    /// then resumes the game
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountDown()
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
    /// 
    /// </summary>
    private void RoundEnd()
    {
        gameState = GameState.RoundEnd;

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// 
    /// </summary>
    public void NewRound()
    {
        currentRound++;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// 
    /// </summary>
    private void GameOver()
    {
        currentRound = 0;
        SceneManager.LoadScene(gameOverLevelID);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    static public int GetCurrentRound()
    { 
        //returning currentRound
        return currentRound;
    }

}
