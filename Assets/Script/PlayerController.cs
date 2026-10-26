
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private VirtualJoystick movementJoystick;

    [Header("Mobile Aiming (Optional)")]
    [SerializeField] private VirtualJoystick aimJoystick;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Camera mainCamera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver)
        {
            moveInput = Vector2.zero;
            return;
        }

        // 1. Read movement input.
        if (Application.isMobilePlatform &&
            movementJoystick != null)
        {
            moveInput = movementJoystick.Direction;
        }
        else
        {
            moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;
        }

        // 2. Aim with the right joystick on mobile.
        if (Application.isMobilePlatform &&
            aimJoystick != null &&
            aimJoystick.Direction.sqrMagnitude > 0.01f)
        {
            RotateShip(aimJoystick.Direction);
        }
        // 3. Aim toward the mouse on PC.
        else if (!Application.isMobilePlatform &&
                 mainCamera != null)
        {
            Vector3 mouseWorld =
                mainCamera.ScreenToWorldPoint(
                    Input.mousePosition);

            Vector2 direction =
                (Vector2)(mouseWorld - transform.position);

            if (direction.sqrMagnitude > 0.001f)
            {
                RotateShip(direction);
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void RotateShip(Vector2 direction)
    {
        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg - 90f;

        rb.MoveRotation(angle);
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
