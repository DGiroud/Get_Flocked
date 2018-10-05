using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

    public int maxRound;               //Max rounds per game
    public float roundLength;          //Max amount of time for the rounds

    private static int instance;       //Game Manager
    private int currentRound;          //Show current round
    private float roundTimer = 60.0f;          //Timer for current round (60 seconds)
    private int playerManager;         //Player Manager
    private int animalManager;         //Anaima Manager
    private int goalManager;           //Goal Manager
    private int obstacleManager;       //Obstacle Manager
    private int lastRound;             //Last round for the game


    //GameManager Instance
    public void Instance()
    {

    }

	// Update is called once per frame
	void Update () {
        //Decrement currentRound timer down to 0
        roundTimer -= 1;

       if (roundTimer <= 0 || currentRound != lastRound)
            {
                NewRound();
            }
      //uncomment when SceneManager is implemented
      //If it is the last round, the SceneManager will be loaded to the end game level 
      // else if (currentRound == lastRound)
      //      SceneManager.Load("endGame");
	}

    private void Awake()
    {
        //Setting currentRound to 0
        currentRound = 0;
        //Setting roundTimer to roundLenght
        roundTimer = roundLength;
    }
    #region Summary(NewRound)
    //Reloading level(s)
    //Clearing map of sheep + reload map
    //Repositioning players back to spawn locations
    //Reset time (roundTimer)
    #endregion
    private void NewRound()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GetCurrentRound()
    {

    }

    public void GetRoundTimer()
    {

    }
}
