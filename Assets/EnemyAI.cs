using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private Animator animator;

    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;
    public float attackDistance = 2f;

    private float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    private GameObject enemyHitbox;
    private bool isAttacking = false;

    private bool isHit = false;
    private float hitTimer = 0f;
    private float hitDuration = 0.3f;

    private AudioSource attackAudio;

    private Vector3 moveVelocity;

    void Awake()
    {
        animator = GetComponent<Animator>();
        attackAudio = GetComponent<AudioSource>();

        if (animator == null)
            Debug.LogError("Animator not found on EnemyAI!");

        if (attackAudio == null)
            Debug.LogWarning("Attack AudioSource not found on Enemy!");

        Transform hitboxTransform = transform.Find("EnemyHitbox");

        if (hitboxTransform != null)
        {
            enemyHitbox = hitboxTransform.gameObject;
        }
        else
        {
            EnemyAttack atk = GetComponentInChildren<EnemyAttack>();
            if (atk != null)
                enemyHitbox = atk.gameObject;
        }

        if (enemyHitbox != null)
            enemyHitbox.SetActive(false);
        else
            Debug.LogError("EnemyHitbox not found!");
    }

    void Start()
    {
        TryFindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            TryFindPlayer();
            return;
        }

        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null && playerHealth.isDead)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        Health myHealth = GetComponent<Health>();
        if (myHealth != null && myHealth.isDead)
            return;

        if (isHit)
        {
            hitTimer -= Time.deltaTime;

            if (hitTimer <= 0f)
                EndHit();
            else
            {
                animator.SetFloat("Speed", 0f);
                return;
            }
        }

        float distance = Vector3.Distance(transform.position, player.position);

        RotateTowardsPlayer();

        if (isAttacking)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (distance > attackDistance)
        {
            MoveTowardsPlayer();
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            AttackPlayer();
        }
    }

    // =========================
    // PLAYER FIND
    // =========================
    void TryFindPlayer()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");

        if (playerGO != null)
        {
            player = playerGO.transform;
            Debug.Log("EnemyAI: Player found and assigned.");
        }
    }

    // =========================
    // MOVEMENT
    // =========================
    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - direction * attackDistance;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref moveVelocity,
            0.1f
        );
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position);
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    // =========================
    // ATTACK SYSTEM
    // =========================
    void AttackPlayer()
    {
        if (isAttacking) return;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    // =========================
    // HITBOX CONTROL
    // =========================
    public void EnableHitbox()
    {
        if (enemyHitbox != null)
            enemyHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        if (enemyHitbox != null)
            enemyHitbox.SetActive(false);
    }

    // =========================
    // HIT SYSTEM
    // =========================
    public void StartHit()
    {
        isHit = true;
        isAttacking = false;

        DisableHitbox();
        hitTimer = hitDuration;
    }

    public void EndHit()
    {
        isHit = false;
        lastAttackTime = 0f;
        Debug.Log("Enemy recovered from hit");
    }

    // =========================
    // PLAYER DEATH
    // =========================
    public void OnPlayerDeath()
    {
        isAttacking = false;
        lastAttackTime = 0f;

        animator.SetFloat("Speed", 0f);
        DisableHitbox();

        Debug.Log("Player died. Enemy idle.");
    }

    // =========================
    // SOUND
    // =========================
    public void PlayAttackSound()
    {
        if (attackAudio == null || attackAudio.clip == null)
            return;

        if (MusicManager.instance != null && !MusicManager.instance.IsMusicOn())
            return;

        attackAudio.PlayOneShot(attackAudio.clip);
    }
}