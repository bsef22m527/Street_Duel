using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private GameObject punchHitbox;
    private Health health;
    private Joystick joystick;
    private PlayerMovement playerMovement;

    [SerializeField] private AudioSource punchAudio; // 🎵 punch
    [SerializeField] private AudioSource blockAudio; // 🛡 block NEW

    private float punchBufferTime = 0.2f; // Buffer window for punch input
    private float punchBufferTimer = 0f;
    private bool punchBuffered = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogError("Animator not found on PlayerCombat!");

        if (punchAudio == null)
            Debug.LogWarning("Punch AudioSource not assigned!");

        if (blockAudio == null)
            Debug.LogWarning("Block AudioSource not assigned!");

        Transform hitboxTransform = transform.Find("PunchHitbox");
        if (hitboxTransform != null)
            punchHitbox = hitboxTransform.gameObject;
        else
            punchHitbox = GetComponentInChildren<Punch>()?.gameObject;

        if (punchHitbox == null)
            Debug.LogError("PunchHitbox not found!");

        health = GetComponent<Health>();

        if (punchHitbox != null)
            punchHitbox.SetActive(false);

        // Get references to joystick and movement controller
        joystick = FindAnyObjectByType<Joystick>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // IMPORTANT: Only process punch input if NOT using joystick movement
        // This prevents the joystick from accidentally triggering punches
        bool isMovingWithJoystick = joystick != null && Mathf.Abs(joystick.Horizontal) > 0.1f;

        // Handle punch input buffering (only from Space key, UI buttons are handled separately via CombatUIManager.OnPunchButton())
        if (!isMovingWithJoystick && Input.GetKeyDown(KeyCode.Space))
        {
            punchBufferTimer = punchBufferTime;
            punchBuffered = true;
        }

        if (punchBuffered)
        {
            punchBufferTimer -= Time.deltaTime;
            if (punchBufferTimer <= 0f)
            {
                punchBuffered = false;
            }
        }

        // Check for buffered punch when animation allows
        if (punchBuffered && CanPunch())
        {
            Punch();
            punchBuffered = false;
        }
    }

    private bool CanPunch()
    {
        // Check if not currently punching (based on animator state)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsName("Punch"); // Assuming "Punch" is the state name
    }

    public void Punch()
    {
        if (!CanPunch()) return; // Prevent overlapping punches
        animator.SetTrigger("Punch");
    }

    public void Taunt()
    {
        animator.SetTrigger("Taunt");
    }

    public void Block()
    {
        animator.SetTrigger("Block");
    }

    // 🎵 PUNCH SOUND (Animation Event)
    public void PlayPunchSound()
    {
        if (MusicManager.instance != null && !MusicManager.instance.IsMusicOn())
            return;

        if (punchAudio != null && punchAudio.clip != null)
        {
            punchAudio.PlayOneShot(punchAudio.clip);
        }
    }

    // 🛡 BLOCK SOUND (Animation Event) — NEW
    public void PlayBlockSound()
    {
        if (MusicManager.instance != null && !MusicManager.instance.IsMusicOn())
            return;

        if (blockAudio != null && blockAudio.clip != null)
        {
            blockAudio.PlayOneShot(blockAudio.clip);
        }
    }

    public void EnableHitbox()
    {
        punchHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        Punch punch = punchHitbox.GetComponent<Punch>();
        if (punch != null)
            punch.ResetHit();

        punchHitbox.SetActive(false);
    }

    public void StartBlock()
    {
        health.isBlocking = true;
    }

    public void StopBlock()
    {
        health.isBlocking = false;
    }
}