using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartMenu : MonoBehaviour
{
    [SerializeField]
    private int lobbySceneID;

    /// <summary>
    /// loads the main game
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(lobbySceneID);
    }

    /// <summary>
    /// forces application quit
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
