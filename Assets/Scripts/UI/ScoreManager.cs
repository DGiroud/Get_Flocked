using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    static private PlayerScores[][] playerScores;

    public static PlayerScores[][] GetPlayerScores()
    {
        return playerScores;
    }

    /// <summary>
    /// 
    /// </summary>
	void Start ()
    {
        instance = this; // assign singleton instance

        if (LevelManager.GetCurrentRound() == 0)
        {
            int maxRounds = LevelManager.Instance.maxRounds;
            playerScores = new PlayerScores[4][];

            for (int i = 0; i < 4; i++)
            {
                playerScores[i] = new PlayerScores[maxRounds];
            }
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="score"></param>
    public void AddScore(int playerID, int score)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].score += score;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public void IncrementGoalCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfGoals++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public void IncrementKickCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfKicks++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public void IncrementInterceptCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfIntercepts++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public void IncrementSabotageCount(int playerID)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].numberOfSabotages++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="toggle"></param>
    public void ToggleSabotagedSelf(int playerID, bool toggle)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].sabotagedSelf = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="distance"></param>
    public void AddDistanceTravelled(int playerID, float distance)
    {
        playerScores[playerID][LevelManager.GetCurrentRound()].distanceTravelled += distance;
    }

    /// <summary>
    /// returns the four player scores in an int array
    /// </summary>
    /// <returns>an array of size 4 containing all player's scores</returns>
    public int[] GetScores()
    {
        int[] scores = new int[4];
        int currentRound = LevelManager.GetCurrentRound();

        for (int i = 0; i < 4; i++)
        {
            PlayerScores currentScores = playerScores[i][currentRound];

            scores[i] = currentScores.score;
        }

        return scores;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetAll()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < LevelManager.Instance.maxRounds; j++)
            {
                ResetScores(i, j);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="round"></param>
    public void ResetScores(int playerID, int round)
    {
        PlayerScores currentScores = playerScores[playerID][round];

        currentScores.score = 0;
        currentScores.numberOfGoals = 0;
        currentScores.numberOfKicks = 0;
        currentScores.numberOfIntercepts = 0;
        currentScores.numberOfSabotages = 0; 
        currentScores.sabotagedSelf = false;
        currentScores.distanceTravelled = 0; 
}
}
