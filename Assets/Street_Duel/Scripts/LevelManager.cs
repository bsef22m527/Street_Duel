using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] public LevelData[] levels;
    private GameObject currentLevel;
    public Transform spawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadLevel(int index)
    {
        if (currentLevel != null)
            Destroy(currentLevel);

        currentLevel = Instantiate(levels[index].levelPrefab, spawnPoint.position, Quaternion.identity);

        // After the level prefab is instantiated, spawn the selected player character
        SpawnSelectedPlayer();
    }

    private void SpawnSelectedPlayer()
    {
        if (GameSession.Instance == null || GameSession.Instance.selectedCharacterPrefab == null)
            return;

        Transform playerSpawn = null;

        // Try to find a child named "PlayerSpawn" inside the instantiated level
        foreach (Transform t in currentLevel.GetComponentsInChildren<Transform>())
        {
            if (t.name == "PlayerSpawn")
            {
                playerSpawn = t;
                break;
            }
        }

        // If not found by name, try to find by tag "PlayerSpawn" on children
        if (playerSpawn == null)
        {
            foreach (Transform t in currentLevel.GetComponentsInChildren<Transform>())
            {
                if (t.CompareTag("PlayerSpawn"))
                {
                    playerSpawn = t;
                    break;
                }
            }
        }

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        if (playerSpawn != null)
        {
            spawnPos = playerSpawn.position;
            spawnRot = playerSpawn.rotation;
        }

        GameObject spawnedPlayer = Instantiate(GameSession.Instance.selectedCharacterPrefab, spawnPos, spawnRot);
        spawnedPlayer.tag = "Player"; // Tag the spawned character as Player
        Debug.Log("Player spawned and tagged: " + spawnedPlayer.name);
    }
}
