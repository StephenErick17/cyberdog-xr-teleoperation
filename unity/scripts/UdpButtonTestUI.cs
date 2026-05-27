using UnityEngine;

public class UdpButtonTestUI : MonoBehaviour
{
    [SerializeField] private UdpCommandSender udpSender;

    // ===== MOVIMIENTO =====
    public void MoveForward()
    {
        Debug.Log("[UI] Botón Forward pulsado");
        udpSender.SendMove(1);
    }

    public void MoveBackward()
    {
        Debug.Log("[UI] Botón Back pulsado");
        udpSender.SendMove(2);
    }

    public void MoveLeft()
    {
        Debug.Log("[UI] Botón Left pulsado");
        udpSender.SendMove(3);
    }

    public void MoveRight()
    {
        Debug.Log("[UI] Botón Right pulsado");
        udpSender.SendMove(4);
    }

    public void StopMove()
    {
        Debug.Log("[UI] Botón Stop pulsado");
        udpSender.SendMove(0);
    }

    // ===== ACCIONES IMPORTANTES =====
    public void ActionStandUp()
    {
        Debug.Log("[UI] Acción StandUp -> código 1");
        udpSender.SendAction(1);
    }

    public void ActionLieDown()
    {
        Debug.Log("[UI] Acción LieDown -> código 2");
        udpSender.SendAction(2);
    }

    public void ActionSlowGait()
    {
        Debug.Log("[UI] Acción SlowGait -> código 4");
        udpSender.SendAction(4);
    }

    public void ActionNormalGait()
    {
        Debug.Log("[UI] Acción NormalGait -> código 5");
        udpSender.SendAction(5);
    }

    // ===== OPCIONAL =====
    public void ActionSit()
    {
        Debug.Log("[UI] Acción Sit -> código 3");
        udpSender.SendAction(3);
    }

    public void ActionJump()
    {
        Debug.Log("[UI] Acción Jump -> código 6");
        udpSender.SendAction(6);
    }
}
