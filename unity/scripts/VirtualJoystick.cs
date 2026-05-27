using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private RectTransform handle;

    [Header("Joystick Settings")]
    [SerializeField] private float maxRadius = 80f;
    [SerializeField] private float outputLimit = 0.54f;

    private RectTransform baseRect;
    private Canvas parentCanvas;
    private Camera uiCamera;

    private Vector2 inputNormalized = Vector2.zero;

    public Vector2 InputNormalized => inputNormalized;

    public float OutputX => inputNormalized.x * outputLimit;
    public float OutputY => inputNormalized.y * outputLimit;

    private void Awake()
    {
        baseRect = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = Camera.main;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputNormalized = Vector2.zero;

        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }
    }

    private void UpdateJoystick(PointerEventData eventData)
    {
        if (baseRect == null || handle == null)
            return;

        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                baseRect,
                eventData.position,
                uiCamera,
                out localPoint))
        {
            return;
        }

        Vector2 clamped = Vector2.ClampMagnitude(localPoint, maxRadius);
        handle.anchoredPosition = clamped;

        inputNormalized = clamped / maxRadius;
    }
}
