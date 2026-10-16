
using UnityEngine;

public class ShopItemPurchase : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private string itemName = "Upgrade";
    [SerializeField] private int cost = 100;

    public void BuyItem()
    {
        if (CoinManager.Instance == null)
        {
            Debug.LogError("CoinManager is missing from the scene!");
            return;
        }

        if (CoinManager.Instance.TrySpendCoins(cost))
        {
            Debug.Log("Purchased: " + itemName);

            // Add the actual upgrade effect here.
        }
    }
}
