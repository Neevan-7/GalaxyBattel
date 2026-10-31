
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ControlLayoutCustomizer : MonoBehaviour
//{
//    [Header("Controls to Customize")]
//    [SerializeField] private RectTransform[] controls;

//    [Header("Customization UI")]
//    [SerializeField] private GameObject customizationPanel;
//    [SerializeField] private Slider sizeSlider;
//    [SerializeField] private Slider opacitySlider;

//    public bool IsEditing { get; private set; }

//    private RectTransform selectedControl;
//    private readonly Dictionary<RectTransform, LayoutData> original
//        = new Dictionary<RectTransform, LayoutData>();

//    [Serializable]
//    private class LayoutData
//    {
//        public string controlName;
//        public float x, y, width, height, alpha;
//    }

//    [Serializable]
//    private class LayoutCollection
//    {
//        public List<LayoutData> items = new List<LayoutData>();
//    }

//    private const string SaveKey = "GD_CustomControlLayout";

//    private void Start()
//    {
//        if (customizationPanel != null)
//            customizationPanel.SetActive(false);

//        LoadLayout();
//    }

//    public void EnterEditMode()
//    {
//        original.Clear();

//        foreach (RectTransform control in controls)
//        {
//            if (control == null) continue;

//            original[control] = Capture(control);
//        }

//        IsEditing = true;
//        SetJoystickInput(false);

//        if (customizationPanel != null)
//            customizationPanel.SetActive(true);

//        SelectControl(controls.Length > 0 ? controls[0] : null);
//    }

//    //public void SelectControl(RectTransform control)
//    //{
//    //    if (!IsEditing || control == null) return;

//    //    selectedControl = control;

//    //    if (sizeSlider != null)
//    //    {
//    //        sizeSlider.SetValueWithoutNotify(
//    //            (control.sizeDelta.x + control.sizeDelta.y) / 2f);
//    //    }

//    //    CanvasGroup group = control.GetComponent<CanvasGroup>();

//    //    if (opacitySlider != null)
//    //    {
//    //        opacitySlider.SetValueWithoutNotify(
//    //            group != null ? group.alpha : 1f);
//    //    }
//    //}


//    private Vector2 selectedOriginalSize;
//    private Vector3 selectedOriginalScale;

//    public void SelectControl(RectTransform control)
//    {
//        if (!IsEditing || control == null) return;

//        selectedControl = control;

//        selectedOriginalSize = control.sizeDelta;
//        selectedOriginalScale = control.localScale;

//        if (sizeSlider != null)
//            sizeSlider.SetValueWithoutNotify(100f);

//        CanvasGroup group = control.GetComponent<CanvasGroup>();

//        if (opacitySlider != null)
//            opacitySlider.SetValueWithoutNotify(
//                group != null ? group.alpha : 1f);
//    }

//    public void OnSizeChanged(float percentage)
//    {
//        if (!IsEditing || selectedControl == null) return;

//        float multiplier = percentage / 100f;

//        selectedControl.localScale =
//            selectedOriginalScale * multiplier;
//    }


//    private void SetJoystickInput(bool enabled)
//    {
//        foreach (RectTransform control in controls)
//        {
//            if (control == null) continue;

//            VirtualJoystick joystick =
//                control.GetComponent<VirtualJoystick>();

//            if (joystick != null)
//                joystick.enabled = enabled;
//        }
//    }


//    //public void OnSizeChanged(float size)
//    //{
//    //    if (!IsEditing || selectedControl == null) return;

//    //    float ratio = selectedControl.sizeDelta.x > 0
//    //        ? selectedControl.sizeDelta.y /
//    //          selectedControl.sizeDelta.x
//    //        : 1f;

//    //    selectedControl.sizeDelta = new Vector2(
//    //        size, size * ratio);
//    //}

//    public void OnOpacityChanged(float alpha)
//    {
//        if (!IsEditing || selectedControl == null) return;

//        CanvasGroup group =
//            selectedControl.GetComponent<CanvasGroup>();

//        if (group == null)
//            group = selectedControl.gameObject.AddComponent<CanvasGroup>();

//        group.alpha = alpha;
//    }

//    public void SaveLayout()
//    {
//        LayoutCollection collection = new LayoutCollection();

//        foreach (RectTransform control in controls)
//        {
//            if (control != null)
//                collection.items.Add(Capture(control));
//        }

//        PlayerPrefs.SetString(
//            SaveKey, JsonUtility.ToJson(collection));

//        PlayerPrefs.Save();
//        FinishEditing();
//    }

//    public void CancelEditing()
//    {
//        foreach (var pair in original)
//        {
//            if (pair.Key != null)
//                Apply(pair.Key, pair.Value);
//        }

//        FinishEditing();
//    }

//    public void ResetAllControls()
//    {
//        foreach (RectTransform control in controls)
//        {
//            if (control != null && original.ContainsKey(control))
//                Apply(control, original[control]);
//        }

//        SelectControl(selectedControl);
//    }

//    public void ResetSavedLayout()
//    {
//        PlayerPrefs.DeleteKey(SaveKey);
//        PlayerPrefs.Save();

