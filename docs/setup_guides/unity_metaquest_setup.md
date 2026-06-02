\# Unity and Meta Quest 3 Setup



This document describes the Unity and Meta Quest 3 configuration used to implement the augmented reality teleoperation interface for Xiaomi CyberDog.



The Unity application provides an AR-oriented teleoperation HUD that allows the operator to command CyberDog through virtual joysticks, trigger discrete actions, and visualize RGB/depth feedback inside the headset.



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



\## Unity environment



The implementation was developed using:



```text

Unity 2022.3.4f1

Android build target

OpenXR support

Meta Quest 3 support

World Space Canvas

UDP communication

AR-oriented teleoperation HUD

```



\## Unity scene structure



The AR teleoperation scene contains the following functional components:



```text

XR camera rig

World Space Canvas

Camera visualization panel

Left virtual joystick

Right virtual joystick

Discrete action buttons

UDP control sender components

UDP video receiver component

```



\## AR HUD components



The HUD integrates the control and visual feedback elements into a single world-space interface.



```text

Camera panel:

&#x20; Displays RGB or depth feedback received through UDP.



Left joystick:

&#x20; Controls linear\_x and angular\_z.



Right joystick:

&#x20; Controls linear\_y.



Action buttons:

&#x20; Send discrete commands such as stand up, lie down, sit, walk, run, or other predefined actions.

```



\## Locomotion mapping



```text

Left joystick:

&#x20; vertical axis    → linear\_x

&#x20; horizontal axis  → angular\_z



Right joystick:

&#x20; horizontal axis  → linear\_y

```



The resulting UDP locomotion message follows this format:



```text

cmd:lx,ly,az

```



Example:



```text

cmd:0.25,0.00,0.00

```



\## Discrete action messages



Discrete action buttons use the following message format:



```text

action:x

```



Examples:



```text

action:1

action:3

```



The action code is interpreted by the ROS 2 bridge and mapped to the corresponding CyberDog gait or action command.



\## UDP configuration in Unity



The Unity UDP sender scripts use the IP address of the Ubuntu Bridge PC.



```text

Bridge IP: <ubuntu\_bridge\_pc\_ip>

Control port: 5005

```



The Unity UDP video receiver listens on:



```text

Video port: 5006

```



The Unity-side configuration links the AR HUD to the UDP communication layer used by the ROS 2 bridge.



\## Essential Unity scripts



```text

unity/scripts/CameraUdpReceiver.cs

unity/scripts/DraggableWorldUIPanel.cs

unity/scripts/JoystickCommandReader.cs

unity/scripts/JoystickUdpSender.cs

unity/scripts/UdpButtonTestUI.cs

unity/scripts/UdpCommandSender.cs

unity/scripts/UdpMoveHoldButton.cs

unity/scripts/VirtualJoystick.cs

```



\## Script roles



```text

CameraUdpReceiver.cs:

&#x20; Receives JPEG-compressed RGB/depth frames through UDP and displays them in the AR HUD.



DraggableWorldUIPanel.cs:

&#x20; Provides repositioning behavior for the world-space HUD panel.



JoystickCommandReader.cs:

&#x20; Reads joystick values and converts them into locomotion command components.



JoystickUdpSender.cs:

&#x20; Sends joystick-based locomotion commands to the Ubuntu Bridge PC.



UdpButtonTestUI.cs:

&#x20; Provides UI-level tests for UDP button commands.



UdpCommandSender.cs:

&#x20; Sends discrete UDP commands, including action commands.



UdpMoveHoldButton.cs:

&#x20; Sends movement commands while a UI button remains pressed.



VirtualJoystick.cs:

&#x20; Implements the virtual joystick interaction behavior.

```



\## Hybrid command logic



The locomotion interface uses a hybrid transmission strategy:



```text

1\. Continuous motion commands are transmitted while the joystick is displaced.

2\. A null command is transmitted when the joystick returns to the center.

3\. Redundant zero-velocity commands are not continuously transmitted while the joystick remains centered.

```



Null command:



```text

cmd:0.00,0.00,0.00

```



This logic reduces unnecessary network traffic and preserves a direct relationship between operator intention and robot motion state.



\## Build target



The Unity project is configured for:



```text

Platform: Android

Device: Meta Quest 3

XR backend: OpenXR

Interface type: AR-oriented world-space HUD

```



\## Validation sequence



```text

1\. Verify joystick motion inside the Unity scene.

2\. Verify UDP command generation through Unity logs.

3\. Verify UDP packet reception on the Ubuntu Bridge PC.

4\. Verify discrete action button messages.

5\. Verify UDP video reception using the camera sender.

6\. Build and deploy the application to Meta Quest 3.

7\. Validate control and visual feedback inside the headset.

```



\## Troubleshooting



\### Commands are not received by the bridge



```text

\- Ubuntu Bridge PC IP address in Unity.

\- UDP port 5005.

\- Network connection between Meta Quest 3 and the bridge computer.

\- Firewall configuration on the bridge computer.

\- Execution state of udp\_to\_ros\_bridge.py.

```



\### Video is not displayed in Unity



```text

\- Unity UDP receiver port 5006.

\- Execution state of camera\_udp\_sender.py.

\- Meta Quest 3 IP address configured in camera\_udp\_sender.py.

\- Activity of the selected CyberDog camera topic.

\- Assignment of the video panel inside the Unity scene.

```



\### Joystick does not control the robot



```text

\- Joystick references in the Unity inspector.

\- UDP sender component configuration.

\- Command format generated by the script.

\- ROS 2 bridge logs.

\- CyberDog control topics.

```



\## Repository exclusion policy



The lightweight repository includes the essential Unity scripts and setup documentation.



Generated Unity folders are excluded through `.gitignore`:



```text

Library/

Temp/

Build/

Builds/

Logs/

UserSettings/

.vs/

```



The complete Unity project can be incorporated as a structured release when full scene-level reproduction is required.

