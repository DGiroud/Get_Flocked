using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

    // singleton instance
    #region singleton

    private static LevelManager instance;

    /// <summary>
    /// getter for singleton instance of LevelManager
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    public int maxRounds;               //Max rounds per game
    public float roundLength;          //Max amount of time for the rounds

    private int currentRound;          //Show current round
    private float roundTimer = 0f;     //Timer for current round
    private float startingTime = 10.0f;

    void Start()
    {
        roundTimer = roundLength;
    }

    // Update is called once per frame
    void Update () {

        roundTimer -= 1 * Time.deltaTime;
        print(roundTimer);

        //Decrement currentRound timer down to 0
        roundTimer -= Time.deltaTime;
        if (roundTimer <= 0 || currentRound != maxRounds)
        {
            NewRound();
        }

        //If it is the last round, the SceneManager will be loaded to the end game level 
        else if (currentRound == maxRounds)
            SceneManager.LoadScene("endGame");
    }

    private void Awake()
    {
        instance = this;
        //text = GetComponent<Text>();

        //Setting currentRound to 0
       // currentRound = 0;
        //Setting roundTimer to roundLenght
       // roundTimer = roundLength;
    }
    #region Summary(NewRound)
    //Reloading level(s)
    //Clearing map of sheep + reload map
    //Repositioning players back to spawn locations
    //Reset time (roundTimer)
    #endregion
    public void NewRound()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


}
