using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    private TextMeshProUGUI label;
    private int levelIndex;

    void Start()
    {
        // Auto-find label (TextMeshProUGUI) in children or on this GameObject
        label = GetComponentInChildren<TextMeshProUGUI>();
        if (label == null)
        {
            Debug.LogError("TextMeshProUGUI label not found on LevelButtonUI!");
        }
    }

    public void Setup(string levelName, int index)
    {
        if (label != null)
        {
            label.text = levelName;
        }
        levelIndex = index;
    }

    public void OnClick()
    {
        LevelManager.Instance.LoadLevel(levelIndex);
    }
}
