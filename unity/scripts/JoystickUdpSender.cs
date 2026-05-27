using System;
using System.Net.Sockets;
using System.Text;
using System.Globalization;
using UnityEngine;

public class JoystickUdpSender : MonoBehaviour
{
    [Header("Joystick References")]
    [SerializeField] private VirtualJoystick leftJoystick;
    [SerializeField] private VirtualJoystick rightJoystick;

    [Header("UDP Config")]
    [SerializeField] private string targetIP = "192.168.242.179";
    [SerializeField] private int targetPort = 5005;
    [SerializeField] private float sendInterval = 0.05f; // 20 Hz

    [Header("Thresholds")]
    [SerializeField] private float deadZone = 0.02f;

    [Header("Current Output")]
    [SerializeField] private float linearX;
    [SerializeField] private float linearY;
    [SerializeField] private float angularZ;

    private UdpClient udpClient;
    private float sendTimer = 0f;

    private bool stopAlreadySent = false;
    private string lastSentMessage = "";

    private void Awake()
    {
        InitializeClient();
    }

    private void InitializeClient()
    {
        if (udpClient != null)
            return;

        try
        {
            udpClient = new UdpClient();
            Debug.Log($"[UDP] Cliente joystick creado. Destino: {targetIP}:{targetPort}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[UDP] Error creando cliente joystick: {e}");
        }
    }

    private void Update()
    {
        if (leftJoystick == null || rightJoystick == null)
            return;

        // Leer joysticks
        linearX = ApplyDeadZone(leftJoystick.OutputY);
        angularZ = -ApplyDeadZone(leftJoystick.OutputX);
        linearY = -ApplyDeadZone(rightJoystick.OutputX);

        sendTimer += Time.deltaTime;
        if (sendTimer < sendInterval)
            return;

        sendTimer = 0f;

        bool isZeroCommand = Mathf.Abs(linearX) < deadZone &&
                             Mathf.Abs(linearY) < deadZone &&
                             Mathf.Abs(angularZ) < deadZone;

        // Caso 1: hay movimiento => enviar continuamente
        if (!isZeroCommand)
        {
            string msg = FormatCommand(linearX, linearY, angularZ);
            SendRaw(msg);
            stopAlreadySent = false;
            return;
        }

        // Caso 2: volvió al centro => enviar un solo cero
        if (isZeroCommand && !stopAlreadySent)
        {
            string msg = FormatCommand(0f, 0f, 0f);
            SendRaw(msg);
            stopAlreadySent = true;
            return;
        }

        // Caso 3: ya está en cero y ya mandamos el stop => no enviar nada
    }

    private float ApplyDeadZone(float value)
    {
        if (Mathf.Abs(value) < deadZone)
            return 0f;
        return value;
    }

    private string FormatCommand(float lx, float ly, float az)
    {
    	return string.Format(
        CultureInfo.InvariantCulture,
        "cmd:{0:F3},{1:F3},{2:F3}",
        lx, ly, az);
    }

    private void SendRaw(string message)
    {
        if (udpClient == null)
        {
            InitializeClient();
        }

        if (udpClient == null)
        {
            Debug.LogError("[UDP] No se pudo inicializar udpClient.");
            return;
        }

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, targetIP, targetPort);

            if (message != lastSentMessage)
            {
                Debug.Log($"[UDP-JOYSTICK] {message}");
                lastSentMessage = message;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[UDP] Error enviando joystick '{message}': {e}");
        }
    }

    private void OnDestroy()
    {
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
    }
}
