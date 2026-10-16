
using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    [SerializeField] private int coinValue = 10;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            CoinManager.AddCoins(coinValue);

            Destroy(gameObject);
        }
    }
}
