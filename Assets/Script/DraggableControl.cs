
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


//using UnityEngine;
//using UnityEngine.EventSystems;

//[RequireComponent(typeof(RectTransform))]
//public class DraggableControl : MonoBehaviour,
//    IPointerDownHandler, IBeginDragHandler,
//    IDragHandler, IEndDragHandler
//{
//    [SerializeField] private ControlLayoutCustomizer customizer;

//    private RectTransform rectTransform;
//    private RectTransform parentRect;
//    private Canvas canvas;

//    private Vector2 pointerOffset;
//    private bool dragging;

//    private void Awake()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        parentRect = rectTransform.parent as RectTransform;
//        canvas = GetComponentInParent<Canvas>();
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        if (customizer == null || !customizer.IsEditing)
//            return;

//        customizer.SelectControl(rectTransform);

//        if (parentRect != null &&
//            RectTransformUtility.ScreenPointToLocalPointInRectangle(
//                parentRect,
//                eventData.position,
//                eventData.pressEventCamera,
//                out Vector2 localPoint))
//        {
//            pointerOffset =
//                localPoint - rectTransform.anchoredPosition;
//        }
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        if (customizer == null || !customizer.IsEditing)
//            return;

//        customizer.SelectControl(rectTransform);
//        dragging = true;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        if (!dragging || customizer == null ||
//            !customizer.IsEditing || parentRect == null)
//            return;

//        Camera eventCamera = eventData.pressEventCamera;

//        if (canvas != null &&
//            canvas.renderMode == RenderMode.ScreenSpaceOverlay)
//        {
//            eventCamera = null;
//        }

//        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            parentRect,
//            eventData.position,
//            eventCamera,
//            out Vector2 localPoint))
//        {
//            rectTransform.anchoredPosition =
//                localPoint - pointerOffset;
//        }
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        dragging = false;
//    }
//}

using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Put this on every control listed in ControlLayoutCustomizer.controls.
/// Works only while customizer.IsEditing is true.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class DraggableControl : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ControlLayoutCustomizer customizer;   // auto-found if left empty
    [SerializeField] private bool clampToParent = true;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Canvas canvas;
    private Vector2 pointerOffset;
    private bool dragging;
    private readonly Vector3[] corners = new Vector3[4];

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;
        canvas = GetComponentInParent<Canvas>();

        if (customizer == null)
            customizer = FindFirstObjectByType<ControlLayoutCustomizer>(FindObjectsInactive.Include);
    }

    private bool Editing => customizer != null && customizer.IsEditing;

    private Camera EventCamera(PointerEventData e)
    {
        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay) return null;
        return e.pressEventCamera != null ? e.pressEventCamera : canvas != null ? canvas.worldCamera : null;
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (!Editing) return;

        customizer.SelectControl(rectTransform);

        if (parentRect != null &&
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect, e.position, EventCamera(e), out Vector2 local))
        {
            pointerOffset = local - rectTransform.anchoredPosition;
        }
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (!Editing) return;
        customizer.SelectControl(rectTransform);
        dragging = true;
    }

    public void OnDrag(PointerEventData e)
    {
        if (!dragging || !Editing || parentRect == null) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect, e.position, EventCamera(e), out Vector2 local))
        {
            rectTransform.anchoredPosition = local - pointerOffset;
            if (clampToParent) ClampToParent();
        }
    }

    public void OnEndDrag(PointerEventData e) => dragging = false;

    private void ClampToParent()
    {
        rectTransform.GetWorldCorners(corners);
        Vector3 min = parentRect.InverseTransformPoint(corners[0]);
        Vector3 max = parentRect.InverseTransformPoint(corners[2]);
        Rect pr = parentRect.rect;
        Vector2 shift = Vector2.zero;

        if (min.x < pr.xMin) shift.x = pr.xMin - min.x;
        else if (max.x > pr.xMax) shift.x = pr.xMax - max.x;

        if (min.y < pr.yMin) shift.y = pr.yMin - min.y;
        else if (max.y > pr.yMax) shift.y = pr.yMax - max.y;

        rectTransform.anchoredPosition += shift;
    }
}