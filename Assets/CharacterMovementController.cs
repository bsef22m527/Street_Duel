using UnityEngine;
using UnityEngine.UI;

public class CharacterMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of movement
    public float maxX = 10f; // Maximum X position (right boundary)
    public float minX = -10f; // Minimum X position (left boundary)

    [Header("UI Buttons")]
    private Button leftButton; // Reference to the left arrow button on canvas
    private Button rightButton; // Reference to the right arrow button on canvas

    private bool moveLeft = false;
    private bool moveRight = false;

    void Start()
    {
        // Auto-find buttons by name
        GameObject leftButtonGO = GameObject.Find("LeftButton");
        if (leftButtonGO != null)
        {
            leftButton = leftButtonGO.GetComponent<Button>();
        }

        GameObject rightButtonGO = GameObject.Find("RightButton");
        if (rightButtonGO != null)
        {
            rightButton = rightButtonGO.GetComponent<Button>();
        }

        // Fallback: find all buttons and assign based on position or hierarchy
        if (leftButton == null || rightButton == null)
        {
            Button[] allButtons = FindObjectsOfType<Button>();
            foreach (Button btn in allButtons)
            {
                if (btn.gameObject.name.Contains("Left"))
                    leftButton = btn;
                else if (btn.gameObject.name.Contains("Right"))
                    rightButton = btn;
            }
        }

        // Add listeners to UI buttons
        if (leftButton != null)
        {
            leftButton.onClick.AddListener(() => StartMovingLeft());
        }
        if (rightButton != null)
        {
            rightButton.onClick.AddListener(() => StartMovingRight());
        }
    }

    void Update()
    {
        // Handle keyboard input
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }

        // Handle UI button continuous movement (if implemented)
        if (moveLeft)
        {
            MoveLeft();
        }
        if (moveRight)
        {
            MoveRight();
        }
    }

    void MoveLeft()
    {
        Vector3 newPosition = transform.position + Vector3.left * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;
    }

    void MoveRight()
    {
        Vector3 newPosition = transform.position + Vector3.right * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;
    }

    // Methods for UI buttons (for single press or toggle)
    public void StartMovingLeft()
    {
        // For continuous movement, you could set moveLeft = true;
        // But for button press, just move once or toggle
        MoveLeft();
    }

    public void StartMovingRight()
    {
        MoveRight();
    }

    // If you want continuous movement with buttons, use these for pointer events
    public void OnLeftButtonDown()
    {
        moveLeft = true;
    }

    public void OnLeftButtonUp()
    {
        moveLeft = false;
    }

    public void OnRightButtonDown()
    {
        moveRight = true;
    }

    public void OnRightButtonUp()
    {
        moveRight = false;
    }
}