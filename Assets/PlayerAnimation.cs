using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;

    void Start()
    {
        // Auto-find animator on this GameObject
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on PlayerAnimation!");
        }

        // Auto-find PlayerMovement on this GameObject
        movement = GetComponent<PlayerMovement>();
        if (movement == null)
        {
            Debug.LogError("PlayerMovement not found on same GameObject!");
        }
    }

    void Update()
    {
        float input = movement.InputValue;

        // Optional smoothing
        animator.SetFloat("Move", input, 0.2f, Time.deltaTime); // Increased damp time for smoother transitions
    }
}