﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartMenu : MonoBehaviour
{
    [SerializeField]
    private int gameLevelID;

    /// <summary>
    /// loads the main game
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(gameLevelID);
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
