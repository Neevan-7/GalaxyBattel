
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectionController : MonoBehaviour
{
    [System.Serializable]
    public class ShipData
    {
        public string shipName;
        public GameObject shipPanel;
        public Sprite shipSprite;
        [Min(0)] public int price;

        [Header("Optional UI")]
        public Image shipImage;
        public TMP_Text shipNameText;
        public TMP_Text priceText;
        public Button buyButton;
        public Button equipButton;
        public TMP_Text buyButtonText;
        public TMP_Text equipButtonText;
    }

    private const string EquippedKey = "GD_EquippedShip";

    [Header("Six Ships - Keep Their Order")]
    [SerializeField] private ShipData[] ships = new ShipData[6];
    [SerializeField] private ShipUpgradeManager upgradeManager;

    private int currentIndex;

    private string OwnedKey(int index)
    {
        return "GD_ShipOwned_" + index;
    }

    private void Start()
    {
        if (ships == null || ships.Length == 0)
        {
            Debug.LogError("Add your ship panels first.");
            return;
        }

        // Unlock the first ship for new players.
        if (PlayerPrefs.GetInt(OwnedKey(0), 0) == 0)
        {
            PlayerPrefs.SetInt(OwnedKey(0), 1);
            PlayerPrefs.Save();
        }

        currentIndex = Mathf.Clamp(
            PlayerPrefs.GetInt(EquippedKey, 0),
            0,
            ships.Length - 1
        );

        // Start by showing the first ship.
        //currentIndex = 0;
        RefreshShips();
    }

    public void NextShip()
    {
        if (!HasShips()) return;

        currentIndex = (currentIndex + 1) % ships.Length;
        RefreshShips();
    }

    public void PreviousShip()
    {
        if (!HasShips()) return;

        currentIndex =
            (currentIndex - 1 + ships.Length) % ships.Length;

        RefreshShips();
    }

    public void BuySelectedShip()
    {
        if (!HasShips()) return;

        ShipData ship = ships[currentIndex];

        if (IsOwned(currentIndex))
        {
            RefreshShips();
            return;
        }

        if (CoinManager.Instance == null)
        {
            Debug.LogError("CoinManager is missing.");
            return;
        }

        if (!CoinManager.Instance.TrySpendCoins(ship.price))
        {
            Debug.Log("Not enough coins to buy " + ship.shipName);
            return;
        }

        PlayerPrefs.SetInt(OwnedKey(currentIndex), 1);
        PlayerPrefs.Save();

        Debug.Log("Purchased " + ship.shipName);
        RefreshShips();
    }

    public void EquipSelectedShip()
    {
        if (!HasShips()) return;

        if (!IsOwned(currentIndex))
        {
            Debug.Log("Buy this ship before equipping it.");
            return;
        }

        PlayerPrefs.SetInt(EquippedKey, currentIndex);
        PlayerPrefs.Save();

        Debug.Log("Equipped " + ships[currentIndex].shipName);
        RefreshShips();
    }

    private bool IsOwned(int index)
    {
        return index == 0 ||
               PlayerPrefs.GetInt(OwnedKey(index), 0) == 1;
    }

    private bool HasShips()
    {
        if (ships == null || ships.Length == 0)
            return false;

        foreach (ShipData ship in ships)
        {
            if (ship == null || ship.shipPanel == null)
            {
                Debug.LogError(
                    "Assign every ship panel in Ship Selection Controller."
                );
                return false;
            }
        }

        return true;
    }

    private void RefreshShips()
    {
        if (!HasShips()) return;

        for (int i = 0; i < ships.Length; i++)
        {
            ShipData ship = ships[i];
            bool selected = i == currentIndex;
            bool owned = IsOwned(i);
            bool equipped =
                PlayerPrefs.GetInt(EquippedKey, 0) == i;

            ship.shipPanel.SetActive(selected);

            //if (selected && upgradeManager != null)
            //{
            //    upgradeManager.SetSelectedShip(i);
            //}

            if (!selected) continue;

            if (ship.shipImage != null)
                ship.shipImage.sprite = ship.shipSprite;

            if (ship.shipNameText != null)
                ship.shipNameText.text = ship.shipName;

            if (ship.priceText != null)
                ship.priceText.text = owned
                    ? "OWNED"
                    : ship.price + " ";

            if (ship.buyButton != null)
                ship.buyButton.gameObject.SetActive(!owned);

            if (ship.buyButtonText != null)
                ship.buyButtonText.text = "BUY";

            if (ship.equipButton != null)
            {
                ship.equipButton.gameObject.SetActive(owned);
                ship.equipButton.interactable = !equipped;
            }

            if (ship.equipButtonText != null)
                ship.equipButtonText.text =
                    equipped ? "EQUIPPED" : "EQUIP";
        }
        if (upgradeManager != null)
        {
            upgradeManager.RefreshAllShipSpecs();
        }
    }
}
