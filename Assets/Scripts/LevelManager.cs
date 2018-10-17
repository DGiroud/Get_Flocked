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

    public int maxRounds;               //Max rounds per game
    public float roundLength;          //Max amount of time for the rounds

    static int currentRound = 0;          //Show current round
    static public int GetCurrentRound()
    { 
        return currentRound;
    }

    // Update is called once per frame
    void Update() {

        //If it is the last round, the SceneManager will be loaded to the end game level 
        if (currentRound == maxRounds)
        {
            SceneManager.LoadScene(gameOver.name);
        }
    }

    private void Awake ()
    {
        instance = this;
        currentRound = 0;
    }

    #region Summary(NewRound)
    //Reloading level(s)
    //Clearing map of sheep + reload map
    //Repositioning players back to spawn locations
    //Reset time (roundTimer)
    #endregion
    public void NewRound()
    {
        currentRound++;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
