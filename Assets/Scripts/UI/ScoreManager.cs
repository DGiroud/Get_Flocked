using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

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

    static private int[] scores;
    static public int[] Scores { get { return scores; } }
    static private Sheep totalNumberOfSheepClaimed;                     //total sheep scored into goals
    static private PlayerProgress progress;                            //player progress for scores

    /// <summary>
    /// 
    /// </summary>
	void Start()
    {
        instance = this; // assign singleton instance

        scores = new int[PlayerManager.Instance.players.Count];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = 0;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="score"></param>
    public void AddScore(int playerID, int score)
    {
        scores[playerID] += score;
    }
    public class PlayerProgress
    {
        public int highScore = 0;    
    }
    public int GetHighScoringPlayer()
    {
        return progress.highScore;
    }

    public void NewPlayerScore(int scores)
    {
        //score is greater than the progress, updating the progress with whatever the new
        //value is and then saves the progress
        if (scores > progress.highScore)
        {
            progress.highScore = scores;
            saveProgress();
        }

        //score is greater than the totalNumberOfSheepClaimed, updating the progress with whatever the new
        //value is and then saves the totalNumberOfSheepClaimed
        if (scores > totalNumberOfSheepClaimed.score)
        {
            totalNumberOfSheepClaimed.score = scores;
            saveProgress();
        }
    }
    
    private void saveProgress()
    {
        //saves the value of the progress.highScore to PlayerPrefs
        //with the key being "HighScore"
        PlayerPrefs.SetInt("HighScore", progress.highScore);
    }


}
