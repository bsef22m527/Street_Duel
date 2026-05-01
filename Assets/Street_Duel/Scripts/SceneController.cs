using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Tooltip("If true, next from last will go to first and back from first goes to last.")]
    public bool wrapAround = true;

    // Call this from the Next button's OnClick
    public void LoadNextScene()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int total = SceneManager.sceneCountInBuildSettings;
        int next = current + 1;
        if (next >= total)
        {
            if (wrapAround) next = 0;
            else
            {
                Debug.LogWarning("SceneController: already at last scene.");
                return;
            }
        }
        SceneManager.LoadScene(next);
    }

    // Call this from the Back button's OnClick
    public void LoadPreviousScene()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int total = SceneManager.sceneCountInBuildSettings;
        int prev = current - 1;
        if (prev < 0)
        {
            if (wrapAround) prev = total - 1;
            else
            {
                Debug.LogWarning("SceneController: already at first scene.");
                return;
            }
        }
        SceneManager.LoadScene(prev);
    }

    // Optional helpers
    public void LoadSceneByIndex(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index);
        else
            Debug.LogWarning("SceneController: invalid scene index " + index);
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
