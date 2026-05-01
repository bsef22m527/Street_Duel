 using UnityEngine;

public class Punch : MonoBehaviour
{
    public int damage = 10;

    private bool hasHit = false; // 👈 ADD

    private void OnEnable()
{
    hasHit = false;
    Invoke(nameof(ForceDisable), 0.15f); // Reduced for tighter timing
}

void ForceDisable()
{
    gameObject.SetActive(false);
}

    private void OnDisable()
    {
        hasHit = false; // 👈 RESET when hitbox closes
    }

    public void ResetHit()
    {
        hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return; // 👈 PREVENT MULTIPLE HITS

        Debug.Log("Player punch hit: " + other.gameObject.name + " with tag: " + other.tag);

        if (other.CompareTag("Enemy"))
        {
            hasHit = true; // 👈 LOCK AFTER FIRST HIT

            Health enemyHealth = other.GetComponentInParent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Enemy Hit! Dealing " + damage + " damage.");
            }
            else
            {
                Debug.LogWarning("Hit enemy but no Health component found on " + other.gameObject.name);
            }
        }
    }
} 