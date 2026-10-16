
using UnityEngine;

public class BlastEffect : MonoBehaviour
{
    [Header("Blast Settings")]
    [SerializeField] private Sprite[] blastFrames;
    [SerializeField] private float frameRate = 0.08f;
    [SerializeField] private float destroyAfter = 0.8f;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (blastFrames == null || blastFrames.Length == 0)
        {
            Destroy(gameObject, destroyAfter);
            return;
        }

        spriteRenderer.sprite = blastFrames[0];
        Destroy(gameObject, destroyAfter);
    }

    private void Update()
    {
        if (blastFrames == null || blastFrames.Length <= 1)
            return;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= blastFrames.Length)
            {
                Destroy(gameObject);
                return;
            }

            spriteRenderer.sprite = blastFrames[currentFrame];
        }
    }
}
