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
    private Object gameOver;
    public float roundTimer;
    static private int[] scores;

    public bool roundStart;
    public float countDown;

    public int maxRounds;              //Max rounds per game
    public float roundLength;          //Max amount of time for the rounds
    static int currentRound;           //Show current round
    
    private void Awake()
    {
        instance = this;
        currentRound = 0;
        roundTimer = roundLength;

        StartCountDown();
    }
    
    void Update()
    {
        if (roundStart)
        {
            CountDown();
            return;
        }

        roundTimer -= Time.deltaTime;

        if (roundTimer <= 0.5f)
            NewRound();
    }
    
    public void StartCountDown()
    {
        Time.timeScale = 0.0f;
        roundStart = true;
        countDown = 5;
    }
    
    public void CountDown()
    {
        countDown -= Time.unscaledDeltaTime;

        Debug.Log(countDown);

        if (countDown <= 0.0f)
            StopCountDown();
    }
    
    public void StopCountDown()
    {
        Time.timeScale = 1.0f;
        roundStart = false;
        countDown = 3.0f;
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
    public void NewRound()
    {
        //If it is the last round, the SceneManager will be loaded to the end game level 
        if (currentRound == maxRounds)
        {
            SceneManager.LoadScene(gameOver.name);
        }

        currentRound++;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }



}
