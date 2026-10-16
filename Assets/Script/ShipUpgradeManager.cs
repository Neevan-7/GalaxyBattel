
//using UnityEngine;

//public class ShipUpgradeManager : MonoBehaviour
//{
//    [Header("Ship Selection")]
//    [SerializeField] private ShipSelectionController shipSelection;

//    [Header("Base Stats")]
//    [SerializeField] private float baseHealth = 100f;
//    [SerializeField] private float baseSpeed = 120f;
//    [SerializeField] private float baseDamage = 80f;
//    [SerializeField] private float baseFireRate = 150f;

//    [Header("Upgrade Prices")]
//    [SerializeField] private int damagePrice = 100;
//    [SerializeField] private int healthPrice = 150;
//    [SerializeField] private int fireRatePrice = 200;

//    [Header("Upgrade Settings")]
//    [SerializeField] private int maxUpgradeLevel = 5;

//    private ShipSpecUI[] shipUIs;
//    private int selectedShipIndex;

//    private void Awake()
//    {
//        shipUIs = FindObjectsByType<ShipSpecUI>(
//            FindObjectsSortMode.None);
//    }

//    private void Start()
//    {
//        RefreshAllShipSpecs();
//    }

//    public void SetSelectedShip(int index)
//    {
//        selectedShipIndex = Mathf.Max(0, index);
//        RefreshSelectedShipSpec();
//    }

//    public void BuyDamageUpgrade()
//    {
//        int level = GetLevel("Damage");

//        if (level >= maxUpgradeLevel)
//        {
//            Debug.Log("Damage upgrade is at maximum level.");
//            return;
//        }

//        if (!SpendCoins(damagePrice))
//            return;

//        SaveLevel("Damage", level + 1);
//        RefreshSelectedShipSpec();

//        Debug.Log("Ship " + (selectedShipIndex + 1) +
//                  " Damage upgraded.");
//    }

//    public void BuyHealthUpgrade()
//    {
//        int level = GetLevel("Health");

//        if (level >= maxUpgradeLevel)
//        {
//            Debug.Log("Health upgrade is at maximum level.");
//            return;
//        }

//        if (!SpendCoins(healthPrice))
//            return;

//        SaveLevel("Health", level + 1);
//        RefreshSelectedShipSpec();

//        Debug.Log("Ship " + (selectedShipIndex + 1) +
//                  " Health upgraded.");
//    }

//    public void BuyFireRateUpgrade()
//    {
//        int level = GetLevel("FireRate");

//        if (level >= maxUpgradeLevel)
//        {
//            Debug.Log("Fire Rate upgrade is at maximum level.");
//            return;
//        }

//        if (!SpendCoins(fireRatePrice))
//            return;

//        SaveLevel("FireRate", level + 1);
//        RefreshSelectedShipSpec();

//        Debug.Log("Ship " + (selectedShipIndex + 1) +
//                  " Fire Rate upgraded.");
//    }

//    private string UpgradeKey(string stat, int shipIndex)
//    {
//        return "GD_Ship_" + shipIndex + "_" + stat + "Level";
//    }

//    private int GetLevel(string stat)
//    {
//        return PlayerPrefs.GetInt(
//            UpgradeKey(stat, selectedShipIndex), 0);
//    }

//    private void SaveLevel(string stat, int level)
//    {
//        PlayerPrefs.SetInt(
//            UpgradeKey(stat, selectedShipIndex), level);

//        PlayerPrefs.Save();
//    }

//    private bool SpendCoins(int amount)
//    {
//        if (CoinManager.Instance == null)
//        {
//            Debug.LogError("CoinManager is missing.");
//            return false;
//        }

//        if (!CoinManager.Instance.TrySpendCoins(amount))
//        {
//            Debug.Log("Not enough coins.");
//            return false;
//        }

//        return true;
//    }

//    private void RefreshAllShipSpecs()
//    {
//        if (shipUIs == null)
//            return;

//        foreach (ShipSpecUI ui in shipUIs)
//        {
//            if (ui != null)
//                RefreshShipSpec(ui);
//        }
//    }

//    private void RefreshSelectedShipSpec()
//    {
//        if (shipUIs == null)
//            return;

//        foreach (ShipSpecUI ui in shipUIs)
//        {
//            if (ui != null && ui.shipIndex == selectedShipIndex)
//                RefreshShipSpec(ui);
//        }
//    }

//    private void RefreshShipSpec(ShipSpecUI ui)
//    {
//        int index = ui.shipIndex;

//        int healthLevel = PlayerPrefs.GetInt(
//            UpgradeKey("Health", index), 0);

//        int damageLevel = PlayerPrefs.GetInt(
//            UpgradeKey("Damage", index), 0);

