\# AR Teleoperation Workflow



This document describes the methodological workflow used to reproduce the augmented reality teleoperation architecture for Xiaomi CyberDog using Meta Quest 3, Unity, UDP communication, and ROS 2.



The system is organized into two decoupled functional channels:



1\. Control channel.

2\. Visual perception channel.



This separation allows the command flow and the visual feedback flow to be configured, tested, and evaluated independently before integrating the complete AR teleoperation workflow.



\## General workflow



```text

1\. The operator interacts with the AR HUD in Meta Quest 3.

2\. Unity generates locomotion or discrete action commands.

3\. Commands are sent through UDP to the Ubuntu bridge computer.

4\. The ROS 2 bridge receives and parses the UDP packets.

5\. The bridge translates the received data into CyberDog-compatible ROS 2 messages.

6\. The bridge publishes the corresponding ROS 2 control commands.

7\. CyberDog executes locomotion or discrete actions.

8\. CyberDog camera frames are acquired through ROS 2 image topics.

9\. The bridge processes, resizes, and compresses the frames.

10\. The processed frames are transmitted through UDP to Unity.

11\. Unity receives the visual stream and displays it inside the AR HUD.

```



\## Control channel



```text

Meta Quest 3 / Unity AR interface

&#x20;       ↓ UDP 5005

Ubuntu Bridge PC

&#x20;       ↓ ROS 2 control topics

Xiaomi CyberDog

```



The control channel transmits lightweight UDP packets representing continuous locomotion commands or discrete robot actions.



The Unity interface sends messages using the following formats:



```text

cmd:lx,ly,az

action:x

```



The ROS 2 bridge receives these messages and publishes the corresponding CyberDog control commands.



\## Visual perception channel



```text

Xiaomi CyberDog camera

&#x20;       ↓ ROS 2 image topics

Ubuntu Bridge PC

&#x20;       ↓ OpenCV/CvBridge + JPEG compression + UDP 5006

Meta Quest 3 / Unity AR interface

```



The visual perception channel transmits RGB or depth visual feedback from CyberDog to the AR interface.



The camera sender subscribes to ROS 2 image topics, converts the images using CvBridge, processes the frames with OpenCV, compresses them as JPEG images, and sends them to Unity through UDP.



\## Methodological principle



The architecture is designed as a modular and reproducible workflow. Each subsystem can be tested independently before performing complete teleoperation.



```text

Unity input

&#x20;       ↓

UDP control test

&#x20;       ↓

ROS 2 bridge

&#x20;       ↓

CyberDog command execution

```



```text

CyberDog camera

&#x20;       ↓

ROS 2 image topic

&#x20;       ↓

UDP video sender

&#x20;       ↓

Unity video receiver

&#x20;       ↓

AR HUD visualization

```



This modular organization allows implementation errors, network issues, ROS 2 topic problems, and Unity-side communication errors to be isolated more easily.



\## Recommended validation order



```text

1\. Verify that the CyberDog and the Ubuntu bridge computer are connected to the same network.

2\. Verify ROS 2 communication between CyberDog and the bridge computer.

3\. Verify that the CyberDog control topics are visible.

4\. Verify that the CyberDog camera topics are available.

5\. Run the UDP-to-ROS 2 control bridge.

6\. Test UDP command reception using simple command packets.

7\. Confirm that CyberDog executes the received locomotion or action commands.

8\. Run the camera UDP sender.

9\. Confirm that Unity receives and displays the camera stream.

10\. Execute the complete AR teleoperation workflow from Meta Quest 3.

```



\## Complete execution sequence



From the Ubuntu bridge computer, source ROS 2:



```bash

source /opt/ros/humble/setup.bash

```



Source the CyberDog message interfaces:



```bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



Run the control bridge:



```bash

python3 ros2\_bridge/control/udp\_to\_ros\_bridge.py

```



Run the camera sender:



```bash

python3 ros2\_bridge/video/camera\_udp\_sender.py

```



Then launch the Unity AR application on Meta Quest 3 and verify:



```text

1\. Joystick commands are received by the bridge.

2\. Discrete action buttons trigger CyberDog actions.

3\. The camera stream is displayed inside the AR HUD.

4\. Control and visual feedback operate simultaneously.

```



\## Expected behavior



A correct implementation should provide:



```text

\- Continuous locomotion commands while the joystick is active.

\- A single null command when the joystick returns to the neutral position.

\- Discrete action commands for posture or gait changes.

\- Real-time RGB or depth visual feedback inside the AR HUD.

\- Independent operation of the control and visual perception channels.

```



\## Notes



The control channel uses lightweight UDP packets and is therefore less demanding than the visual perception channel.



The visual perception channel is more sensitive to bandwidth, frame size, JPEG quality, packet loss, and network congestion.



When reproducing the workflow, update the following values according to the active network configuration:



```text

\- Ubuntu bridge computer IP address in the Unity UDP sender scripts.

\- Meta Quest 3 IP address in ros2\_bridge/video/camera\_udp\_sender.py.

\- UDP control port, if different from 5005.

\- UDP video port, if different from 5006.

```

