
using UnityEngine;

public class PlayerShipLoader : MonoBehaviour
{
    [System.Serializable]
    public class ShipSpriteEntry
    {
        public string shipName;
        public Sprite shipSprite;
    }

    [Header("Player Visual")]
    [SerializeField] private SpriteRenderer playerRenderer;

    [Header("Ships - Same Order As Shop")]
    [SerializeField] private ShipSpriteEntry[] shipSprites = new ShipSpriteEntry[6];

    private const string EquippedKey = "GD_EquippedShip";

    private void Start()
    {
        ApplyEquippedShip();
    }

    public void ApplyEquippedShip()
    {
        if (playerRenderer == null)
        {
            Debug.LogError("PlayerShipLoader: Player SpriteRenderer is missing.");
            return;
        }

        if (shipSprites == null || shipSprites.Length == 0)
        {
            Debug.LogError("PlayerShipLoader: Assign your ship sprites.");
            return;
        }

        int index = PlayerPrefs.GetInt(EquippedKey, 0);

        if (index < 0 || index >= shipSprites.Length)
        {
            Debug.LogWarning("Invalid equipped ship index. Using the starter ship.");
            index = 0;
        }

        if (shipSprites[index] == null ||
            shipSprites[index].shipSprite == null)
        {
            Debug.LogError("Missing ship sprite at index " + index);
            return;
        }

        playerRenderer.sprite = shipSprites[index].shipSprite;

        Debug.Log("Gameplay ship applied: " + shipSprites[index].shipName);
    }
}
