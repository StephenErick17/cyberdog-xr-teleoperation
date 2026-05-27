using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CameraUdpReceiver : MonoBehaviour
{
    [Header("UDP Config")]
    [SerializeField] private int listenPort = 5006;

    [Header("UI")]
    [SerializeField] private RawImage targetRawImage;

    [Header("Texture Settings")]
    [SerializeField] private int initialWidth = 320;
    [SerializeField] private int initialHeight = 240;

    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isRunning = false;

    private byte[] latestImageBytes;
    private readonly object imageLock = new object();

    private Texture2D texture;

    private void Start()
    {
        if (targetRawImage == null)
        {
            Debug.LogError("[CAMERA] targetRawImage no asignado.");
            return;
        }

        texture = new Texture2D(initialWidth, initialHeight, TextureFormat.RGB24, false);
        targetRawImage.texture = texture;

        StartReceiver();
    }

    private void StartReceiver()
    {
        try
        {
            udpClient = new UdpClient(listenPort);
            isRunning = true;

            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log($"[CAMERA] Escuchando UDP en puerto {listenPort}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[CAMERA] Error iniciando receptor UDP: {e}");
        }
    }

    private void ReceiveLoop()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, listenPort);

        while (isRunning)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);

                lock (imageLock)
                {
                    latestImageBytes = data;
                }
            }
            catch (SocketException)
            {
                // normal al cerrar
            }
            catch (Exception e)
            {
                Debug.LogError($"[CAMERA] Error recibiendo imagen UDP: {e}");
            }
        }
    }

    private void Update()
    {
        byte[] imageData = null;

        lock (imageLock)
        {
            if (latestImageBytes != null)
            {
                imageData = latestImageBytes;
                latestImageBytes = null;
            }
        }

        if (imageData != null)
        {
            bool loaded = texture.LoadImage(imageData);
            if (loaded)
            {
                targetRawImage.texture = texture;
            }
        }
    }

    private void OnDestroy()
    {
        isRunning = false;

        try
        {
            udpClient?.Close();
        }
        catch { }

        try
        {
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Join(200);
            }
        }
        catch { }
    }
}
