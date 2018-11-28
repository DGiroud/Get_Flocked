using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// a container of relevant player scores
/// </summary>
[System.Serializable]
public struct PlayerScores
{
    // main stats
    public int place; // 1st, 2nd, 3rd or 4th
    public int score; // number of points gained
    
    // bonus stats
    public int numberOfGoals; // amount of sheep scored
    public int numberOfKicks; // amount of sheep kicked
    public int numberOfIntercepts; // amount of sheep kicked off other people
    public int numberOfSabotages; // amount of black sheep scored in enemy goals
    public int numberOfGoldSheep; // amount of gold sheep scored
    public bool sabotagedSelf; // true if a player scores a black sheep on themself
    public float distanceTravelled; // total distance travelled during the round
}

public class ScoreManager : MonoBehaviour
{
    // singleton instance
    #region singleton

    private static ScoreManager instance;

    /// <summary>
    /// getter for singleton instance of ScoreManager
    /// </summary>
    public static ScoreManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    private int currentSheep = 0;
    public int CurrentSheep { get { return currentSheep; } }
    [SerializeField]
    private int requiredSheep = 20;
    public int RequiredSheep { get { return requiredSheep; } }

    // 2D playerScores array. First dimension is players, second is rounds
    static private PlayerScores[][] playerScores;
    static public PlayerScores[][] GetPlayerScores()
    {
        return playerScores;
    }

    /// <summary>
    /// initialisation of the 2D array of PlayerScores
    /// </summary>
	void Start ()
    {
        instance = this; // assign singleton instance

        // only do initialisation on round 1
        if (LevelManager.GetCurrentRound() == 0)
        {
            // set max rounds
            int maxRounds = LevelManager.Instance.maxRounds;

            // initialise player scores
            playerScores = new PlayerScores[4][];

            // iterate over the initialised player scores
            for (int i = 0; i < playerScores.Length; i++)
            {
                // initialise round scores
                playerScores[i] = new PlayerScores[maxRounds];
            }
        }
	}

    /// <summary>
    /// helper function which adds one to the total sheep scored
    /// </summary>
    public void IncrementTotalSheep()
    {
        currentSheep++;
    }

    /// <summary>
    /// helper function which adds score to a particular player
    /// </summary>
    /// <param name="playerID">the index of the player to reward</param>
    /// <param name="score">the amount of points to reward</param>
    public void AddScore(int playerID, int score)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].score += score;
    }

    /// <summary>
    /// helper function which increments a particular player's goal count by one
    /// </summary>
    /// <param name="playerID">the index of the player to reward</param>
    public void IncrementGoldCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfGoldSheep++;
    }

    /// <summary>
    /// helper function which increments a particular player's goal count by one
    /// </summary>
    /// <param name="playerID">the index of the player to reward</param>
    public void IncrementGoalCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfGoals++;
    }

    /// <summary>
    /// helper function which increments a particular player's kick count by one
    /// </summary>
    /// <param name="playerID">the index of the player to reward</param>
    public void IncrementKickCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfKicks++;
    }

    /// <summary>
    /// helper function which increments a particular player's intercept count by one
    /// </summary>
    /// <param name="playerID">the index of the player to reward</param>
    public void IncrementInterceptCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfIntercepts++;
    }

    /// <summary>
    /// helper function which increments a particular player's sabotage count by one
    /// </summary>
    /// <param name="playerID">the index of the player to reward</param>
    public void IncrementSabotageCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfSabotages++;
    }

    /// <summary>
    /// helper function which is called when a player sabotages themselves
    /// </summary>
    /// <param name="playerID">the index of the player to reward</param>
    public void ToggleSabotagedSelf(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].sabotagedSelf = true;
    }

    /// <summary>
    /// helper function which increments a particular player's distance travelled
    /// </summary>
    /// <param name="playerID">the index of the player to reward</param>
    /// <param name="distance">the distance travelled</param>
    public void AddDistanceTravelled(int playerID, float distance)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].distanceTravelled += distance;
    }

    /// <summary>
    /// function to be called after each round, which iterates through all
    /// players and determines their placement for the round (e.g. 1st, 3rd, etc.)
    /// </summary>
    public void SetRoundPlacements()
    {
        int currentRound = LevelManager.GetCurrentRound();

        // iterate over all players
        for (int i = 0; i < playerScores.Length; i++)
        {
            // get this particular player's score for THIS round
            PlayerScores playerRoundScores = playerScores[i][currentRound];
            int roundPlacement = 1; // 1st place by default

            // iterate over all the other players
            for (int j = 0; j < playerScores.Length; j++)
            {
                if (i == j)
                    continue; // ignore self

                // get the other player's score for this round
                int otherPlayerScore = playerScores[j][currentRound].score;

                // if the current player has a lower score than the other player
                if (playerRoundScores.score < otherPlayerScore)
                    roundPlacement++; // higher round placement
            }

            // set round placement for each player
            playerScores[i][currentRound].place = roundPlacement;
        }
    }

    /// <summary>
    /// helper function which returns the four player scores in an int array
    /// </summary>
    /// <returns>an array of size 4 containing all player's scores</returns>
    public int[] GetScores()
    {
        // initialise output array
        int[] scores = new int[4];
        int currentRound = LevelManager.GetCurrentRound();

        // iterate over all players
        for (int i = 0; i < playerScores.Length; i++)
        {
            // get their score for THIS round
            PlayerScores currentScores = playerScores[i][currentRound];

            // add it to output
            scores[i] = currentScores.score;
        }

        return scores;
    }
    
    /// <summary>
    /// helper function which iterates over players, evaluates each one's score
    /// and returns the actor ID (index) of the leading player
    /// </summary>
    /// <param name="actor">optional argument, pass in an actor that you want to ignore</param>
    /// <returns>the ID (index) of the highest scoring player</returns>
    public int GetHighestScoringPlayer(GameObject actor = null)
    {
        float highestScore = Mathf.NegativeInfinity;
        int relevantPlayerID = Random.Range(0, 3);

        // iterate over all possible scores
        for (int i = 0; i < playerScores.Length; i++)
        {
            // ignore self if required
            if (i == actor.GetComponent<BaseActor>().actorID)
                continue;

            // get the rounds scores of a particular player
            PlayerScores[] currentPlayerScores = playerScores[i];
            int currentPlayerScore = currentPlayerScores[LevelManager.GetCurrentRound()].score;

            // if a higher score is found
            if (currentPlayerScore > highestScore)
            {
                highestScore = currentPlayerScore; // update highest score
                relevantPlayerID = i; // update corresponding player ID
            }
        }

        // return the ID of the highest scoring player
        return relevantPlayerID;
    }
}
