\# IP Configuration



This document describes the IP configuration required to reproduce the augmented reality teleoperation architecture for Xiaomi CyberDog using Meta Quest 3, Unity, UDP communication, and ROS 2.



The system requires three devices connected to the same network:



```text

1\. Meta Quest 3

2\. Ubuntu Bridge PC

3\. Xiaomi CyberDog

```



The IP addresses may change depending on the active network. Therefore, the configuration must be verified before each experimental session.



\## Network topology



```text

Meta Quest 3 / Unity AR interface

&#x20;       ↓ UDP control packets

Ubuntu Bridge PC

&#x20;       ↓ ROS 2 communication

Xiaomi CyberDog

```



For visual feedback:



```text

Xiaomi CyberDog camera

&#x20;       ↓ ROS 2 image topics

Ubuntu Bridge PC

&#x20;       ↓ UDP video stream

Meta Quest 3 / Unity AR interface

```



\## Required IP addresses



The following addresses must be identified before running the system:



```text

Meta Quest 3 IP address

Ubuntu Bridge PC IP address

Xiaomi CyberDog IP address

```



\## Ubuntu Bridge PC IP



On the Ubuntu Bridge PC, check the active IP address with:



```bash

ip addr

```



or:



```bash

hostname -I

```



The bridge computer IP address must be configured in the Unity UDP sender scripts, because Unity sends control commands to this device through UDP port `5005`.



Relevant Unity scripts may include:



```text

unity/scripts/JoystickUdpSender.cs

unity/scripts/UdpCommandSender.cs

unity/scripts/UdpMoveHoldButton.cs

```



\## Meta Quest 3 IP



The Meta Quest 3 IP address is required by the camera UDP sender, because the Ubuntu Bridge PC sends the visual stream to the headset through UDP port `5006`.



The Quest IP address must be configured in:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



Example:



```python

self.quest\_ip = '192.168.242.18'

self.quest\_port = 5006

```



This value must be updated when the headset receives a different IP address.



\## Xiaomi CyberDog IP



The CyberDog must be reachable from the Ubuntu Bridge PC through the same network.



To verify connectivity, use:



```bash

ping <cyberdog\_ip>

```



Example:



```bash

ping 192.168.242.XX

```



The exact IP address depends on the active network configuration.



\## ROS 2 communication



After confirming basic network connectivity, verify that CyberDog ROS 2 topics are visible from the Ubuntu Bridge PC:



```bash

ros2 topic list | grep mi1036358

```



Expected topics include:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

/mi1036358/camera/color/image\_raw

/mi1036358/camera/depth/image\_rect\_raw

/mi1036358/camera/aligned\_depth\_to\_color/image\_raw

```



\## UDP communication summary



```text

Unity / Meta Quest 3

&#x20;       sends to

Ubuntu Bridge PC IP:5005



Ubuntu Bridge PC

&#x20;       sends to

Meta Quest 3 IP:5006

```



\## Recommended session checklist



Before each test, verify:



```text

1\. Meta Quest 3, Ubuntu Bridge PC, and CyberDog are connected to the same network.

2\. The Ubuntu Bridge PC IP address is correctly set in the Unity UDP sender scripts.

3\. The Meta Quest 3 IP address is correctly set in camera\_udp\_sender.py.

4\. UDP port 5005 is used for control commands.

5\. UDP port 5006 is used for camera streaming.

6\. CyberDog ROS 2 topics are visible from the bridge computer.

7\. The selected camera topic is active.

8\. No firewall rule is blocking UDP communication.

```



\## Troubleshooting



\### Unity commands do not reach the bridge



Check:



```text

\- The Ubuntu Bridge PC IP address in Unity.

\- UDP port 5005.

\- Whether udp\_to\_ros\_bridge.py is running.

\- Whether the Meta Quest 3 and Ubuntu Bridge PC are on the same network.

```



\### Camera video does not appear in Unity



Check:



```text

\- The Meta Quest 3 IP address in camera\_udp\_sender.py.

\- UDP port 5006.

\- Whether camera\_udp\_sender.py is running.

\- Whether Unity is listening on the correct video port.

\- Whether the selected ROS 2 camera topic is publishing frames.

```



\### ROS 2 topics are not visible



Check:



```text

\- CyberDog is powered on.

\- CyberDog and the Ubuntu Bridge PC are connected to the same network.

\- ROS 2 environment variables are correctly configured.

\- Cyclone DDS configuration is active when required.

\- The CyberDog message interfaces are sourced.

```



\## Notes



The reported experiments used a shared mobile wireless network with an available bandwidth of approximately 18.67 Mbps. Under different network conditions, IP addresses, latency, frame reception, jitter, and video stability may vary.



For more stable operation, a dedicated wireless network is recommended when available.

