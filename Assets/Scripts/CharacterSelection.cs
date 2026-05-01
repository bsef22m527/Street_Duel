using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public LoadCharacter loader;
    public GameObject[] characters;
    public string[] animationNames; // 👈 one name per character, fill in Inspector
    public int selectedCharacter = 0;

    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == 0);
        }
        PlayAnimation(selectedCharacter);
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
        PlayAnimation(selectedCharacter);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0) selectedCharacter += characters.Length;
        characters[selectedCharacter].SetActive(true);
        PlayAnimation(selectedCharacter);
    }

    void PlayAnimation(int index)
    {
        Animator anim = characters[index].GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play(animationNames[index]); // 👈 plays this character's specific animation
        }
        else
        {
            Debug.LogWarning("No Animator found on: " + characters[index].name);
        }
    }

   public void StartGame()
{
    PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    PlayerPrefs.Save();
    Debug.Log("Saved character index: " + selectedCharacter);
    SceneManager.LoadScene("Level 1"); // 👈 your exact game scene name
}
}