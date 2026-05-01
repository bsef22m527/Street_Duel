using UnityEngine;

public class LevelListUI : MonoBehaviour
{
    private Transform contentParent;
    private GameObject buttonPrefab;

    private void Start()
    {
        // Auto-find contentParent by name
        GameObject content = GameObject.Find("Content");
        if (content != null)
        {
            contentParent = content.transform;
        }
        else
        {
            // Fallback: find by tag
            GameObject contentGO = GameObject.FindGameObjectWithTag("Content");
            if (contentGO != null)
            {
                contentParent = contentGO.transform;
            }
            else
            {
                Debug.LogError("Content parent not found. Name it 'Content' or tag it.");
            }
        }

        // Auto-find buttonPrefab in Resources
        buttonPrefab = Resources.Load<GameObject>("LevelButtonPrefab");
        if (buttonPrefab == null)
        {
            // Fallback: look for it in scene as a child
            LevelButtonUI buttonComponent = GetComponentInChildren<LevelButtonUI>(true);
            if (buttonComponent != null)
            {
                buttonPrefab = buttonComponent.gameObject;
            }
            else
            {
                Debug.LogError("ButtonPrefab not found. Place it in Resources/LevelButtonPrefab.prefab or as a child.");
            }
        }

        Populate();
    }

    void Populate()
    {
        for (int i = 0; i < LevelManager.Instance.levels.Length; i++)
        {
            var data = LevelManager.Instance.levels[i];

            GameObject btnObj = Instantiate(buttonPrefab, contentParent);

            var ui = btnObj.GetComponent<LevelButtonUI>();
            ui.Setup(data.levelName, i);
        }
    }
}
