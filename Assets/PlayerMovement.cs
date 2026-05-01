using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float acceleration = 10f; // New: for smooth acceleration
    private Joystick joystick;
    private Transform enemy;

    // Expose state (read-only for other scripts)
    public float InputValue { get; private set; }
    public int MovementState { get; private set; }
    // 1 = forward (towards enemy)
    // -1 = backward (away)
    // 0 = idle

    private Vector3 currentVelocity; // For SmoothDamp

    void Start()
    {
        // Auto-find joystick in scene
        joystick = FindAnyObjectByType<Joystick>();
        if (joystick == null)
        {
            Debug.LogError("Joystick not found in scene!");
        }

        // Auto-find enemy by tag
        GameObject enemyGO = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyGO != null)
        {
            enemy = enemyGO.transform;
        }
        else
        {
            Debug.LogError("Enemy not found in scene!");
        }
    }

    void Update()
    {
        // Get input from both joystick and keyboard
        float joystickInput = joystick != null ? joystick.Horizontal : 0f;
        float keyboardInput = 0f;

        // Check arrow keys
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            keyboardInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            keyboardInput = 1f;
        }

        // Use joystick input if available, otherwise use keyboard input
        float targetInput = Mathf.Abs(joystickInput) > 0.1f ? joystickInput : keyboardInput;

        // Smooth the input value for gradual acceleration
        InputValue = Mathf.SmoothDamp(InputValue, targetInput, ref currentVelocity.x, 0.1f);

        Vector3 directionToEnemy = (enemy.position - transform.position);
        directionToEnemy.y = 0f;
        directionToEnemy.Normalize();

        Vector3 movement = directionToEnemy * InputValue;
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        UpdateState(InputValue);
    }

    void UpdateState(float input)
    {
        float deadZone = 0.1f;

        if (input > deadZone)
            MovementState = 1;
        else if (input < -deadZone)
            MovementState = -1;
        else
            MovementState = 0;
    }
}