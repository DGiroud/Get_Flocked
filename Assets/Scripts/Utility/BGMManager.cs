using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGMPhase
{
    MainMenu,
    Round1,
    Round2,
    Round3,
    RoundEnd,
    GameOver
}

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    // singleton instance
    #region singleton

    private static BGMManager instance;

    /// <summary>
    /// getter for singleton instance of BGMManager
    /// </summary>
    public static BGMManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    private AudioSource audioSource;

    public BGMPhase bgmPhase;

    // the different BGM for each relevant scene
    [SerializeField]
    private AudioClip mainMenuBGM;
    [SerializeField]
    private AudioClip round1;
    [SerializeField]
    private AudioClip round2;
    [SerializeField]
    private AudioClip round3;
    [SerializeField]
    private AudioClip gameOverJingle;

    // Use this for initialization
    void Start ()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);

        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(audioSource.time + " / " + audioSource.clip.length);

        switch (bgmPhase)
        {
            case BGMPhase.MainMenu: // main menu BGM
                audioSource.volume = 1.0f;
                audioSource.clip = mainMenuBGM;
                break;
            case BGMPhase.Round1: // round 1 BGM

                if (audioSource.time < audioSource.clip.length)
                    return;

                audioSource.clip = round1; 
                break;
            case BGMPhase.Round2: // round 2 BGM

                if (audioSource.time < audioSource.clip.length)
                    return;

                audioSource.clip = round2;
                break;
            case BGMPhase.Round3: // round 3 BGM

                if (audioSource.time < audioSource.clip.length)
                    return;

                audioSource.clip = round3;
                break;
            case BGMPhase.RoundEnd:
                StartCoroutine(FadeOut(Time.deltaTime * 0.01f));
                break;
            case BGMPhase.GameOver: // game over BGM
                audioSource.volume = 1.0f;
                audioSource.clip = gameOverJingle;
                break;
        }
        
        if (!audioSource.isPlaying)
            StartCoroutine(PlayBGM());
    }

    IEnumerator PlayBGM()
    {
        audioSource.Play();
        yield return new WaitForSecondsRealtime(audioSource.clip.length);
    }

    public IEnumerator FadeOut(float value)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= value;
            yield return null;
        }
    }

    IEnumerator FadeIn(float value)
    {
        audioSource.volume += value;
        yield return null;
    }
}
