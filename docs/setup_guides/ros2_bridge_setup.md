\# ROS 2 Bridge Setup



This document describes the setup procedure required to run the ROS 2 bridge used in the augmented reality teleoperation architecture for Xiaomi CyberDog.



The Ubuntu Bridge PC acts as the intermediate layer between the Meta Quest 3 / Unity AR interface and the CyberDog ROS 2 environment. It receives UDP control commands from Unity, publishes CyberDog-compatible ROS 2 messages, and sends camera frames back to Unity through UDP.



\## System role



```text

Meta Quest 3 / Unity AR interface

&#x20;       ↓ UDP 5005

Ubuntu Bridge PC

&#x20;       ↓ ROS 2 control topics

Xiaomi CyberDog

```



For visual feedback:



```text

Xiaomi CyberDog camera

&#x20;       ↓ ROS 2 image topics

Ubuntu Bridge PC

&#x20;       ↓ OpenCV/CvBridge + JPEG compression + UDP 5006

Meta Quest 3 / Unity AR interface

```



\## Bridge computer environment



The Ubuntu Bridge PC uses:



```text

Ubuntu 22.04

ROS 2 Humble

Python 3

OpenCV

CvBridge

CyberDog ROS 2 message interfaces

Network access to Xiaomi CyberDog

Network access to Meta Quest 3

```



\## CyberDog environment



The CyberDog platform runs its native ROS 2 environment and exposes the required control and camera topics.



```text

Ubuntu 18.04

ROS 2 Foxy

Cyclone DDS

CyberDog motion message interfaces

CyberDog camera topics

```



\## ROS 2 environment



Source ROS 2 Humble on the Ubuntu Bridge PC:



```bash

source /opt/ros/humble/setup.bash

```



Source the CyberDog message interfaces:



```bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



The bridge uses CyberDog-specific message types:



```text

motion\_msgs/msg/SE3VelocityCMD

motion\_msgs/msg/ActionRequest

```



\## ROS 2 communication verification



Verify that CyberDog topics are visible from the Ubuntu Bridge PC:



```bash

ros2 topic list | grep mi1036358

```



Control topics:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

```



Camera topics:



```text

/mi1036358/camera/color/image\_raw

/mi1036358/camera/depth/image\_rect\_raw

/mi1036358/camera/aligned\_depth\_to\_color/image\_raw

```



\## Control bridge execution



From the repository root:



```bash

python3 ros2\_bridge/control/udp\_to\_ros\_bridge.py

```



The control bridge listens on:



```text

0.0.0.0:5005

```



Supported UDP message formats:



```text

cmd:lx,ly,az

action:x

```



Example packets:



```text

cmd:0.25,0.00,0.00

cmd:0.00,0.00,0.40

cmd:0.00,0.15,0.00

action:1

action:3

```



The bridge translates these packets into CyberDog ROS 2 control messages.



\## Camera sender execution



The Meta Quest 3 IP address is configured in:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



From the repository root:



```bash

python3 ros2\_bridge/video/camera\_udp\_sender.py

```



The camera sender subscribes to a CyberDog image topic, processes the frame with CvBridge and OpenCV, compresses it as JPEG, and sends it to Unity through UDP port `5006`.



\## Execution sequence



```text

1\. Xiaomi CyberDog is powered on.

2\. CyberDog, Ubuntu Bridge PC, and Meta Quest 3 are connected to the same network.

3\. The Ubuntu Bridge PC IP address is verified.

4\. The Meta Quest 3 IP address is verified.

5\. ROS 2 Humble is sourced on the bridge computer.

6\. CyberDog message interfaces are sourced.

7\. CyberDog ROS 2 topics are verified.

8\. udp\_to\_ros\_bridge.py is executed.

9\. camera\_udp\_sender.py is executed.

10\. The Unity AR application is launched on Meta Quest 3.

11\. Robot command execution and camera visualization are validated.

```



\## ROS 2 verification commands



List CyberDog topics:



```bash

ros2 topic list | grep mi1036358

```



Check the body command topic:



```bash

ros2 topic echo /mi1036358/body\_cmd

```



Check the action topic:



```bash

ros2 topic echo /mi1036358/cyberdog\_action

```



Check the RGB camera frequency:



```bash

ros2 topic hz /mi1036358/camera/color/image\_raw

```



Check image topic information:



```bash

ros2 topic info /mi1036358/camera/color/image\_raw

```



\## Network verification commands



Check the bridge computer IP address:



```bash

ip addr

```



Check connectivity with another device:



```bash

ping <device\_ip>

```



Example:



```bash

ping 192.168.242.18

```



\## Troubleshooting



\### CyberDog topics are not visible



```text

\- CyberDog power state.

\- Network connection between CyberDog and Ubuntu Bridge PC.

\- ROS 2 environment sourcing.

\- ROS\_DOMAIN\_ID compatibility.

\- Cyclone DDS configuration.

\- CyberDog ROS 2 environment state.

```



\### Unity commands do not move the robot



```text

\- Execution state of udp\_to\_ros\_bridge.py.

\- Unity destination IP address.

\- UDP port 5005.

\- CyberDog control topic availability.

\- CyberDog message interface sourcing.

```



\### Camera stream is not displayed in Unity



```text

\- Execution state of camera\_udp\_sender.py.

\- Meta Quest 3 IP address in camera\_udp\_sender.py.

\- UDP port 5006.

\- Selected CyberDog camera topic.

\- Unity video receiver configuration.

```



\## Reproducibility notes



The control channel uses lightweight UDP packets and provides the command path between the AR interface and CyberDog.



The visual perception channel performs image acquisition, conversion, resizing, JPEG compression, UDP transmission, reconstruction in Unity, and display inside the AR HUD.



The bridge setup is verified before running the complete teleoperation workflow.

