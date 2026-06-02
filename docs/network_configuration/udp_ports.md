\# UDP Ports



This document summarizes the UDP ports used in the augmented reality teleoperation architecture for Xiaomi CyberDog.



The system uses two independent UDP communication channels:



```text

UDP 5005: Control channel

UDP 5006: Visual perception channel

```



This separation allows the command flow and the visual feedback flow to operate independently.



\## Port summary



```text

Port 5005

Direction: Meta Quest 3 / Unity → Ubuntu Bridge PC

Purpose: Locomotion and discrete action commands



Port 5006

Direction: Ubuntu Bridge PC → Meta Quest 3 / Unity

Purpose: RGB/depth camera stream

```



\## Control channel



The control channel sends operator commands from the Unity AR interface to the Ubuntu bridge computer.



```text

Meta Quest 3 / Unity AR interface

&#x20;       ↓ UDP 5005

Ubuntu Bridge PC

&#x20;       ↓ ROS 2 control topics

Xiaomi CyberDog

```



The bridge script associated with this channel is:



```text

ros2\_bridge/control/udp\_to\_ros\_bridge.py

```



The default listening address is:



```text

0.0.0.0:5005

```



Supported UDP message formats:



```text

cmd:lx,ly,az

action:x

```



Examples:



```text

cmd:0.25,0.00,0.00

cmd:0.00,0.00,0.40

cmd:0.00,0.15,0.00

action:1

action:3

```



\## Visual perception channel



The visual perception channel sends RGB or depth camera frames from the Ubuntu bridge computer to Unity.



```text

Xiaomi CyberDog camera

&#x20;       ↓ ROS 2 image topic

Ubuntu Bridge PC

&#x20;       ↓ UDP 5006

Meta Quest 3 / Unity AR interface

```



The bridge script associated with this channel is:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



The default destination used during development was:



```text

192.168.242.18:5006

```



This address corresponds to the Meta Quest 3 IP address and must be updated whenever the headset receives a different IP address.



\## Configuration points



Update the Ubuntu bridge computer IP address in the Unity UDP sender scripts.



Update the Meta Quest 3 IP address in:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



Check that the selected ports are not blocked by firewall rules.



\## Recommended checks



From the Ubuntu bridge computer, verify the local IP address:



```bash

ip addr

```



Verify that the UDP control bridge is running:



```bash

python3 ros2\_bridge/control/udp\_to\_ros\_bridge.py

```



Verify that the camera sender is running:



```bash

python3 ros2\_bridge/video/camera\_udp\_sender.py

```



Verify that CyberDog ROS 2 topics are available:



```bash

ros2 topic list | grep mi1036358

```



\## Network notes



The experiments were conducted using a shared mobile wireless network with an available bandwidth of approximately 18.67 Mbps.



The control channel uses lightweight UDP packets and is less demanding in terms of bandwidth.



The visual perception channel is more sensitive to:



```text

\- Available bandwidth

\- Packet loss

\- Frame size

\- JPEG quality

\- Network congestion

\- Distance to the access point

\- Simultaneous traffic from other devices

```



For more stable visual feedback, a dedicated wireless network or improved bandwidth is recommended.

