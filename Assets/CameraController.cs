using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Targets")]
    private Transform player1; // The player
    private Transform player2; // The enemy

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0, 5, -10); // Default offset from midpoint
    public float followSpeed = 5f; // Speed at which camera follows the midpoint
    public float rotationSpeed = 3f; // Speed at which camera rotates to look at midpoint

    [Header("Zoom Settings")]
    public float minZoom = 5f; // Minimum zoom distance
    public float maxZoom = 20f; // Maximum zoom distance
    public float zoomSpeed = 2f; // Speed of zooming
    public float currentZoom = 10f; // Current zoom level

    private Camera cam;
    private Vector3 positionVelocity; // For smooth position
    private float rotationVelocity; // For smooth rotation

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraController must be attached to a GameObject with a Camera component.");
        }

        // Auto-find player by tag
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player1 = playerGO.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Tag it with 'Player'.");
        }

        // Auto-find enemy by tag
        GameObject enemyGO = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyGO != null)
        {
            player2 = enemyGO.transform;
        }
        else
        {
            Debug.LogWarning("Enemy not found. Tag it with 'Enemy'.");
        }
    }

    void Update()
    {
        // Calculate midpoint
        Vector3 midpoint = CalculateMidpoint();

        // Handle zoom input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scrollInput * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // Adjust offset based on zoom
        Vector3 adjustedOffset = offset.normalized * currentZoom;

        // Calculate desired position
        Vector3 desiredPosition = midpoint + adjustedOffset;

        // Smoothly move camera to desired position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, 0.1f, followSpeed);

        // Make camera look at midpoint with smooth rotation
        Quaternion desiredRotation = Quaternion.LookRotation(midpoint - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

    Vector3 CalculateMidpoint()
    {
        if (player1 != null && player2 != null)
        {
            return (player1.position + player2.position) / 2f;
        }
        else if (player1 != null)
        {
            return player1.position;
        }
        else if (player2 != null)
        {
            return player2.position;
        }
        else
        {
            Debug.LogWarning("No players assigned to CameraController.");
            return Vector3.zero;
        }
    }
}