//        foreach (RectTransform control in controls)
//        {
//            if (control != null && original.ContainsKey(control))
//                Apply(control, original[control]);
//        }
//    }

//    private void FinishEditing()
//    {
//        IsEditing = false;
//        SetJoystickInput(true);
//        selectedControl = null;

//        if (customizationPanel != null)
//            customizationPanel.SetActive(false);
//    }

//    private LayoutData Capture(RectTransform control)
//    {
//        CanvasGroup group = control.GetComponent<CanvasGroup>();

//        return new LayoutData
//        {
//            controlName = control.name,
//            x = control.anchoredPosition.x,
//            y = control.anchoredPosition.y,
//            width = control.sizeDelta.x,
//            height = control.sizeDelta.y,
//            alpha = group != null ? group.alpha : 1f
//        };
//    }

//    private void Apply(RectTransform control, LayoutData data)
//    {
//        control.anchoredPosition = new Vector2(data.x, data.y);
//        control.sizeDelta = new Vector2(data.width, data.height);

//        CanvasGroup group = control.GetComponent<CanvasGroup>();

//        if (group == null)
//            group = control.gameObject.AddComponent<CanvasGroup>();

//        group.alpha = data.alpha;
//    }

//    private void LoadLayout()
//    {
//        if (!PlayerPrefs.HasKey(SaveKey)) return;

//        LayoutCollection collection = JsonUtility.FromJson<LayoutCollection>(
//            PlayerPrefs.GetString(SaveKey));

//        if (collection == null || collection.items == null) return;

//        foreach (RectTransform control in controls)
//        {
//            if (control == null) continue;

//            foreach (LayoutData data in collection.items)
//            {
//                if (data.controlName == control.name)
//                {
//                    Apply(control, data);
//                    break;
//                }
//            }
//        }
//    }
//}


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Lets the player move / resize / fade the on-screen mobile controls and saves the result.
/// Put this on one always-active object (e.g. ControlLayoutManager).
/// </summary>
public class ControlLayoutCustomizer : MonoBehaviour
{
    [Header("Controls to Customize (joystick, fire, shield, ability...)")]
    [SerializeField] private RectTransform[] controls;

    [Header("Customization UI")]
    [SerializeField] private GameObject customizationPanel;
    [SerializeField] private Slider sizeSlider;      // range is forced to 50-150 (%)
    [SerializeField] private Slider opacitySlider;   // range is forced to 0.3-1

    [Header("Limits")]
    [SerializeField] private float minSizePercent = 50f;
    [SerializeField] private float maxSizePercent = 150f;
    [SerializeField] private float minOpacity = 0.3f;

    public bool IsEditing { get; private set; }

    private RectTransform selectedControl;

    // Factory layout (captured once, before any saved layout is applied) -> used by RESET.
    private readonly Dictionary<RectTransform, LayoutData> defaults = new Dictionary<RectTransform, LayoutData>();
    // Layout when the edit screen was opened -> used by CANCEL.
    private readonly Dictionary<RectTransform, LayoutData> original = new Dictionary<RectTransform, LayoutData>();
    // Components switched off while editing so a tap/drag doesn't fire the ability or move the ship.
    private readonly List<Behaviour> disabledWhileEditing = new List<Behaviour>();

    [Serializable]
    private class LayoutData
    {
        public string controlName;
        public float x, y, scale, alpha;
    }

    [Serializable]
    private class LayoutCollection
    {
        public List<LayoutData> items = new List<LayoutData>();
    }

    // v2: new data format (scale instead of width/height). Old saves are ignored.
    private const string SaveKey = "GD_CustomControlLayout_v2";

    private void Awake()
    {
        ConfigureSlider(sizeSlider, minSizePercent, maxSizePercent, true);
        ConfigureSlider(opacitySlider, minOpacity, 1f, false);

        foreach (RectTransform control in controls)
        {
            if (control == null) continue;
            defaults[control] = Capture(control);
        }

        if (sizeSlider != null) sizeSlider.onValueChanged.AddListener(OnSizeChanged);
        if (opacitySlider != null) opacitySlider.onValueChanged.AddListener(OnOpacityChanged);
    }

    private void Start()
    {
        if (customizationPanel != null)
            customizationPanel.SetActive(false);

        LoadLayout();
    }

    private void OnDestroy()
    {
        if (sizeSlider != null) sizeSlider.onValueChanged.RemoveListener(OnSizeChanged);
        if (opacitySlider != null) opacitySlider.onValueChanged.RemoveListener(OnOpacityChanged);
    }

    private static void ConfigureSlider(Slider s, float min, float max, bool whole)
    {
        if (s == null) return;
        s.minValue = min;
        s.maxValue = max;
        s.wholeNumbers = whole;
    }

    // ????????????? Edit mode ?????????????

