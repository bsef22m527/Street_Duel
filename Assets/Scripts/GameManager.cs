using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public GameObject selectionUI; // drag your Canvas here

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Debug.Log("GameManager running. Showing character selection.");
        selectionUI.SetActive(true); // make sure selection UI is visible at start
    }

    public void SpawnCharacter(int index)
    {
        if (index >= characterPrefabs.Length)
        {
            Debug.LogError("Index out of range: " + index);
            return;
        }

        Debug.Log("Spawning: " + characterPrefabs[index].name);
        GameObject spawnedCharacter = Instantiate(characterPrefabs[index], spawnPoint.position, Quaternion.identity);
        spawnedCharacter.tag = "Player"; // Tag the spawned character as Player
        Debug.Log("Character spawned and tagged successfully.");
    }
}