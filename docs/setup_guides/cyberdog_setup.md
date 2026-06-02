\# Xiaomi CyberDog Setup



This document summarizes the basic CyberDog setup and verification steps required to reproduce the augmented reality teleoperation architecture using Meta Quest 3, Unity, UDP communication, and ROS 2.



The CyberDog acts as the physical quadruped robot platform. It receives locomotion and discrete action commands through ROS 2 topics and provides RGB/depth camera streams used as visual feedback inside the AR interface.



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

&#x20;       ↓ UDP 5006

Meta Quest 3 / Unity AR interface

```



\## CyberDog environment



The implementation was tested with the CyberDog native ROS 2 environment.



Expected robot-side environment:



```text

Robot: Xiaomi CyberDog

Embedded platform: NVIDIA Jetson Xavier NX

Operating system: Ubuntu 18.04

ROS version: ROS 2 Foxy

DDS middleware: Cyclone DDS

```



\## Namespace



The CyberDog namespace used in the implementation was:



```text

/mi1036358

```



If a different CyberDog unit or namespace is used, the scripts must be updated accordingly.



Relevant scripts:



```text

ros2\_bridge/control/udp\_to\_ros\_bridge.py

ros2\_bridge/video/camera\_udp\_sender.py

```



\## Required ROS 2 interfaces



The bridge requires CyberDog-specific message interfaces, mainly:



```text

motion\_msgs/msg/SE3VelocityCMD

motion\_msgs/msg/ActionRequest

```



These interfaces must be available and sourced on the Ubuntu Bridge PC before running the bridge scripts.



Example:



```bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



Adjust the path according to the local workspace where the CyberDog interfaces are installed.



\## Control topics



The main CyberDog control topics used by the bridge are:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

```



The `body\_cmd` topic is used for continuous locomotion commands.



The `cyberdog\_action` topic is used for discrete posture or gait actions.



\## Camera topics



The main camera topics considered in the implementation are:



```text

/mi1036358/camera/color/image\_raw

/mi1036358/camera/depth/image\_rect\_raw

/mi1036358/camera/aligned\_depth\_to\_color/image\_raw

```



The default topic used by the camera sender is:



```text

/mi1036358/camera/color/image\_raw

```



Depth topics can be enabled by modifying the active topic and mode inside:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



\## Verify ROS 2 topic discovery



From the Ubuntu Bridge PC, source ROS 2 and the required interfaces:



```bash

source /opt/ros/humble/setup.bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



Then verify that CyberDog topics are visible:



```bash

ros2 topic list | grep mi1036358

```



Expected output should include control and camera topics such as:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

/mi1036358/camera/color/image\_raw

```



\## Verify camera stream



Check whether the RGB camera topic is publishing:



```bash

ros2 topic hz /mi1036358/camera/color/image\_raw

```



Check topic information:



```bash

ros2 topic info /mi1036358/camera/color/image\_raw

```



If the camera topic is not active, verify whether the camera service is available:



```bash

ros2 service list | grep camera

```



The camera sender script attempts to enable the camera using:



```text

/mi1036358/camera/enable

```



If this service is not available, the script can still subscribe to the active image topic when the camera is already publishing.



\## Verify control topics



Check that the body command topic exists:



```bash

ros2 topic info /mi1036358/body\_cmd

```



Check that the action topic exists:



```bash

ros2 topic info /mi1036358/cyberdog\_action

```



Optional echo commands:



```bash

ros2 topic echo /mi1036358/body\_cmd

ros2 topic echo /mi1036358/cyberdog\_action

```



\## Control command flow



The UDP-to-ROS 2 bridge receives commands from Unity and publishes them to CyberDog.



```text

Unity joystick or action button

&#x20;       ↓

UDP packet

&#x20;       ↓

udp\_to\_ros\_bridge.py

&#x20;       ↓

ROS 2 message

&#x20;       ↓

CyberDog control topic

&#x20;       ↓

Robot execution

```



The supported UDP control formats are:



```text

cmd:lx,ly,az

action:x

```



The bridge converts `cmd:lx,ly,az` messages into `SE3VelocityCMD`.



The bridge converts `action:x` messages into `ActionRequest`.



\## Safety recommendations



Before running teleoperation tests:



```text

1\. Place CyberDog in an open and safe area.

2\. Keep enough free space around the robot.

3\. Verify battery level before testing.

4\. Avoid testing near stairs, fragile objects, or people.

5\. Start with low velocity values.

6\. Confirm that a stop or null command is correctly transmitted.

7\. Keep physical access to the robot for emergency intervention.

```



Recommended null command:



```text

cmd:0.00,0.00,0.00

```



\## Recommended startup sequence



```text

1\. Power on Xiaomi CyberDog.

2\. Connect CyberDog to the same network as the Ubuntu Bridge PC and Meta Quest 3.

3\. Verify network connectivity between CyberDog and the Ubuntu Bridge PC.

4\. Source ROS 2 Humble on the Ubuntu Bridge PC.

5\. Source CyberDog message interfaces.

6\. Verify CyberDog ROS 2 topics.

7\. Run udp\_to\_ros\_bridge.py.

8\. Run camera\_udp\_sender.py.

9\. Launch the Unity AR application on Meta Quest 3.

10\. Test discrete actions first.

11\. Test low-speed locomotion commands.

12\. Test complete AR teleoperation with visual feedback.

```



\## Troubleshooting



\### CyberDog topics are not visible



Check:



```text

\- CyberDog is powered on.

\- CyberDog and the Ubuntu Bridge PC are on the same network.

\- ROS 2 environments are sourced correctly.

\- CyberDog message interfaces are available.

\- Cyclone DDS is configured correctly.

\- ROS\_DOMAIN\_ID is compatible between devices.

```



\### Robot does not move



Check:



```text

\- udp\_to\_ros\_bridge.py is running.

\- Unity is sending UDP commands to the correct bridge IP.

\- UDP port 5005 is not blocked.

\- /mi1036358/body\_cmd is available.

\- motion\_msgs/msg/SE3VelocityCMD is sourced correctly.

\- The robot is in a state that allows locomotion.

```



\### Discrete actions do not execute



Check:



```text

\- /mi1036358/cyberdog\_action is available.

\- motion\_msgs/msg/ActionRequest is sourced correctly.

\- The action code is supported by udp\_to\_ros\_bridge.py.

\- The robot is not busy executing another action.

```



\### Camera is not available



Check:



```text

\- The selected image topic is publishing.

\- The camera service is available, if required.

\- camera\_udp\_sender.py is using the correct topic.

\- CvBridge and OpenCV are installed on the bridge computer.

```



\## Notes



The CyberDog ROS 2 environment may vary depending on the robot configuration, firmware, network setup, and available interfaces.



When reproducing the architecture with another CyberDog unit, verify the namespace, topic names, message types, and camera topics before running the complete AR teleoperation workflow.

