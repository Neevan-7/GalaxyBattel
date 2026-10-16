
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class ShipSpecUI : MonoBehaviour
//{
//    [Header("Ship Index: Ship 1 = 0, Ship 2 = 1, etc.")]
//    public int shipIndex;

//    [Header("Sliders")]
//    public Slider healthSlider;
//    public Slider speedSlider;
//    public Slider damageSlider;
//    public Slider fireRateSlider;

//    [Header("Value Text")]
//    public TMP_Text healthText;
//    public TMP_Text speedText;
//    public TMP_Text damageText;
//    public TMP_Text fireRateText;

//    public void DisplayStats(
//        float health, float speed, float damage, float fireRate)
//    {
//        SetStat(healthSlider, healthText, health);
//        SetStat(speedSlider, speedText, speed);
//        SetStat(damageSlider, damageText, damage);
//        SetStat(fireRateSlider, fireRateText, fireRate);
//    }

//    private void SetStat(
//        Slider slider, TMP_Text label, float value)
//    {
//        if (slider != null)
//        {
//            slider.minValue = 0f;
//            slider.maxValue = Mathf.Max(slider.maxValue, value);
//            slider.value = value;
//        }

//        if (label != null)
//            label.text = Mathf.RoundToInt(value).ToString();
//    }
//}


using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSpecUI : MonoBehaviour
{
    [Header("Ship Index: Ship 1 = 0, Ship 2 = 1 ... Ship 6 = 5")]
    [Range(0, 5)]
    public int shipIndex;

    [Header("Sliders")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider damageSlider;
    [SerializeField] private Slider fireRateSlider;

    [Header("Value Text")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text fireRateText;

    public void DisplayStats(
        float health,
        float speed,
        float damage,
        float fireRate)
    {
        SetStat(healthSlider, healthText, health);
        SetStat(speedSlider, speedText, speed);
        SetStat(damageSlider, damageText, damage);
        SetStat(fireRateSlider, fireRateText, fireRate);
    }

    private void SetStat(Slider slider, TMP_Text label, float value)
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = Mathf.Max(1f, value);
            slider.value = value;
        }

        if (label != null)
        {
            label.text = Mathf.RoundToInt(value).ToString("D3");
        }
    }
}
