using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    // scene transition variables
    [SerializeField]
    private int mainMenuLevelID;
    [SerializeField]
    private int gameLevelID;

    [SerializeField]
    private GameObject[] playerPrefabs;
    private int winnerID;
    [SerializeField]
    private Vector2 cheerSpacing;
    private float cheerTimer = 0.0f;

    // all end game UI elements
    [SerializeField]
    private Text playerWinText;
    [SerializeField]
    private Text[] roundTexts;
    [SerializeField]
    private Text[] scoreTexts;
    [SerializeField]
    private Text[] goldTexts;
    [SerializeField]
    private Text[] goalsTexts;
    [SerializeField]
    private Text[] kicksTexts;
    [SerializeField]
    private Text[] interceptTexts;
    [SerializeField]
    private Text[] distanceTexts;
    [SerializeField]
    private Text[] finalScoreTexts;

    // player score variables
    private PlayerScores[][] playerScores;
    private int[] finalScores;

    // the IDs of the highest scoring player in each field
    private int highestScoringPlayer;
    private int mostGoalsPlayer;
    private int mostKicksPlayer;
    private int mostInterceptsPlayer;
    private int highestDistancePlayer;

    /// <summary>
    /// does all of the point and score calculation, and prints everything to the screen
    /// </summary>
    private void Start()
    {
        BGMManager.Instance.bgmPhase = BGMPhase.GameOver;

        // get player scores from score manager
        playerScores = ScoreManager.GetPlayerScores();

        // initialise final score array
        finalScores = new int[4];

        // print/set the text values of all fields (points, gold sheep, goals, etc.)
        PrintAllTextFields();

        // delegate points to each player depending on their performance in each field
        DistributePoints();

        // print all of the final scores on the screen
        UpdateFinalScoreTexts();

        // determine the winner and print it in the form of "Player _ WINS!"
        winnerID = EvaluateWinner();

        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            Animator actorAnimator = playerPrefabs[i].GetComponent<Animator>();

            if (i == winnerID)
            {
                int randomWinAnimation = Random.Range(0, 7);
                actorAnimator.SetInteger("winValue", randomWinAnimation);
            }
            else
            {
                int randomLoseAnimation = Random.Range(0, 7);
                actorAnimator.SetInteger("loseValue", randomLoseAnimation);
            }
        }
    }

    /// <summary>
    /// iterate over all players and rounds. Add up their scores and print/set the text 
    /// values of all fields (points, gold sheep, goals, etc.)
    /// </summary>
    private void PrintAllTextFields()
    {
        // iterate over all players
        for (int i = 0; i < playerScores.Length; i++)
        {
            // define count variables
            int score = 0, gold = 0, goals = 0, kicks = 0, intercepts = 0;
            float distance = 0;
            string roundPlacementText = "";

            // iterate over each round
            for (int j = 0; j < playerScores[i].Length; j++)
            {
                PlayerScores playerRoundScores = playerScores[i][j];
                int roundPlacement = playerRoundScores.place;
                roundPlacementText += roundPlacement.ToString();

                // format round placement in the form of 1st, 2nd, 3rd etc.
                switch (roundPlacement)
                {
                    case 1:
                        roundPlacementText += "st";
                        break;
                    case 2:
                        roundPlacementText += "nd";
                        break;
                    case 3:
                        roundPlacementText += "rd";
                        break;
                    case 4:
                        roundPlacementText += "th";
                        break;
                }

                // disallow comma on last round, e.g. 1st, 4th, 2nd
                if (j < playerScores[i].Length - 1)
                    roundPlacementText += ", ";

                // increment count variables
                score += playerRoundScores.score;
                gold += playerRoundScores.numberOfGoldSheep;
                goals += playerRoundScores.numberOfGoals;
                kicks += playerRoundScores.numberOfKicks;
                intercepts += playerRoundScores.numberOfIntercepts;
                distance += playerRoundScores.distanceTravelled;
            }

            // print counted variables
            roundTexts[i].text = roundPlacementText;
            scoreTexts[i].text = score.ToString();
            goldTexts[i].text = gold.ToString();
            goalsTexts[i].text = goals.ToString();
            kicksTexts[i].text = kicks.ToString();
            interceptTexts[i].text = intercepts.ToString();
            distanceTexts[i].text = distance.ToString("0") + " m";
        }
    }

    /// <summary>
    /// look at each players scores over each round, and determine the leading player
    /// in each field (e.g. highest scorer, highest distance travelled)
    /// </summary>
    public void DistributePoints()
    {
        int highestPoints = 0, mostGoals = 0, mostKicks = 0, mostIntercepts = 0;
        float highestDistanceTravelled = 0;

        // iterate over all players
        for (int i = 0; i < playerScores.Length; i++)
        {
            // counter variables
            int totalScore = 0, totalGoals = 0, totalKicks = 0, totalIntercepts = 0;
            float totalDistanceTravelled = 0.0f;

            // iterate over each round
            for (int j = 0; j < playerScores[i].Length; j++)
            {
                PlayerScores scores = playerScores[i][j];
                
                // increment counter variables
                totalScore += scores.score;
                totalGoals += scores.numberOfGoals;
                totalKicks += scores.numberOfKicks;
                totalIntercepts += scores.numberOfIntercepts;
                totalDistanceTravelled += scores.distanceTravelled;

                // reward 3 points for 1st place, 2 points for 2nd place, 1 point for 3rd place, etc.
                finalScores[i] += 4 - scores.place;

                // reward 1 point per golden sheep
                finalScores[i] += scores.numberOfGoldSheep;
            }

            // update leading players if necessary
            if (totalScore > highestPoints)
            {
                highestScoringPlayer = i; // update highest scoring player
                highestPoints = totalScore;
            }

            if (totalGoals > mostGoals)
            {
                mostGoalsPlayer = i; // update player who scored most goals
                mostGoals = totalGoals;
            }

            if (totalKicks > mostKicks)
            {
                mostKicksPlayer = i; // update player who kicked the most
                mostKicks = totalKicks;
            }

            if (totalIntercepts > mostIntercepts)
            {
                mostInterceptsPlayer = i; // update player who intercepted the most
                mostIntercepts = totalIntercepts;
            }

            if (totalDistanceTravelled > highestDistanceTravelled)
            {
                highestDistancePlayer = i; // update player who travelled the most
                highestDistanceTravelled = totalDistanceTravelled;
            }
        }

        // for each achievement, give a point to the leading player in that field
        finalScores[highestScoringPlayer]++;
        finalScores[mostGoalsPlayer]++;
        finalScores[mostKicksPlayer]++;
        finalScores[mostInterceptsPlayer]++;
        finalScores[highestDistancePlayer]++;
    }

    /// <summary>
    /// iterates over each of the final scores and prints them as required
    /// </summary>
    private void UpdateFinalScoreTexts()
    {
        // iterate over final scores
        for (int i = 0; i < finalScoreTexts.Length; i++)
        {
            // print it to UI
            finalScoreTexts[i].text = finalScores[i].ToString();
        }
    }

    /// <summary>
    /// iterates over all final scores and simple determines the highest
    /// </summary>
    /// <returns>returns the ID of the winning player</returns>
    private int EvaluateWinner()
    {
        int winner = 0;

        // iterate over all players/final scores
        for (int i = 0; i < finalScores.Length; i++)
        {
            // if new highest score...
            if (finalScores[i] > finalScores[winner])
                winner = i; // update winner
        }

        // update the UI text in the form "Player _ WINS!"
        playerWinText.text = "Player " + (winner + 1).ToString() + " WINS!";
        return winner;
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
