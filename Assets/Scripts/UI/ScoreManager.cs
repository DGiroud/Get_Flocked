using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    private int[] scores;
    public int[] Scores { get { return scores; } }

    /// <summary>
    /// 
    /// </summary>
	void Start ()
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
}
