using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartMenu : MonoBehaviour
{
    [SerializeField]
    private Object game;

    /// <summary>
    /// loads the main game
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(game.name);
    }

    /// <summary>
    /// opens the settings menu
    /// </summary>
    public void Settings()
    {

    }

    /// <summary>
    /// forces application quit
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
