using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public CharacterData[] characters;

    private Transform previewSpawnPoint;
    private GameObject buttonPrefab;
    private Transform contentParent;

    private GameObject currentPreview;

    private void Start()
    {
        // Auto-find previewSpawnPoint by name/tag
        GameObject previewPoint = GameObject.Find("PreviewSpawnPoint");
        if (previewPoint != null)
        {
            previewSpawnPoint = previewPoint.transform;
        }
        else
        {
            // Fallback: search by tag
            GameObject tagged = GameObject.FindGameObjectWithTag("PreviewSpawnPoint");
            if (tagged != null)
            {
                previewSpawnPoint = tagged.transform;
            }
            else
            {
                Debug.LogError("PreviewSpawnPoint not found. Name it 'PreviewSpawnPoint' or tag it.");
            }
        }

        // Auto-find contentParent (ScrollView Content) by name
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

        // Auto-find buttonPrefab in Resources or as child
        // Assuming it's in Resources/CharacterButtonPrefab
        buttonPrefab = Resources.Load<GameObject>("CharacterButtonPrefab");
        if (buttonPrefab == null)
        {
            // Fallback: look for it in scene as a child (disabled)
            CharacterButtonUI buttonComponent = GetComponentInChildren<CharacterButtonUI>(true);
            if (buttonComponent != null)
            {
                buttonPrefab = buttonComponent.gameObject;
            }
            else
            {
                Debug.LogError("ButtonPrefab not found. Place it in Resources/CharacterButtonPrefab.prefab or as a child.");
            }
        }

        PopulateButtons();
    }

    void PopulateButtons()
    {
        foreach (var c in characters)
        {
            var obj = Instantiate(buttonPrefab, contentParent);
            obj.GetComponent<CharacterButtonUI>().Setup(c, this);
        }
    }

    public void SelectCharacter(CharacterData data)
    {
        if (currentPreview != null)
            Destroy(currentPreview);

        currentPreview = Instantiate(data.characterPrefab, previewSpawnPoint.position, Quaternion.identity);
        Debug.Log("This");
        GameSession.Instance.selectedCharacterPrefab = data.characterPrefab;
    }
}