
//using TMPro;
//using UnityEngine;

//public class CoinManager : MonoBehaviour
//{
//    public static CoinManager Instance { get; private set; }

//    private const string CoinsKey = "GalaxyDefender_Coins";

//    [SerializeField] private TMP_Text coinsText;

//    public int TotalCoins => PlayerPrefs.GetInt(CoinsKey, 0);

//    private void Awake()
//    {
//        Instance = this;
//        RefreshCoins();
//    }

//    private void OnEnable()
//    {
//        RefreshCoins();
//    }

//    public void RefreshCoins()
//    {
//        if (coinsText != null)
//        {
//            coinsText.text = TotalCoins.ToString("D5");
//        }
//    }

//    public static void AddCoins(int amount)
//    {
//        if (amount <= 0) return;

//        int currentCoins = PlayerPrefs.GetInt(CoinsKey, 0);
//        PlayerPrefs.SetInt(CoinsKey, currentCoins + amount);
//        PlayerPrefs.Save();

//        if (Instance != null)
//        {
//            Instance.RefreshCoins();
//        }
//    }

//    public bool TrySpendCoins(int amount)
//    {
//        if (amount <= 0 || TotalCoins < amount)
//        {
//            Debug.Log("Not enough coins!");
//            return false;
//        }

//        PlayerPrefs.SetInt(CoinsKey, TotalCoins - amount);
//        PlayerPrefs.Save();
//        RefreshCoins();

//        return true;
//    }
//}


using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private const string CoinsKey = "GalaxyDefender_Coins";

    [Header("Coin Displays")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text coinsTextIcon;

    public int TotalCoins => PlayerPrefs.GetInt(CoinsKey, 0);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        RefreshCoins();
    }

    private void OnEnable()
    {
        RefreshCoins();
    }

    public void RefreshCoins()
    {
        string balance = TotalCoins.ToString("D5");

        if (coinsText != null)
            coinsText.text = balance;

        if (coinsTextIcon != null)
            coinsTextIcon.text = balance;
    }

    public static void AddCoins(int amount)
    {
        if (amount <= 0)
            return;

        int total = PlayerPrefs.GetInt(CoinsKey, 0);
        PlayerPrefs.SetInt(CoinsKey, total + amount);
        PlayerPrefs.Save();

        if (Instance != null)
            Instance.RefreshCoins();
    }

    public bool TrySpendCoins(int amount)
    {
        if (amount <= 0 || TotalCoins < amount)
        {
            Debug.Log("Not enough coins!");
            return false;
        }

        PlayerPrefs.SetInt(CoinsKey, TotalCoins - amount);
        PlayerPrefs.Save();

        RefreshCoins();
        return true;
    }
}
