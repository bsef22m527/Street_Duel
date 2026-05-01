using UnityEngine;

public class  ExitButton : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Game is exiting...");

        Application.Quit();

        // This only works inside Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}