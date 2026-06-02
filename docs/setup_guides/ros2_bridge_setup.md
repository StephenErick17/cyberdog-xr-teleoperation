\# ROS 2 Bridge Setup



This document describes the basic setup procedure required to run the ROS 2 bridge used in the augmented reality teleoperation architecture for Xiaomi CyberDog.



The bridge computer acts as the intermediate layer between the Meta Quest 3 / Unity AR interface and the CyberDog ROS 2 environment. It receives UDP control commands from Unity, publishes CyberDog-compatible ROS 2 messages, and sends camera frames back to Unity through UDP.



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



\## Bridge computer requirements



The Ubuntu bridge computer should provide:



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



\## CyberDog requirements



The CyberDog platform should be running its native ROS 2 environment and must expose the required control and camera topics.



Expected CyberDog environment:



```text

Ubuntu 18.04

ROS 2 Foxy

Cyclone DDS

CyberDog motion message interfaces

CyberDog camera topics

```



\## Source ROS 2 Humble



On the Ubuntu bridge computer, source ROS 2 Humble:



```bash

source /opt/ros/humble/setup.bash

```



\## Source CyberDog message interfaces



The bridge requires CyberDog-specific message types such as:



```text

motion\_msgs/msg/SE3VelocityCMD

motion\_msgs/msg/ActionRequest

```



Source the workspace where these interfaces are available:



```bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



Adjust the path if the CyberDog interfaces are installed in a different workspace.



\## Verify ROS 2 communication



Check that the CyberDog topics are visible from the bridge computer:



```bash

ros2 topic list | grep mi1036358

```



Expected control topics include:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

```



Expected camera topics include:



```text

/mi1036358/camera/color/image\_raw

/mi1036358/camera/depth/image\_rect\_raw

/mi1036358/camera/aligned\_depth\_to\_color/image\_raw

```



\## Run the UDP-to-ROS 2 control bridge



From the repository root, run:



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



Examples:



```text

cmd:0.25,0.00,0.00

cmd:0.00,0.00,0.40

cmd:0.00,0.15,0.00

action:1

action:3

```



The bridge translates these packets into CyberDog ROS 2 control messages.



\## Run the camera UDP sender



Before running the camera sender, update the Meta Quest 3 IP address inside:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



Then run the script from the repository root:



```bash

python3 ros2\_bridge/video/camera\_udp\_sender.py

```



The camera sender subscribes to a CyberDog image topic, processes the frame with CvBridge and OpenCV, compresses it as JPEG, and sends it to Unity through UDP port `5006`.



\## Recommended execution order



```text

1\. Start Xiaomi CyberDog.

2\. Connect CyberDog, Ubuntu Bridge PC, and Meta Quest 3 to the same network.

3\. Verify the IP address of the Ubuntu Bridge PC.

4\. Verify the IP address of Meta Quest 3.

5\. Source ROS 2 Humble on the bridge computer.

6\. Source the CyberDog message interfaces.

7\. Verify that CyberDog ROS 2 topics are visible.

8\. Run udp\_to\_ros\_bridge.py.

9\. Run camera\_udp\_sender.py.

10\. Launch the Unity AR application on Meta Quest 3.

11\. Verify robot command execution and camera visualization.

```



\## Useful ROS 2 commands



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



\## Useful network commands



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



Check that:



```text

\- CyberDog is powered on.

\- CyberDog and the bridge computer are connected to the same network.

\- ROS 2 environment variables are correctly configured.

\- Cyclone DDS is active and correctly configured.

\- The CyberDog ROS 2 environment is running.

```



\### Unity commands do not move the robot



Check that:



```text

\- udp\_to\_ros\_bridge.py is running.

\- Unity is sending commands to the correct bridge computer IP address.

\- UDP port 5005 is not blocked.

\- The CyberDog control topics are visible.

\- The CyberDog message interfaces are sourced.

```



\### Camera stream is not displayed in Unity



Check that:



```text

\- camera\_udp\_sender.py is running.

\- The Quest IP address is correctly configured in camera\_udp\_sender.py.

\- UDP port 5006 is not blocked.

\- The selected CyberDog camera topic is active.

\- Unity is listening on the expected video port.

```



\## Notes



The control channel uses lightweight UDP packets and should remain responsive under moderate network constraints.



The visual perception channel is more demanding because it involves image acquisition, conversion, resizing, JPEG compression, UDP transmission, reconstruction in Unity, and display inside the AR HUD.



For stable operation, a dedicated wireless network is recommended when available.

