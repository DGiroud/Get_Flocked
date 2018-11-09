using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region serializable structs

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

[System.Serializable]
public struct CountDownPanel
{
    // reference to panel
    public GameObject countDownPanel;

    // panel fields
    public Text countDownText;
    public string[] startTexts;
}

[System.Serializable]
public struct RoundEndPanel
{
    // reference to panel
    public GameObject roundEndPanel;
    public GameObject timesUpPanel;
    public GameObject readyUpPanel;

    // panel fields
    public GameObject[] notReadyPanels;
    public GameObject[] readyPanels;
    public Text[] scoreTexts;
}

#endregion

/// <summary>
/// 
/// </summary>
public class UIMainGame : MonoBehaviour
{
    // singleton instance
    #region singleton

    private static UIMainGame instance;

    /// <summary>
    /// getting for singleton instance of UIMainGame
    /// </summary>
    public static UIMainGame Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    // variables
    #region variables

    [SerializeField]
    private MainGamePanel mainGamePanel; // scores, timer, etc.

    [SerializeField]
    private CountDownPanel countDownPanel; // 3... 2... 1... GO

    [SerializeField]
    private RoundEndPanel roundEndPanel; // press "A" to ready up
    
    private static bool[] playerReady; // boolean array for players to toggle between ready/not ready at round end
    private string randomStartText;

    #endregion

    /// <summary>
    /// updates the UI round text e.g. "Round 1", "Round 2" etc.
    /// </summary>
    void Start ()
    {
        // assign singleton instance
        instance = this;

        // set up main game UI
        mainGamePanel.roundText.text = "Round " + (LevelManager.GetCurrentRound() + 1).ToString();

        // initialise player ready boolean array
        playerReady = new bool[4];

        // choose a random starting phrase (e.g. GO! Start! etc.)
        randomStartText = countDownPanel.startTexts[Random.Range(0, countDownPanel.startTexts.Length)];
    }

    /// <summary>
    /// draws the relevant UI such as the round timer and player scores
    /// </summary>
    void Update()
    {
        // depending on the current state of the game (Pause, RoundEnd, etc.) do stuff with the UI
        switch (LevelManager.Instance.gameState)
        {
            case GameState.CountDown: // count down UI
            {
                if (countDownPanel.countDownPanel.activeSelf == false)
                    ToggleCountDownUI(); // turn on the UI panel if it's not already active

                UpdateCountDownText(); // draw countdown
                break;
            }
            case GameState.Main: // main game UI
            {
                if (mainGamePanel.mainGamePanel.activeSelf == false)
                    ToggleMainUI(); // turn on the UI panel if it's not already active

                UpdateSheepCountText(); // draw timer
                UpdateScoreText(); // draw scores
                break;
            }
            case GameState.Pause: // pause UI
            {
            
                break;
            }
            case GameState.TimesUp:
            {
                if (roundEndPanel.timesUpPanel.activeSelf == false)
                    ToggleRoundEndUI(true); // turn on the UI panel if it's not already active                    
                break;
            }
            case GameState.RoundEnd: // round end UI
            {
                if (roundEndPanel.readyUpPanel.activeSelf == false)
                    ToggleRoundEndUI(false); // turn on the UI panel if it's not already active
                break;
            }
        }

    }

    #region UI toggle functions

    /// <summary>
    /// helper function which activates the countdown UI panels
    /// </summary>
    private void ToggleCountDownUI()
    {
        DisableAllPanels();
        countDownPanel.countDownPanel.SetActive(true);
    }

    /// <summary>
    /// helper function which activates the main game UI panels
    /// </summary>
    private void ToggleMainUI()
    {
        DisableAllPanels();
        mainGamePanel.mainGamePanel.SetActive(true);
    }

    /// <summary>
    /// helper function which activates the round end UI panels
    /// </summary>
    private void ToggleRoundEndUI(bool isMessage = true)
    {
        DisableAllPanels();
        roundEndPanel.roundEndPanel.SetActive(true);
        roundEndPanel.timesUpPanel.SetActive(isMessage);
        roundEndPanel.readyUpPanel.SetActive(!isMessage);

        SetCPUReady();
    }

    /// <summary>
    /// helper function which disables all UI panels in the main scene
    /// </summary>
    private void DisableAllPanels()
    {
        mainGamePanel.mainGamePanel.SetActive(false);
        countDownPanel.countDownPanel.SetActive(false);
        roundEndPanel.roundEndPanel.SetActive(false);
    }

    #endregion

    /// <summary>
    /// draws the count down timer which plays at the start of each round
    /// </summary>
    private void UpdateCountDownText()
    {
        // get seconds
        int seconds = (int)(LevelManager.Instance.countDown % 60);
        
        // print "3... 2... 1... GO!"
        if (seconds >= 1)
            countDownPanel.countDownText.text = seconds.ToString(); // print numbers
        else
            countDownPanel.countDownText.text = randomStartText; // print starting text
    }

    /// <summary>
    /// draws the round timer in the format of "00:00"
    /// </summary>
    private void UpdateSheepCountText()
    {
        // get current round time
        int currentSheep = ScoreManager.Instance.CurrentSheep;
        int requiredSheep = ScoreManager.Instance.RequiredSheep;

        // print on screen in the format of "0/0"
        mainGamePanel.timerText.text = currentSheep.ToString() + "/" + requiredSheep.ToString();
    }

    /// <summary>
    /// draws each player score in their respective corner
    /// </summary>
    private void UpdateScoreText()
    {
        // get scores
        int[] scores = ScoreManager.Instance.GetScores();

        // iterate over scores and print on screen
        for (int i = 0; i < scores.Length; i++)
        {
            // get score
            string score = scores[i].ToString();

            // update the text fields
            mainGamePanel.scoreTexts[i].text = score;
            roundEndPanel.scoreTexts[i].text = score;
        }
    }

    /// <summary>
    /// function which set's a player to "ready" and updates their panels accordingly
    /// </summary>
    /// <param name="actorID">the index of the player</param>
    public void ToggleReady(int actorID)
    {
        playerReady[actorID] = true; // ready
        roundEndPanel.notReadyPanels[actorID].SetActive(false);
        roundEndPanel.readyPanels[actorID].SetActive(true);

        if (CheckAllReady())
            LevelManager.Instance.NewRound();
    }

    /// <summary>
    /// helper function which iterates through all players, identifies which ones are CPU and
    /// automatically readies up for each one
    /// </summary>
    private void SetCPUReady()
    {
        // get all players
        List<GameObject> players = PlayerManager.Instance.players;

        // iterate over players
        for (int i = 0; i < players.Count; i++)
        {
            // get player
            BaseActor player = players[i].GetComponent<BaseActor>();

            // if player is a CPU
            if (player.actorType == ActorType.CPU)
            {
                // it's automatically ready for next round
                ToggleReady(i);
            }
        }
    }

    /// <summary>
    /// helper function which checks all players to see if they've all readied up
    /// </summary>
    /// <returns>false if there's one or more persons not ready, true if everyone is ready</returns>
    public static bool CheckAllReady()
    {
        // iterate over player ready boolean array
        for (int i = 0; i < playerReady.Length; i++)
        {
            // get the player's ready status
            bool isPlayerReady = playerReady[i];

            // if there's a single person not ready, return false
            if (!isPlayerReady)
                return false;
        }

        // everyone is ready
        return true;
    }
}
