using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private AudioSource bgAudioSource;
    private bool isMusicOn = true; // 🎛 global state

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        bgAudioSource = GetComponent<AudioSource>();
        bgAudioSource.loop = true;
        bgAudioSource.Play();
    }

    // 🎵 BACKGROUND CONTROL
    public void TurnOffBG()
    {
        if (bgAudioSource.isPlaying)
            bgAudioSource.Pause();
    }

    public void TurnOnBG()
    {
        if (isMusicOn)
            bgAudioSource.UnPause();
    }

    // 🎛 GLOBAL MUSIC OFF
    public void TurnAllMusicOff()
    {
        isMusicOn = false;

        // Stop background
        if (bgAudioSource != null)
            bgAudioSource.Pause();

        // Stop ALL audio in scene
        AudioListener.pause = true;
    }

    // 🎛 GLOBAL MUSIC ON
    public void TurnAllMusicOn()
    {
        isMusicOn = true;

        AudioListener.pause = false;

        // Resume background
        if (bgAudioSource != null)
            bgAudioSource.UnPause();
    }

    public bool IsMusicOn()
    {
        return isMusicOn;
    }
}