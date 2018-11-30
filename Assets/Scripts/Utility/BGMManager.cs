using System.Collections;
using UnityEngine;

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

    public float volume;

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
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();

        volume = audioSource.volume;
	}
	
	// Update is called once per frame
	void Update ()
    { 
        switch (bgmPhase)
        {
            case BGMPhase.MainMenu: // main menu BGM
                audioSource.volume = volume;
                audioSource.clip = mainMenuBGM;
                break;
            case BGMPhase.Round1: // round 1 BGM
                audioSource.clip = round1; 
                break;
            case BGMPhase.Round2: // round 2 BGM
                audioSource.clip = round2;
                break;
            case BGMPhase.Round3: // round 3 BGM
                audioSource.clip = round3;
                break;
            case BGMPhase.RoundEnd:
                StartCoroutine(FadeOut(Time.deltaTime * 0.005f));
                break;
            case BGMPhase.GameOver: // game over BGM
                audioSource.volume = volume;
                audioSource.clip = gameOverJingle;
                break;
        }

        if (!audioSource.isPlaying)
        {
            StopAllCoroutines();
            StartCoroutine(PlayBGM());

            if (audioSource.clip == gameOverJingle)
                bgmPhase = BGMPhase.MainMenu;
        }
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
