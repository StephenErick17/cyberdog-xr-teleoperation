using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UdpCommandSender : MonoBehaviour
{
    [Header("UDP Config")]
    [SerializeField] private string targetIP = "192.168.242.179";
    [SerializeField] private int targetPort = 5005;

    private UdpClient udpClient;

    private void Awake()
    {
        Debug.Log("[UDP] Awake() ejecutado.");
        InitializeClient();
    }

    private void Start()
    {
        Debug.Log("[UDP] Start() ejecutado.");
        InitializeClient();
    }

    private void OnEnable()
    {
        Debug.Log("[UDP] OnEnable() ejecutado.");
        InitializeClient();
    }

    private void InitializeClient()
    {
        if (udpClient != null)
        {
            Debug.Log("[UDP] Cliente ya inicializado.");
            return;
        }

        try
        {
            udpClient = new UdpClient();
            Debug.Log($"[UDP] Cliente creado correctamente. Destino: {targetIP}:{targetPort}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[UDP] Error al crear UdpClient: {e}");
        }
    }

    public void SendMove(int code)
    {
        Debug.Log($"[UDP] SendMove llamado con código: {code}");
        SendRaw($"move:{code}");
    }

    public void SendAction(int code)
    {
        Debug.Log($"[UDP] SendAction llamado con código: {code}");
        SendRaw($"action:{code}");
    }

    public void SendRaw(string message)
    {
        if (udpClient == null)
        {
            Debug.LogWarning("[UDP] udpClient era null. Intentando inicializar en caliente...");
            InitializeClient();
        }

        if (udpClient == null)
        {
            Debug.LogError("[UDP] No se pudo inicializar udpClient. Envío cancelado.");
            return;
        }

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            int sent = udpClient.Send(data, data.Length, targetIP, targetPort);
            Debug.Log($"[UDP] Enviado OK ({sent} bytes): {message} -> {targetIP}:{targetPort}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[UDP] Error al enviar '{message}': {e}");
        }
    }

    private void OnDisable()
    {
        Debug.Log("[UDP] OnDisable() ejecutado.");
    }

    private void OnDestroy()
    {
        Debug.Log("[UDP] OnDestroy() ejecutado.");
        CloseClient();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("[UDP] OnApplicationQuit() ejecutado.");
        CloseClient();
    }

    private void CloseClient()
    {
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
            Debug.Log("[UDP] Cliente cerrado.");
        }
    }
}
