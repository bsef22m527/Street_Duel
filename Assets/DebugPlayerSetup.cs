using UnityEngine;

public class DebugPlayerSetup : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== DEBUG PLAYER SETUP ===");

        // Check if there's a player tagged in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("Found player by tag: " + player.name);
            Debug.Log("Player position: " + player.transform.position);
            Debug.Log("Player rotation: " + player.transform.rotation.eulerAngles);

            // Check if it has PlayerCombat
            PlayerCombat pc = player.GetComponent<PlayerCombat>();
            if (pc != null)
            {
                Debug.Log("Player has PlayerCombat component ✓");

                // Check PlayerCombat's internal state
                Animator anim = player.GetComponent<Animator>();
                if (anim != null)
                {
                    Debug.Log("PlayerCombat has Animator ✓");
                }
                else
                {
                    Debug.LogError("PlayerCombat missing Animator!");
                }

                // Check for punch hitbox
                Transform punchTransform = player.transform.Find("PunchHitbox");
                if (punchTransform != null)
                {
                    Debug.Log("PunchHitbox found ✓");
                }
                else
                {
                    Debug.LogError("PunchHitbox not found! Looking for Punch component...");
                    Punch punchComp = player.GetComponentInChildren<Punch>();
                    if (punchComp != null)
                    {
                        Debug.Log("Found Punch component on: " + punchComp.gameObject.name);
                    }
                    else
                    {
                        Debug.LogError("No Punch component found either!");
                    }
                }
            }
            else
            {
                Debug.LogError("Player does NOT have PlayerCombat component!");
            }

            // Check if it has PlayerMovement (since movement works)
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                Debug.Log("Player has PlayerMovement component ✓");
            }
            else
            {
                Debug.LogError("Player does NOT have PlayerMovement component!");
            }
        }
        else
        {
            Debug.LogError("No GameObject tagged as 'Player' found in scene!");

            // List all GameObjects that might be the player
            Debug.Log("Looking for potential player objects:");
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("Amy") || obj.name.Contains("Player") ||
                    obj.GetComponent<PlayerMovement>() != null ||
                    obj.GetComponent<PlayerCombat>() != null)
                {
                    Debug.Log("- POTENTIAL PLAYER: " + obj.name + " (tag: " + obj.tag + ")");
                    Component[] comps = obj.GetComponents<Component>();
                    foreach (Component comp in comps)
                    {
                        if (comp is PlayerMovement || comp is PlayerCombat || comp is Animator)
                        {
                            Debug.Log("  - Has: " + comp.GetType().Name);
                        }
                    }
                }
            }
        }

        // Check if CombatUIManager exists
        CombatUIManager cui = FindObjectOfType<CombatUIManager>();
        if (cui != null)
        {
            Debug.Log("CombatUIManager found in scene ✓");
        }
        else
        {
            Debug.LogError("CombatUIManager NOT found in scene!");
        }

        // Check GameSession
        if (GameSession.Instance != null)
        {
            if (GameSession.Instance.selectedCharacterPrefab != null)
            {
                Debug.Log("GameSession has selectedCharacterPrefab: " + GameSession.Instance.selectedCharacterPrefab.name);
            }
            else
            {
                Debug.LogWarning("GameSession exists but selectedCharacterPrefab is null");
            }
        }
        else
        {
            Debug.LogWarning("GameSession.Instance is null");
        }

        Debug.Log("=== END DEBUG ===");
    }
}