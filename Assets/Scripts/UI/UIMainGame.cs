using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainGame : MonoBehaviour
{
    [SerializeField]
    private Text roundText;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Text[] scoreTexts;

    /// <summary>
    /// updates the UI round text e.g. "Round 1", "Round 2" etc.
    /// </summary>
    void Start ()
    {
        roundText.text = "Round " + (LevelManager.GetCurrentRound() + 1).ToString();
    }

    /// <summary>
    /// draws the relevant UI such as the round timer and player scores
    /// </summary>
    void Update()
    {
        DrawTimerText();
        DrawScoreText();
    }

    /// <summary>
    /// draws the round timer in the format of "00:00"
    /// </summary>
    private void DrawTimerText()
    {
        // get current round time
        float roundTimer = LevelManager.Instance.roundTimer;

        // convert to minutes and seconds
        int minutes = (int)Mathf.Floor(roundTimer / 60);
        int seconds = (int)(roundTimer % 60);

        // print on screen in the format of "00:00"
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    /// <summary>
    /// draws each player score in their respective corner
    /// </summary>
    private void DrawScoreText()
    {
        // get scores
        int[] scores = ScoreManager.Instance.Scores;

        // iterate over scores and print on screen
        for (int i = 0; i < scores.Length; i++)
            scoreTexts[i].text = scores[i].ToString();
    }
}
