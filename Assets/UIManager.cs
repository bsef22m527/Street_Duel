using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void MusicOff()
    {
        if (MusicManager.instance != null)
        {
            MusicManager.instance.TurnAllMusicOff();
        }
    }

    public void MusicOn()
    {
        if (MusicManager.instance != null)
        {
            MusicManager.instance.TurnAllMusicOn();
        }
    }

    public void ResumeGameMusic()
    {
        if (MusicManager.instance != null)
        {
            MusicManager.instance.TurnOnBG();
        }
    }
}