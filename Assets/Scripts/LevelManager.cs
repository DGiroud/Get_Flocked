using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

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
    static private int[] scores;

    [HideInInspector]
    public bool roundStart;
    [HideInInspector]
    public float countDown;

    public int maxRounds;              //Max rounds per game
    public float roundLength;          //Max amount of time for the rounds
    static int currentRound = 0;           //Show current round
    
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
            NewRound();
    }
    
    /// <summary>
    /// count-down subroutine which pauses the game, counts down from 3,
    /// then resumes the game
    /// </summary>
    /// <returns></returns>
    IEnumerator CountDown()
    {
        // amount of time to pause for (3 seconds)
        float pauseTime = Time.realtimeSinceStartup + 4.0f;

        // pause time
        Time.timeScale = 0.0f;
        roundStart = true;

        // keep counting down until time catches up with pauseTime
        while (Time.realtimeSinceStartup < pauseTime)
        {
            // update countDown value
            countDown = pauseTime - Time.realtimeSinceStartup;
            yield return null;
        }

        // resume time
        Time.timeScale = 1.0f; 
        roundStart = false; 
    }
    
    static public int GetCurrentRound()
    { 
        //returning currentRound
        return currentRound;
    }

    #region NewRound
    /// <summary>
    ///Reloading level(s)
    ///Clearing map of sheep + reload map
    ///Repositioning players back to spawn locations
    ///Reset time (roundTimer)
    /// </summary>
    #endregion
    private void NewRound()
    {
        currentRound++;

        //If it is the last round, the SceneManager will be loaded to the end game level 
        if (currentRound == maxRounds)
        {
            currentRound = 0;
            SceneManager.LoadScene(gameOverLevelID);
        }
        else
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
