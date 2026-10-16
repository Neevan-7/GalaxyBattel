
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

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
            return;
        }

        // Read keyboard movement.
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        // Rotate the ship toward the mouse.
        if (mainCamera != null)
        {
            Vector3 mouseWorld =
                mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction =
                (Vector2)(mouseWorld - transform.position);

            if (direction.sqrMagnitude > 0.001f)
            {
                // Assumes the ship sprite's nose points upward.
                float angle =
                    Mathf.Atan2(direction.y, direction.x)
                    * Mathf.Rad2Deg - 90f;

                transform.rotation =
                    Quaternion.Euler(0f, 0f, angle);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
