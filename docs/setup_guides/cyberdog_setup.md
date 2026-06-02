\# Xiaomi CyberDog Setup



This document describes the CyberDog setup and verification steps required to reproduce the augmented reality teleoperation architecture using Meta Quest 3, Unity, UDP communication, and ROS 2.



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



A different CyberDog unit or namespace requires updating the corresponding namespace references in the bridge scripts.



Relevant scripts:



```text

ros2\_bridge/control/udp\_to\_ros\_bridge.py

ros2\_bridge/video/camera\_udp\_sender.py

```



\## Required ROS 2 interfaces



The bridge uses CyberDog-specific message interfaces:



```text

motion\_msgs/msg/SE3VelocityCMD

motion\_msgs/msg/ActionRequest

```



These interfaces are sourced on the Ubuntu Bridge PC before running the bridge scripts.



Example:



```bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



\## Control topics



The main CyberDog control topics used by the bridge are:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

```



The `body\_cmd` topic handles continuous locomotion commands.



The `cyberdog\_action` topic handles discrete posture and gait actions.



\## Camera topics



The main camera topics used in the implementation are:



```text

/mi1036358/camera/color/image\_raw

/mi1036358/camera/depth/image\_rect\_raw

/mi1036358/camera/aligned\_depth\_to\_color/image\_raw

```



The default camera topic used by the video sender is:



```text

/mi1036358/camera/color/image\_raw

```



Depth visualization is activated by changing the active topic and mode in:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



\## ROS 2 topic discovery



On the Ubuntu Bridge PC, source ROS 2 and the CyberDog interfaces:



```bash

source /opt/ros/humble/setup.bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



Verify CyberDog topic discovery:



```bash

ros2 topic list | grep mi1036358

```



The discovered topics include control and camera topics such as:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

/mi1036358/camera/color/image\_raw

```



\## Camera stream verification



Check the RGB camera frequency:



```bash

ros2 topic hz /mi1036358/camera/color/image\_raw

```



Check camera topic information:



```bash

ros2 topic info /mi1036358/camera/color/image\_raw

```



List camera services:



```bash

ros2 service list | grep camera

```



The camera sender can request camera activation through:



```text

/mi1036358/camera/enable

```



The script can also subscribe to an already active image topic when the camera stream is publishing.



\## Control topic verification



Check the body command topic:



```bash

ros2 topic info /mi1036358/body\_cmd

```



Check the action topic:



```bash

ros2 topic info /mi1036358/cyberdog\_action

```



Echo commands for verification:



```bash

ros2 topic echo /mi1036358/body\_cmd

ros2 topic echo /mi1036358/cyberdog\_action

```



\## Control command flow



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



Supported UDP control formats:



```text

cmd:lx,ly,az

action:x

```



The bridge converts `cmd:lx,ly,az` messages into `SE3VelocityCMD`.



The bridge converts `action:x` messages into `ActionRequest`.



\## Safety protocol



Teleoperation tests are performed with the robot in a controlled and open area.



```text

1\. CyberDog is placed in a safe area with sufficient free space.

2\. Battery level is verified before testing.

3\. Nearby stairs, fragile objects, and people are avoided.

4\. Initial tests use low velocity values.

5\. Stop or null command behavior is verified before continuous operation.

6\. Physical access to the robot is maintained during the test.

```



Null command:



```text

cmd:0.00,0.00,0.00

```



\## Startup sequence



```text

1\. Power on Xiaomi CyberDog.

2\. Connect CyberDog, Ubuntu Bridge PC, and Meta Quest 3 to the same network.

3\. Verify network connectivity between CyberDog and the Ubuntu Bridge PC.

4\. Source ROS 2 Humble on the Ubuntu Bridge PC.

5\. Source CyberDog message interfaces.

6\. Verify CyberDog ROS 2 topics.

7\. Run udp\_to\_ros\_bridge.py.

8\. Run camera\_udp\_sender.py.

9\. Launch the Unity AR application on Meta Quest 3.

10\. Validate discrete actions.

11\. Validate low-speed locomotion.

12\. Validate complete AR teleoperation with visual feedback.

```



\## Troubleshooting



\### CyberDog topics are not visible



```text

\- CyberDog power state.

\- Network connection between CyberDog and Ubuntu Bridge PC.

\- ROS 2 environment sourcing.

\- CyberDog message interface availability.

\- Cyclone DDS configuration.

\- ROS\_DOMAIN\_ID compatibility.

```



\### Robot does not move



```text

\- Execution state of udp\_to\_ros\_bridge.py.

\- Unity UDP destination IP.

\- UDP port 5005.

\- Availability of /mi1036358/body\_cmd.

\- Availability of motion\_msgs/msg/SE3VelocityCMD.

\- Robot locomotion state.

```



\### Discrete actions do not execute



```text

\- Availability of /mi1036358/cyberdog\_action.

\- Availability of motion\_msgs/msg/ActionRequest.

\- Supported action code in udp\_to\_ros\_bridge.py.

\- Current execution state of the robot.

```



\### Camera stream is not available



```text

\- Activity of the selected image topic.

\- Availability of the camera service.

\- Active topic configured in camera\_udp\_sender.py.

\- CvBridge installation.

\- OpenCV installation.

```



\## Reproducibility notes



CyberDog ROS 2 configuration can vary depending on robot firmware, network configuration, active namespace, and available interfaces.



The namespace, topic names, message types, and camera topics are verified before executing the complete AR teleoperation workflow.

