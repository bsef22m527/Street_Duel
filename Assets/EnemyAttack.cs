using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;
    private bool hasHit = false;

    private void OnEnable()
    {
        hasHit = false; // reset every attack
        Invoke(nameof(ForceDisable), 0.15f); // Add safety disable
    }

    void ForceDisable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        Debug.Log("Enemy attack hit: " + other.gameObject.name + " with tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            hasHit = true;

            Health playerHealth = other.GetComponentInParent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player Hit by Enemy! Dealing " + damage + " damage.");
            }
            else
            {
                Debug.LogWarning("Hit player but no Health component found on " + other.gameObject.name);
            }
        }
    }
}