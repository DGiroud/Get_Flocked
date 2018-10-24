using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public struct MainGamePanel
{
    // reference to panel
    public GameObject mainGamePanel;

    // panel fields
    public Text roundText;
    public Text timerText;
    public Text[] scoreTexts;
}

/// <summary>
/// 
/// </summary>
[System.Serializable]
public struct CountDownPanel
{
    // reference to panel
    public GameObject countDownPanel;

    // panel fields
    public Text countDownText;
    public string[] startTexts;
}


/// <summary>
/// 
/// </summary>
public class UIMainGame : MonoBehaviour
{
    [SerializeField]
    private MainGamePanel mainGamePanel;

    [SerializeField]
    private CountDownPanel countDownPanel;

    private bool panelToggle = true;
    private string randomStartText;

    /// <summary>
    /// updates the UI round text e.g. "Round 1", "Round 2" etc.
    /// </summary>
    void Start ()
    {
        // set up main game UI
        mainGamePanel.mainGamePanel.SetActive(false);
        mainGamePanel.roundText.text = "Round " + (LevelManager.GetCurrentRound() + 1).ToString();

        // set up count down UI
        countDownPanel.countDownPanel.SetActive(true);

        randomStartText = countDownPanel.startTexts[Random.Range(0, countDownPanel.startTexts.Length)];
    }

    /// <summary>
    /// draws the relevant UI such as the round timer and player scores
    /// </summary>
    void Update()
    {
        // if it's the start of the round...
        if (LevelManager.Instance.roundStart)
        {
            DrawCountDownText(); // draw count down
        }
        else if (panelToggle)
        {
            mainGamePanel.mainGamePanel.SetActive(true);
            countDownPanel.countDownPanel.SetActive(false);
            panelToggle = false;
        }

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
        mainGamePanel.timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    /// <summary>
    /// draws each player score in their respective corner
    /// </summary>
    private void DrawScoreText()
    {
        // get scores
        int[] scores = ScoreManager.Scores;

        // iterate over scores and print on screen
        for (int i = 0; i < scores.Length; i++)
            mainGamePanel.scoreTexts[i].text = scores[i].ToString();
    }

    /// <summary>
    /// draws the count down timer which plays at the start of each round
    /// </summary>
    private void DrawCountDownText()
    {
        // get seconds
        int seconds = (int)(LevelManager.Instance.countDown % 60);
        
        // print "3... 2... 1... GO!"
        if (seconds >= 1)
            countDownPanel.countDownText.text = seconds.ToString(); // print on screen
        else
            countDownPanel.countDownText.text = randomStartText;
    }
}
