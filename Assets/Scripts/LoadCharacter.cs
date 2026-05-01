using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        Debug.Log("LoadCharacter script is running.");

        if (characterPrefabs.Length == 0)
        {
            Debug.LogError("characterPrefabs array is EMPTY. Assign prefabs in the Inspector.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("spawnPoint is NOT assigned in the Inspector.");
            return;
        }

        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0);
        Debug.Log("Loaded selectedCharacter index from PlayerPrefs: " + selectedCharacter);

        if (selectedCharacter >= characterPrefabs.Length)
        {
            Debug.LogError("selectedCharacter index " + selectedCharacter + " is out of range. Array size is " + characterPrefabs.Length);
            return;
        }

        GameObject prefab = characterPrefabs[selectedCharacter];
        Debug.Log("Spawning prefab: " + prefab.name);
        // Use prefab's original rotation instead of Quaternion.identity
        Quaternion spawnRotation = prefab.transform.rotation;
        Debug.Log("Spawning with rotation: " + spawnRotation.eulerAngles);
        GameObject spawnedCharacter = Instantiate(prefab, spawnPoint.position, spawnRotation);
        spawnedCharacter.tag = "Player"; // Tag the spawned character as Player
        Debug.Log("Character spawned and tagged successfully.");
    }
}