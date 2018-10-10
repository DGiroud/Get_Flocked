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

    [SerializeField]
    private Text[] scoreTexts;

    private int[] scores;

	// Use this for initialization
	void Start ()
    {
        instance = this;

        scores = new int[PlayerManager.Instance.players.Count];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = 0;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		for (int i = 0; i < scores.Length; i++)
        {
            scoreTexts[i].text = scores[i].ToString();
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
