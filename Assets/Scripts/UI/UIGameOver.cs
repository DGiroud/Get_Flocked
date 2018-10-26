using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField]
    private int mainMenuLevelID;
    [SerializeField]
    private int gameLevelID;

    [SerializeField]
    private Text[] scoreText;
    [SerializeField]
    private Text[] goalsText;
    [SerializeField]
    private Text[] kicksText;
    [SerializeField]
    private Text[] interceptTexts;
    [SerializeField]
    private Text[] distanceTexts;

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        PlayerScores[][] playerScores = ScoreManager.GetPlayerScores();

        for (int i = 0; i < 4; i++)
        {
            PlayerScores[] currentPlayerScores = playerScores[i];
            int score = 0, goals = 0, kicks = 0, intercepts = 0;
            float distance = 0;

            for (int j = 0; j < currentPlayerScores.Length; j++)
            {
                PlayerScores playerRoundScores = currentPlayerScores[j];

                score += playerRoundScores.score;
                goals += playerRoundScores.numberOfGoals;
                kicks += playerRoundScores.numberOfKicks;
                intercepts += playerRoundScores.numberOfIntercepts;
                distance += playerRoundScores.distanceTravelled;
            }

            scoreText[i].text = score.ToString();
            goalsText[i].text = goals.ToString();
            kicksText[i].text = kicks.ToString();
            interceptTexts[i].text = intercepts.ToString();
            distanceTexts[i].text = distance.ToString("0") + " m";
        }
    }

    /// <summary>
    /// loads the main game
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene(gameLevelID);
    }

    /// <summary>
    /// loads the main menu
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuLevelID);
    }

    /// <summary>
    /// forces application quit
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
