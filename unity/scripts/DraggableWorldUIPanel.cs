using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWorldUIPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Panel a mover")]
    [SerializeField] private RectTransform panelToMove;

    [Header("Opcional")]
    [SerializeField] private bool preserveZ = true;

    private Vector3 worldOffset;
    private float fixedZ;

    private void Awake()
    {
        if (panelToMove == null)
        {
            RectTransform rt = GetComponentInParent<RectTransform>();
            panelToMove = rt;
        }

        if (panelToMove != null)
        {
            fixedZ = panelToMove.position.z;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (panelToMove == null)
            return;

        Vector3 hitPoint = GetWorldPoint(eventData);
        worldOffset = panelToMove.position - hitPoint;
        fixedZ = panelToMove.position.z;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (panelToMove == null)
            return;

        Vector3 hitPoint = GetWorldPoint(eventData);
        Vector3 targetPos = hitPoint + worldOffset;

        if (preserveZ)
            targetPos.z = fixedZ;

        panelToMove.position = targetPos;
    }

    private Vector3 GetWorldPoint(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.isValid)
            return eventData.pointerCurrentRaycast.worldPosition;

        return panelToMove.position;
    }
}
