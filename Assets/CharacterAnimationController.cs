using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Called by UI Buttons
    public void PlayAnimation(string triggerName)
    {
        if(anim == null)
        {
            Debug.LogWarning("Animator not found on character.");
            return;
        }

        anim.SetTrigger(triggerName);
    }
}