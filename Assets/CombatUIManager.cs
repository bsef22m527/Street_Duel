using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    private PlayerCombat playerCombat;

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        // Keep trying to find player if not found yet
        if (playerCombat == null)
        {
            FindPlayer();
        }
    }

    private void FindPlayer()
    {
        // Find the spawned player character by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCombat = player.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                Debug.Log("CombatUIManager found PlayerCombat on: " + player.name);

                // Verify PlayerCombat has required components
                Animator anim = player.GetComponent<Animator>();
                if (anim == null)
                {
                    Debug.LogError("PlayerCombat found but Animator component missing!");
                }

                // Check for punch hitbox
                Transform punchHitbox = player.transform.Find("PunchHitbox");
                if (punchHitbox == null)
                {
                    Debug.LogError("PunchHitbox child not found on player!");
                }
                else
                {
                    Debug.Log("PunchHitbox found on player");
                }
            }
            else
            {
                Debug.LogError("PlayerCombat component not found on player: " + player.name);
                // List all components on the player for debugging
                Component[] components = player.GetComponents<Component>();
                Debug.Log("Components on player:");
                foreach (Component comp in components)
                {
                    Debug.Log("- " + comp.GetType().Name);
                }
            }
        }
        else
        {
            // Only log warning occasionally to avoid spam
            if (Time.frameCount % 60 == 0) // Every ~1 second at 60fps
            {
                Debug.LogWarning("Player not found in scene yet... (CombatUIManager)");
            }
        }
    }

    // Public method to manually refresh player finding (call this after spawning)
    public void RefreshPlayerReference()
    {
        Debug.Log("Manually refreshing player reference...");
        FindPlayer();
    }

    // Alternative method to set player directly
    public void SetPlayerReference(GameObject playerGO)
    {
        if (playerGO != null)
        {
            playerCombat = playerGO.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                Debug.Log("Player reference set manually to: " + playerGO.name);
            }
            else
            {
                Debug.LogError("Manual player reference set but no PlayerCombat component found!");
            }
        }
    }

    // UI Button methods - these will be called by the UI buttons
    public void OnPunchButton()
    {
        Debug.Log("OnPunchButton called");
        if (playerCombat != null)
        {
            playerCombat.Punch();
            Debug.Log("Punch action executed on: " + playerCombat.gameObject.name);
        }
        else
        {
            Debug.LogError("PlayerCombat not found for punch action! Trying to find player again...");
            FindPlayer();
            if (playerCombat != null)
            {
                playerCombat.Punch();
                Debug.Log("Punch action executed after re-finding player");
            }
        }
    }

    public void OnTauntButton()
    {
        Debug.Log("OnTauntButton called");
        if (playerCombat != null)
        {
            playerCombat.Taunt();
            Debug.Log("Taunt action executed on: " + playerCombat.gameObject.name);
        }
        else
        {
            Debug.LogError("PlayerCombat not found for taunt action! Trying to find player again...");
            FindPlayer();
            if (playerCombat != null)
            {
                playerCombat.Taunt();
                Debug.Log("Taunt action executed after re-finding player");
            }
        }
    }

    public void OnBlockButton()
    {
        Debug.Log("OnBlockButton called");
        if (playerCombat != null)
        {
            playerCombat.Block();
            Debug.Log("Block action executed on: " + playerCombat.gameObject.name);
        }
        else
        {
            Debug.LogError("PlayerCombat not found for block action! Trying to find player again...");
            FindPlayer();
            if (playerCombat != null)
            {
                playerCombat.Block();
                Debug.Log("Block action executed after re-finding player");
            }
        }
    }
}