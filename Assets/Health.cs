using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private AudioSource hurtAudio;
    private float lastHurtTime = 0f;
    public float hurtCooldown = 0.3f;

    public int maxHealth = 100;
    public int currentHealth;
    public bool isBlocking = false;

    private AudioSource winAudioSource;
    private AudioSource loseAudioSource;
    private Animator animator;
    private Slider healthBar;

    public bool isDead = false;

    // 🎮 PANELS
    private GameObject winPanel;
    private GameObject losePanel;

    private float panelDelay = 4f;

    // 🕹 JOYSTICK (TAG BASED - PREFAB SAFE)
    private GameObject joystickCanvas;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animator not found on Health!");

        // =========================
        // 🕹 FIND JOYSTICK VIA TAG
        // =========================
        joystickCanvas = GameObject.FindGameObjectWithTag("JoystickCanvas");

        if (joystickCanvas == null)
            Debug.LogWarning("JoystickCanvas not found! Make sure it has tag 'JoystickCanvas'");

        // =========================
        // HEALTH BAR SETUP
        // =========================
        if (GetComponent<PlayerCombat>() != null)
        {
            healthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar")?.GetComponent<Slider>();
        }
        else if (GetComponent<EnemyAI>() != null)
        {
            healthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar")?.GetComponent<Slider>();
        }
        else
        {
            healthBar = FindObjectOfType<Slider>();
        }

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // =========================
        // PANEL FINDING
        // =========================
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("WinPanel"))
                winPanel = obj;
            else if (obj.CompareTag("LosePanel"))
                losePanel = obj;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (isBlocking)
            damage /= 2;

        currentHealth -= damage;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (animator != null)
            animator.SetTrigger("Hit");

        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null)
            enemyAI.StartHit();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        DisableCharacter();

        StartCoroutine(ShowPanelWithDelay());
    }

    void DisableCharacter()
    {
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        PlayerCombat pc = GetComponent<PlayerCombat>();
        if (pc != null) pc.enabled = false;

        if (GetComponent<PlayerCombat>() != null)
        {
            EnemyAI[] allEnemies = FindObjectsOfType<EnemyAI>();
            foreach (EnemyAI enemy in allEnemies)
                enemy.OnPlayerDeath();
        }
    }

    void FindAudioSources()
    {
        GameObject winObj = GameObject.FindGameObjectWithTag("WinMusic");
        if (winObj != null)
            winAudioSource = winObj.GetComponent<AudioSource>();

        GameObject loseObj = GameObject.FindGameObjectWithTag("LoseMusic");
        if (loseObj != null)
            loseAudioSource = loseObj.GetComponent<AudioSource>();
    }

    public void PlayHurtSound()
    {
        if (isDead) return;

        if (hurtAudio == null || hurtAudio.clip == null) return;

        if (MusicManager.instance != null && !MusicManager.instance.IsMusicOn())
            return;

        hurtAudio.PlayOneShot(hurtAudio.clip);
    }

    IEnumerator ShowPanelWithDelay()
    {
        yield return new WaitForSeconds(panelDelay);

        FindAudioSources();

        if (MusicManager.instance != null)
            MusicManager.instance.TurnOffBG();

        // =========================
        // PLAYER LOST
        // =========================
        if (GetComponent<PlayerCombat>() != null)
        {
            if (losePanel != null)
                losePanel.SetActive(true);

            // 🕹 DISABLE JOYSTICK
            if (joystickCanvas != null)
                joystickCanvas.SetActive(false);

            if (loseAudioSource != null && MusicManager.instance.IsMusicOn())
                loseAudioSource.Play();
        }
        // =========================
        // ENEMY LOST (WIN)
        // =========================
        else
        {
            if (winPanel != null)
                winPanel.SetActive(true);

            // 🕹 DISABLE JOYSTICK
            if (joystickCanvas != null)
                joystickCanvas.SetActive(false);

            if (winAudioSource != null && MusicManager.instance.IsMusicOn())
                winAudioSource.Play();
        }
    }

    // =========================
    // OPTIONAL RESET FUNCTION
    // =========================
    public void ResetJoystick()
    {
        joystickCanvas = GameObject.FindGameObjectWithTag("JoystickCanvas");

        if (joystickCanvas != null)
            joystickCanvas.SetActive(true);
    }
}