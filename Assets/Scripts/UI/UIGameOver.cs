using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField]
    private Object game;
    [SerializeField]
    private Object mainMenu;

    /// <summary>
    /// loads the main game
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene(game.name);
    }

    /// <summary>
    /// loads the main menu
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu.name);
    }

    /// <summary>
    /// forces application quit
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
