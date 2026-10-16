
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSpriteSwitcher : MonoBehaviour
{
    [Header("Player Sprites")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite movingSprite;

    [Header("Movement Detection")]
    [SerializeField] private float movementThreshold = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool isMoving =
            rb.linearVelocity.magnitude > movementThreshold;

        if (isMoving && movingSprite != null)
        {
            spriteRenderer.sprite = movingSprite;
        }
        else if (!isMoving && idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }
}