//        int fireRateLevel = PlayerPrefs.GetInt(
//            UpgradeKey("FireRate", index), 0);

//        float health = baseHealth * Mathf.Pow(1.25f, healthLevel);
//        float damage = baseDamage * Mathf.Pow(1.25f, damageLevel);
//        float fireRate = baseFireRate * Mathf.Pow(1.2f, fireRateLevel);

//        ui.DisplayStats(health, baseSpeed, damage, fireRate);
//    }
//}


using UnityEngine;

public class ShipUpgradeManager : MonoBehaviour
{
    [Header("Base Ship Stats")]
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float baseSpeed = 120f;
    [SerializeField] private float baseDamage = 80f;
    [SerializeField] private float baseFireRate = 150f;

    [Header("Upgrade Prices")]
    [SerializeField] private int damagePrice = 100;
    [SerializeField] private int healthPrice = 150;
    [SerializeField] private int fireRatePrice = 200;

    [Header("Upgrade Settings")]
    [SerializeField] private int maxUpgradeLevel = 5;
    [SerializeField] private float healthMultiplier = 1.25f;
    [SerializeField] private float damageMultiplier = 1.25f;
    [SerializeField] private float fireRateMultiplier = 1.2f;

    private const string EquippedKey = "GD_EquippedShip";

    private ShipSpecUI[] shipSpecs;

    private void Awake()
    {
        shipSpecs = FindObjectsByType<ShipSpecUI>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );
    }

    private void Start()
    {
        RefreshAllShipSpecs();
    }

    // The purchase target is ALWAYS the equipped ship.
    private int GetEquippedShipIndex()
    {
        int index = PlayerPrefs.GetInt(EquippedKey, 0);
        return Mathf.Clamp(index, 0, 5);
    }

    private string GetKey(int shipIndex, string stat)
    {
        return $"GD_Ship_{shipIndex}_{stat}Level";
    }

    private int GetLevel(int shipIndex, string stat)
    {
        return PlayerPrefs.GetInt(GetKey(shipIndex, stat), 0);
    }

    private void SetLevel(int shipIndex, string stat, int level)
    {
        PlayerPrefs.SetInt(GetKey(shipIndex, stat), level);
        PlayerPrefs.Save();
    }

    public void BuyDamageUpgrade()
    {
        BuyUpgrade("Damage", damagePrice);
    }

    public void BuyHealthUpgrade()
    {
        BuyUpgrade("Health", healthPrice);
    }

    public void BuyFireRateUpgrade()
    {
        BuyUpgrade("FireRate", fireRatePrice);
    }

    private void BuyUpgrade(string stat, int price)
    {
        int equippedIndex = GetEquippedShipIndex();
        int currentLevel = GetLevel(equippedIndex, stat);

        if (currentLevel >= maxUpgradeLevel)
        {
            Debug.Log($"{stat} upgrade is already at maximum level.");
            return;
        }

        if (CoinManager.Instance == null)
        {
            Debug.LogError("CoinManager.Instance was not found.");
            return;
        }

        if (!CoinManager.Instance.TrySpendCoins(price))
        {
            Debug.Log("Not enough coins for this upgrade.");
            return;
        }

        SetLevel(equippedIndex, stat, currentLevel + 1);

        RefreshAllShipSpecs();

        Debug.Log(
            $"{stat} upgraded for equipped ship index {equippedIndex}. " +
            $"New level: {currentLevel + 1}"
        );
    }

    public void RefreshAllShipSpecs()
    {
        // Re-scan in case the shop panels were instantiated later.
        shipSpecs = FindObjectsByType<ShipSpecUI>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (ShipSpecUI spec in shipSpecs)
        {
            if (spec == null)
                continue;

            int index = Mathf.Clamp(spec.shipIndex, 0, 5);

            int healthLevel = GetLevel(index, "Health");
            int damageLevel = GetLevel(index, "Damage");
            int fireRateLevel = GetLevel(index, "FireRate");

            float health = baseHealth *
                Mathf.Pow(healthMultiplier, healthLevel);

            float damage = baseDamage *
                Mathf.Pow(damageMultiplier, damageLevel);

            float fireRate = baseFireRate *
                Mathf.Pow(fireRateMultiplier, fireRateLevel);

            // Speed is displayed but not upgraded by these three buttons.
            spec.DisplayStats(health, baseSpeed, damage, fireRate);
        }
    }

    public int GetEquippedDamageLevel()
    {
        return GetLevel(GetEquippedShipIndex(), "Damage");
    }

    public int GetEquippedHealthLevel()
    {
        return GetLevel(GetEquippedShipIndex(), "Health");
    }

    public int GetEquippedFireRateLevel()
    {
        return GetLevel(GetEquippedShipIndex(), "FireRate");
    }
}
