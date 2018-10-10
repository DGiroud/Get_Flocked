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
    private Text roundText;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Text[] scoreTexts;

    private float roundTimer;
    static private int[] scores;

	// Use this for initialization
	void Start ()
    {
        instance = this; // assign singleton instance

        roundText.text = "Round " + (LevelManager.GetCurrentRound() + 1).ToString();
        roundTimer = LevelManager.Instance.roundLength;
        scores = new int[PlayerManager.Instance.players.Count];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = 0;
        }
	}
	
    /// <summary>
    /// 
    /// </summary>
	void Update ()
    {
        roundTimer -= Time.deltaTime;

        if (roundTimer <= 0.5f)
            LevelManager.Instance.NewRound();

        UpdateTimer();
        UpdateScores();
	}

    private void UpdateTimer()
    {
        int minutes = (int)Mathf.Floor(roundTimer / 60);
        int seconds = (int)(roundTimer % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }


    private void UpdateScores()
    {
        for (int i = 0; i < scores.Length; i++)
            scoreTexts[i].text = scores[i].ToString();
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
