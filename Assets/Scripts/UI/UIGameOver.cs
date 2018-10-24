using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField]
    private int mainMenuLevelID;
    [SerializeField]
    private int gameLevelID;

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
