using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;       //audio
    Resolution[] resolution;            //array of resolutions
    public Dropdown resDropDown;        //dropdown of resolutions
    public DynamicCamera other;
    void Start()
    {
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
    
    //public void GetDynamicCam()
    //{
    //    other.MoveZoom();
    //}

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
        Screen.fullScreen = isFullScreen;
    }
}
