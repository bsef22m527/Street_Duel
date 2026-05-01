using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        // Check if GameSession has a selected character
        if (GameSession.Instance != null && GameSession.Instance.selectedCharacterPrefab != null)
        {
            Debug.Log("PlayerSpawner: Found selected character prefab: " + GameSession.Instance.selectedCharacterPrefab.name);

            // Find a spawn point (you can set this in Inspector or find by tag)
            Transform spawnPoint = transform; // Use this object's position, or set in Inspector

            // Spawn the character
            GameObject spawnedPlayer = Instantiate(GameSession.Instance.selectedCharacterPrefab,
                                                 spawnPoint.position,
                                                 GameSession.Instance.selectedCharacterPrefab.transform.rotation);

            // Tag it as Player
            spawnedPlayer.tag = "Player";

            Debug.Log("PlayerSpawner: Character spawned and tagged as 'Player': " + spawnedPlayer.name);

            // Notify CombatUIManager if it exists
            CombatUIManager cui = FindObjectOfType<CombatUIManager>();
            if (cui != null)
            {
                cui.RefreshPlayerReference();
                Debug.Log("PlayerSpawner: Notified CombatUIManager to refresh reference");
            }
        }
        else
        {
            Debug.LogError("PlayerSpawner: No selected character found in GameSession!");
        }
    }
}