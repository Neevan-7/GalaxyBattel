
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlLayoutCustomizer : MonoBehaviour
{
    [Header("Controls to Customize")]
    [SerializeField] private RectTransform[] controls;

    [Header("Customization UI")]
    [SerializeField] private GameObject customizationPanel;
    [SerializeField] private Slider sizeSlider;
    [SerializeField] private Slider opacitySlider;

    public bool IsEditing { get; private set; }

    private RectTransform selectedControl;
    private readonly Dictionary<RectTransform, LayoutData> original
        = new Dictionary<RectTransform, LayoutData>();

    [Serializable]
    private class LayoutData
    {
        public string controlName;
        public float x, y, width, height, alpha;
    }

    [Serializable]
    private class LayoutCollection
    {
        public List<LayoutData> items = new List<LayoutData>();
    }

    private const string SaveKey = "GD_CustomControlLayout";

    private void Start()
    {
        if (customizationPanel != null)
            customizationPanel.SetActive(false);

        LoadLayout();
    }

    public void EnterEditMode()
    {
        original.Clear();

        foreach (RectTransform control in controls)
        {
            if (control == null) continue;

            original[control] = Capture(control);
        }

        IsEditing = true;
        SetJoystickInput(false);

        if (customizationPanel != null)
            customizationPanel.SetActive(true);

        SelectControl(controls.Length > 0 ? controls[0] : null);
    }

    //public void SelectControl(RectTransform control)
    //{
    //    if (!IsEditing || control == null) return;

    //    selectedControl = control;

    //    if (sizeSlider != null)
    //    {
    //        sizeSlider.SetValueWithoutNotify(
    //            (control.sizeDelta.x + control.sizeDelta.y) / 2f);
    //    }

    //    CanvasGroup group = control.GetComponent<CanvasGroup>();

    //    if (opacitySlider != null)
    //    {
    //        opacitySlider.SetValueWithoutNotify(
    //            group != null ? group.alpha : 1f);
    //    }
    //}


    private Vector2 selectedOriginalSize;
    private Vector3 selectedOriginalScale;

    public void SelectControl(RectTransform control)
    {
        if (!IsEditing || control == null) return;

        selectedControl = control;

        selectedOriginalSize = control.sizeDelta;
        selectedOriginalScale = control.localScale;

        if (sizeSlider != null)
            sizeSlider.SetValueWithoutNotify(100f);

        CanvasGroup group = control.GetComponent<CanvasGroup>();

        if (opacitySlider != null)
            opacitySlider.SetValueWithoutNotify(
                group != null ? group.alpha : 1f);
    }

    public void OnSizeChanged(float percentage)
    {
        if (!IsEditing || selectedControl == null) return;

        float multiplier = percentage / 100f;

        selectedControl.localScale =
            selectedOriginalScale * multiplier;
    }


    private void SetJoystickInput(bool enabled)
    {
        foreach (RectTransform control in controls)
        {
            if (control == null) continue;

            VirtualJoystick joystick =
                control.GetComponent<VirtualJoystick>();

            if (joystick != null)
                joystick.enabled = enabled;
        }
    }


    //public void OnSizeChanged(float size)
    //{
    //    if (!IsEditing || selectedControl == null) return;

    //    float ratio = selectedControl.sizeDelta.x > 0
    //        ? selectedControl.sizeDelta.y /
    //          selectedControl.sizeDelta.x
    //        : 1f;

    //    selectedControl.sizeDelta = new Vector2(
    //        size, size * ratio);
    //}

    public void OnOpacityChanged(float alpha)
    {
        if (!IsEditing || selectedControl == null) return;

        CanvasGroup group =
            selectedControl.GetComponent<CanvasGroup>();

        if (group == null)
            group = selectedControl.gameObject.AddComponent<CanvasGroup>();

        group.alpha = alpha;
    }

    public void SaveLayout()
    {
        LayoutCollection collection = new LayoutCollection();

        foreach (RectTransform control in controls)
        {
            if (control != null)
                collection.items.Add(Capture(control));
        }

        PlayerPrefs.SetString(
            SaveKey, JsonUtility.ToJson(collection));

        PlayerPrefs.Save();
        FinishEditing();
    }

    public void CancelEditing()
    {
        foreach (var pair in original)
        {
            if (pair.Key != null)
                Apply(pair.Key, pair.Value);
        }

        FinishEditing();
    }

    public void ResetAllControls()
    {
        foreach (RectTransform control in controls)
        {
            if (control != null && original.ContainsKey(control))
                Apply(control, original[control]);
        }

        SelectControl(selectedControl);
    }

    public void ResetSavedLayout()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();

        foreach (RectTransform control in controls)
        {
            if (control != null && original.ContainsKey(control))
                Apply(control, original[control]);
        }
    }

    private void FinishEditing()
    {
        IsEditing = false;
        SetJoystickInput(true);
        selectedControl = null;

        if (customizationPanel != null)
            customizationPanel.SetActive(false);
    }

    private LayoutData Capture(RectTransform control)
    {
        CanvasGroup group = control.GetComponent<CanvasGroup>();

        return new LayoutData
        {
            controlName = control.name,
            x = control.anchoredPosition.x,
            y = control.anchoredPosition.y,
            width = control.sizeDelta.x,
            height = control.sizeDelta.y,
            alpha = group != null ? group.alpha : 1f
        };
    }

    private void Apply(RectTransform control, LayoutData data)
    {
        control.anchoredPosition = new Vector2(data.x, data.y);
        control.sizeDelta = new Vector2(data.width, data.height);

        CanvasGroup group = control.GetComponent<CanvasGroup>();

        if (group == null)
            group = control.gameObject.AddComponent<CanvasGroup>();

        group.alpha = data.alpha;
    }

    private void LoadLayout()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        LayoutCollection collection = JsonUtility.FromJson<LayoutCollection>(
            PlayerPrefs.GetString(SaveKey));

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
