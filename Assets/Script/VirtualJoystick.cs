
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField, Range(0.1f, 1f)]
    private float handleRange = 0.65f;

    public Vector2 Direction { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
            return;

        Vector2 radius = background.rect.size * 0.5f;

        Direction = new Vector2(
            radius.x > 0 ? localPoint.x / radius.x : 0,
            radius.y > 0 ? localPoint.y / radius.y : 0
        );

        Direction = Vector2.ClampMagnitude(Direction, 1f);

        handle.anchoredPosition = new Vector2(
            Direction.x * radius.x * handleRange,
            Direction.y * radius.y * handleRange
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}
