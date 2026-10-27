
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class DraggableControl : MonoBehaviour,
//    IBeginDragHandler, IDragHandler, IEndDragHandler,
//    IPointerClickHandler
//{
//    [SerializeField] private ControlLayoutCustomizer customizer;

//    private RectTransform rectTransform;
//    private RectTransform parentRect;

//    private void Awake()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        parentRect = rectTransform.parent as RectTransform;
//    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        if (customizer != null && customizer.IsEditing)
//            customizer.SelectControl(rectTransform);
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        if (customizer != null && customizer.IsEditing)
//            customizer.SelectControl(rectTransform);
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        if (customizer == null || !customizer.IsEditing)
//            return;

//        if (parentRect == null)
//            return;

//        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            parentRect,
//            eventData.position,
//            eventData.pressEventCamera,
//            out Vector2 localPoint))
//        {
//            rectTransform.anchoredPosition = localPoint;
//        }
//    }

//    public void OnEndDrag(PointerEventData eventData) { }
//}


using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DraggableControl : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler,
    IDragHandler, IEndDragHandler
{
    [SerializeField] private ControlLayoutCustomizer customizer;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Canvas canvas;

    private Vector2 pointerOffset;
    private bool dragging;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (customizer == null || !customizer.IsEditing)
            return;

        customizer.SelectControl(rectTransform);

        if (parentRect != null &&
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            pointerOffset =
                localPoint - rectTransform.anchoredPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (customizer == null || !customizer.IsEditing)
            return;

        customizer.SelectControl(rectTransform);
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || customizer == null ||
            !customizer.IsEditing || parentRect == null)
            return;

        Camera eventCamera = eventData.pressEventCamera;

        if (canvas != null &&
            canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            eventCamera = null;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition =
                localPoint - pointerOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }
}
