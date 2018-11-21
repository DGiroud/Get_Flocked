using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class SettingsMenu : MonoBehaviour {

    [Tooltip("Main Audio mixer/master volume")]
    public AudioMixer audioMixer;       //audio
    Resolution[] resolution;            //array of resolutions
    [Tooltip("Resolution Drop Down Box (1920 x 1080, ect...)")]
    public Dropdown resDropDown;        //dropdown of resolutions

    void Start()
    {
        ScreenResolution();
    }

    public void ScreenResolution()
    {
        //screen resolutions
        resolution = Screen.resolutions;
        //clear out the options in resdropdown
        resDropDown.ClearOptions();
        //list of strings in which is the options
        List<string> optionsList = new List<string>();
        //loop through each element and create a format for the resolution

        int currentResIndex = 0;
        for (int i = 0; i < resolution.Length; i++)
        {
            string option = resolution[i].width + " x " + resolution[i].height;
            optionsList.Add(option);

            //setting the screen resolution to what is selected on screen
            if (resolution[i].width == Screen.currentResolution.width &&
                 resolution[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        //adds the optionslist to the resultion drop down list
        resDropDown.AddOptions(optionsList);
        resDropDown.value = currentResIndex;
        resDropDown.RefreshShownValue();
    }

    /// <summary>
    /// back button
    /// </summary>
    /// <param name="gamePad"></param>
    public void GamePadBack(GamePadState gamePad)
    {
        //if button B is pressed
        if (gamePad.Buttons.B == ButtonState.Pressed)
            //goes back to pause screen
            if (LevelManager.Instance.gameState == GameState.Pause)
                LevelManager.Instance.Pause();
    }

    /// <summary>
    /// Adjusting volume sounds
    /// </summary>
    /// <param name="volume"></param>
    public void VolumeSettings (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    /// <summary>
    /// Setting the quality levels for the game
    /// Low/Medium/High
    /// </summary>
    /// <param name="qualityIndex"></param>
    public void QuialitySettings(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Allowing fullscreen when toggled
    /// </summary>
    /// <param name="isFullScreen"></param>
    public void FullScreenToggle(bool isFullScreen)
    {
        //setting fullscreen
        Screen.fullScreen = isFullScreen;
    }
}
