using TMPro;
using UnityEngine;

public class CharacterButtonUI : MonoBehaviour
{
    private TextMeshProUGUI label;
    private CharacterData data;
    private CharacterSelectionManager manager;

    void Start()
    {
        // Auto-find label (TextMeshProUGUI) in children or on this GameObject
        label = GetComponentInChildren<TextMeshProUGUI>();
        if (label == null)
        {
            Debug.LogError("TextMeshProUGUI label not found on CharacterButtonUI!");
        }
    }

    public void Setup(CharacterData character, CharacterSelectionManager mgr)
    {
        data = character;
        manager = mgr;
        if (label != null)
        {
            label.text = character.characterName;
        }
    }

    public void OnClick()
    {
        manager.SelectCharacter(data);
    }
}