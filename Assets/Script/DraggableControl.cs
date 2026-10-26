
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableControl : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerClickHandler
{
    [SerializeField] private ControlLayoutCustomizer customizer;

    private RectTransform rectTransform;
    private RectTransform parentRect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (customizer != null && customizer.IsEditing)
            customizer.SelectControl(rectTransform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (customizer != null && customizer.IsEditing)
            customizer.SelectControl(rectTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (customizer == null || !customizer.IsEditing)
            return;

        if (parentRect == null)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData) { }
}
