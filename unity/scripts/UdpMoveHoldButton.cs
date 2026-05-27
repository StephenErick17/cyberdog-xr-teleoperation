using UnityEngine;
using UnityEngine.EventSystems;

public class UdpMoveHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UdpCommandSender udpSender;
    [SerializeField] private int moveCode = 0;
    [SerializeField] private string debugName = "MoveButton";

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"[UI] {debugName} PointerDown -> move:{moveCode}");
        udpSender.SendMove(moveCode);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"[UI] {debugName} PointerUp -> move:0");
        udpSender.SendMove(0);
    }
}
