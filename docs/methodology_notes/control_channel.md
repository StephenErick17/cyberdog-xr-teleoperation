\# Control Channel



This document describes the control channel used in the augmented reality teleoperation architecture for Xiaomi CyberDog.



The control channel transmits the operator’s intention from the Meta Quest 3 / Unity AR interface to the Xiaomi CyberDog through a lightweight UDP-to-ROS 2 bridge.



\## Control channel overview



```text

Meta Quest 3 / Unity AR interface

&#x20;       ↓ UDP 5005

Ubuntu Bridge PC

&#x20;       ↓ ROS 2 control topics

Xiaomi CyberDog

```



The control channel is responsible for:



```text

\- Continuous locomotion commands.

\- Discrete posture and gait actions.

\- Stop or null commands.

\- Translation from Unity UDP packets to CyberDog ROS 2 messages.

```



\## Main implementation files



Unity-side scripts:



```text

unity/scripts/JoystickCommandReader.cs

unity/scripts/JoystickUdpSender.cs

unity/scripts/UdpCommandSender.cs

unity/scripts/UdpMoveHoldButton.cs

unity/scripts/VirtualJoystick.cs

```



ROS 2 bridge script:



```text

ros2\_bridge/control/udp\_to\_ros\_bridge.py

```



\## UDP communication



The Unity AR interface sends control packets to the Ubuntu Bridge PC using UDP port `5005`.



```text

Unity / Meta Quest 3

&#x20;       sends UDP packets to

Ubuntu Bridge PC:5005

```



The ROS 2 bridge listens on:



```text

0.0.0.0:5005

```



\## UDP message formats



The control channel uses two message formats:



```text

cmd:lx,ly,az

action:x

```



\## Continuous locomotion command



The continuous locomotion command has the following format:



```text

cmd:lx,ly,az

```



Where:



```text

lx → linear velocity along the x axis

ly → linear velocity along the y axis

az → angular velocity around the z axis

```



Example commands:



```text

cmd:0.25,0.00,0.00

cmd:-0.25,0.00,0.00

cmd:0.00,0.15,0.00

cmd:0.00,0.00,0.40

cmd:0.00,0.00,0.00

```



The null command is used to stop the robot:



```text

cmd:0.00,0.00,0.00

```



\## Joystick mapping



The Unity AR interface uses two virtual joysticks.



```text

Left joystick:

&#x20; vertical axis    → linear\_x

&#x20; horizontal axis  → angular\_z



Right joystick:

&#x20; horizontal axis  → linear\_y

```



This mapping separates longitudinal/yaw motion from lateral motion.



\## Hybrid transmission logic



The locomotion interface uses a hybrid command transmission strategy:



```text

1\. While the joystick is displaced, Unity sends continuous locomotion commands.

2\. When the joystick returns to the neutral position, Unity sends one null command.

3\. While the joystick remains centered, Unity stops sending redundant zero commands.

4\. A new command sequence starts when the joystick is moved again.

```



This approach reduces unnecessary network traffic and preserves a clear relationship between the operator’s intention and the robot’s movement state.



\## Discrete action command



Discrete action commands have the following format:



```text

action:x

```



Examples:



```text

action:1

action:2

action:3

action:4

action:5

action:6

```



These commands are interpreted by the ROS 2 bridge and mapped to CyberDog posture or gait actions.



Typical actions include:



```text

stand up

lie down

sit

walk

run

predefined gait/action command

```



\## ROS 2 bridge behavior



The bridge script:



```text

ros2\_bridge/control/udp\_to\_ros\_bridge.py

```



performs the following operations:



```text

1\. Opens a UDP socket on port 5005.

2\. Receives UDP packets from Unity.

3\. Parses the received message.

4\. Identifies whether the message is a locomotion command or an action command.

5\. Converts locomotion commands into SE3VelocityCMD messages.

6\. Converts action commands into ActionRequest messages.

7\. Publishes the resulting messages to CyberDog ROS 2 topics.

```



\## ROS 2 control topics



The bridge publishes to:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

```



The `body\_cmd` topic is used for continuous locomotion.



The `cyberdog\_action` topic is used for discrete posture and gait commands.



\## ROS 2 message types



The control channel uses CyberDog-specific ROS 2 message types:



```text

motion\_msgs/msg/SE3VelocityCMD

motion\_msgs/msg/ActionRequest

```



These message interfaces must be available in the ROS 2 environment before running the bridge.



\## Basic execution



From the Ubuntu Bridge PC, source ROS 2:



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



Expected terminal output should indicate that the UDP bridge is listening on port `5005`.



\## Validation procedure



Recommended validation order:



```text

1\. Verify that CyberDog ROS 2 topics are visible.

2\. Run udp\_to\_ros\_bridge.py.

3\. Send a simple UDP test command.

4\. Verify that the bridge receives the packet.

5\. Verify that the bridge publishes to /mi1036358/body\_cmd or /mi1036358/cyberdog\_action.

6\. Verify that CyberDog executes the command.

7\. Test Unity joystick commands.

8\. Test Unity discrete action buttons.

9\. Test null command behavior.

```



\## Useful ROS 2 commands



List CyberDog topics:



```bash

ros2 topic list | grep mi1036358

```



Check body command topic:



```bash

ros2 topic info /mi1036358/body\_cmd

```



Check action topic:



```bash

ros2 topic info /mi1036358/cyberdog\_action

```



Echo body command topic:



```bash

ros2 topic echo /mi1036358/body\_cmd

```



Echo action topic:



```bash

ros2 topic echo /mi1036358/cyberdog\_action

```



\## Safety notes



Before testing locomotion:



```text

\- Place the robot in an open area.

\- Start with low velocity values.

\- Verify that the null command stops the robot.

\- Avoid obstacles, stairs, fragile objects, and nearby people.

\- Keep physical access to the robot during testing.

```



\## Troubleshooting



\### The bridge does not receive Unity commands



Check:



```text

\- Unity is sending to the correct Ubuntu Bridge PC IP address.

\- UDP port 5005 is configured correctly.

\- The Meta Quest 3 and Ubuntu Bridge PC are on the same network.

\- The firewall is not blocking UDP traffic.

\- udp\_to\_ros\_bridge.py is running.

```



\### The bridge receives commands but the robot does not move



Check:



```text

\- /mi1036358/body\_cmd is available.

\- CyberDog message interfaces are sourced.

\- The robot is in a valid locomotion state.

\- The velocity values are not too small.

\- ROS 2 communication with CyberDog is active.

```



\### Discrete actions do not execute



Check:



```text

\- /mi1036358/cyberdog\_action is available.

\- motion\_msgs/msg/ActionRequest is sourced.

\- The action code is supported by udp\_to\_ros\_bridge.py.

\- CyberDog is not busy executing another action.

```



\## Notes



The control channel uses lightweight UDP packets and is less demanding than the visual perception channel.



The decoupled design allows the control path to remain operational even if visual feedback is temporarily degraded.



For reproducible tests, record the following values:



```text

\- Ubuntu Bridge PC IP address.

\- Meta Quest 3 IP address.

\- UDP control port.

\- ROS\_DOMAIN\_ID.

\- CyberDog namespace.

\- Velocity scaling values used in Unity.

```

