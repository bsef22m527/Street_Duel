using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
    [Tooltip("If true, LoadNextLevel will wrap from the last build scene back to the first.")]
    public bool wrapAround = false;

    public void RetryCurrentLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadNextLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int total = SceneManager.sceneCountInBuildSettings;
        int next = current + 1;

        if (next >= total)
        {
            if (wrapAround)
            {
                next = 0;
            }
            else
            {
                Debug.LogWarning("LevelTransitionManager: already at last scene and wrapAround is disabled.");
                return;
            }
        }

        SceneManager.LoadScene(next);
    }

    public void LoadSceneByIndex(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.LogWarning($"LevelTransitionManager: invalid scene index {index}.");
        }
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