    /// <summary>Hook this to the "Custom Controller" button (or call it from ControlCustomizationNavigation).</summary>
    public void EnterEditMode()
    {
        original.Clear();
        foreach (RectTransform control in controls)
        {
            if (control == null) continue;
            original[control] = Capture(control);
        }

        IsEditing = true;
        SetInputComponentsEnabled(false);

        if (customizationPanel != null)
            customizationPanel.SetActive(true);

        SelectControl(controls.Length > 0 ? controls[0] : null);
    }

    public void SelectControl(RectTransform control)
    {
        if (!IsEditing || control == null) return;

        selectedControl = control;

        float baseScale = GetDefaultScale(control);

        if (sizeSlider != null)
            sizeSlider.SetValueWithoutNotify(
                Mathf.Clamp(control.localScale.x / baseScale * 100f, minSizePercent, maxSizePercent));

        if (opacitySlider != null)
        {
            CanvasGroup group = control.GetComponent<CanvasGroup>();
            opacitySlider.SetValueWithoutNotify(group != null ? group.alpha : 1f);
        }
    }

    // Size slider: value is a PERCENTAGE of the control's default size (100 = default).
    public void OnSizeChanged(float percentage)
    {
        if (!IsEditing || selectedControl == null) return;

        float s = GetDefaultScale(selectedControl) * (percentage / 100f);
        selectedControl.localScale = new Vector3(s, s, 1f);
    }

    public void OnOpacityChanged(float alpha)
    {
        if (!IsEditing || selectedControl == null) return;

        GetOrAddGroup(selectedControl).alpha = alpha;
    }

    // ????????????? Buttons ?????????????

    public void SaveLayout()
    {
        LayoutCollection collection = new LayoutCollection();
        foreach (RectTransform control in controls)
        {
            if (control != null) collection.items.Add(Capture(control));
        }

        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(collection));
        PlayerPrefs.Save();
        FinishEditing();
    }

    public void CancelEditing()
    {
        foreach (var pair in original)
        {
            if (pair.Key != null) Apply(pair.Key, pair.Value);
        }
        FinishEditing();
    }

    /// <summary>"RESET" button: put every control back to the factory layout (still needs Save to keep it).</summary>
    public void ResetAllControls()
    {
        foreach (var pair in defaults)
        {
            if (pair.Key != null) Apply(pair.Key, pair.Value);
        }
        SelectControl(selectedControl);
    }

    /// <summary>"RESET ALL" button: factory layout AND delete the saved layout.</summary>
    public void ResetSavedLayout()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
        ResetAllControls();
    }

    private void FinishEditing()
    {
        IsEditing = false;
        selectedControl = null;
        SetInputComponentsEnabled(true);

        if (customizationPanel != null)
            customizationPanel.SetActive(false);
    }

    // ????????????? Helpers ?????????????

    private void SetInputComponentsEnabled(bool enable)
    {
        if (enable)
        {
            foreach (Behaviour b in disabledWhileEditing)
                if (b != null) b.enabled = true;
            disabledWhileEditing.Clear();
            return;
        }

        foreach (RectTransform control in controls)
        {
            if (control == null) continue;

            foreach (VirtualJoystick j in control.GetComponentsInChildren<VirtualJoystick>(true))
                Disable(j);
            foreach (MobileFireButton f in control.GetComponentsInChildren<MobileFireButton>(true))
                Disable(f);
            foreach (Button b in control.GetComponentsInChildren<Button>(true))
                Disable(b);
        }
    }

    private void Disable(Behaviour b)
    {
        if (b != null && b.enabled)
        {
            b.enabled = false;
            disabledWhileEditing.Add(b);
        }
    }

    private float GetDefaultScale(RectTransform control)
    {
        if (defaults.TryGetValue(control, out LayoutData d) && d.scale > 0.0001f)
            return d.scale;
        return 1f;
    }

    private static CanvasGroup GetOrAddGroup(RectTransform control)
    {
        CanvasGroup g = control.GetComponent<CanvasGroup>();
        if (g == null) g = control.gameObject.AddComponent<CanvasGroup>();
        return g;
    }

    private LayoutData Capture(RectTransform control)
    {
        CanvasGroup group = control.GetComponent<CanvasGroup>();
        return new LayoutData
        {
            controlName = control.name,
            x = control.anchoredPosition.x,
            y = control.anchoredPosition.y,
            scale = control.localScale.x,
            alpha = group != null ? group.alpha : 1f
        };
    }

    private void Apply(RectTransform control, LayoutData data)
    {
        float s = data.scale > 0.0001f ? data.scale : 1f;

        control.anchoredPosition = new Vector2(data.x, data.y);
        control.localScale = new Vector3(s, s, 1f);
        GetOrAddGroup(control).alpha = Mathf.Clamp01(data.alpha);
    }

    private void LoadLayout()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        LayoutCollection collection = JsonUtility.FromJson<LayoutCollection>(PlayerPrefs.GetString(SaveKey));
        if (collection == null || collection.items == null) return;

        foreach (RectTransform control in controls)
        {
            if (control == null) continue;

            foreach (LayoutData data in collection.items)
            {
                if (data.controlName == control.name)
                {
                    Apply(control, data);
                    break;
                }
            }
        }
    }
}