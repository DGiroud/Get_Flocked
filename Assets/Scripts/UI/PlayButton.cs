using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField]
    private int sceneID;

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneID);
    }

}
