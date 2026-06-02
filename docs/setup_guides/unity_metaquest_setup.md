\# Unity and Meta Quest 3 Setup



This document summarizes the Unity and Meta Quest 3 setup used to reproduce the augmented reality teleoperation interface for Xiaomi CyberDog.



The Unity application implements an AR-oriented teleoperation HUD that allows the operator to control CyberDog through virtual joysticks, trigger discrete actions, and visualize RGB/depth feedback inside the headset.



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



\## Unity requirements



The Unity project should be configured with:



```text

Unity 2022.3.4f1

Android build target

OpenXR support

Meta Quest 3 support

World Space Canvas

UDP communication scripts

```



\## Unity scene structure



The AR teleoperation scene should include:



```text

1\. AR camera or XR camera rig.

2\. World Space Canvas used as the teleoperation HUD.

3\. Camera visualization panel.

4\. Left virtual joystick.

5\. Right virtual joystick.

6\. Discrete action buttons.

7\. UDP control sender components.

8\. UDP video receiver component.

```



\## AR HUD components



The HUD integrates:



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



The final UDP locomotion message follows this format:



```text

cmd:lx,ly,az

```



Example:



```text

cmd:0.25,0.00,0.00

```



\## Discrete action messages



Discrete action buttons send messages using the format:



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



The Unity UDP sender scripts must use the IP address of the Ubuntu Bridge PC.



Example configuration:



```text

Bridge IP: <ubuntu\_bridge\_pc\_ip>

Control port: 5005

```



The Unity UDP video receiver must listen on:



```text

Video port: 5006

```



Before building the application, verify:



```text

1\. The Ubuntu Bridge PC IP address is correct.

2\. The UDP control port is set to 5005.

3\. The UDP video receiver is listening on port 5006.

4\. The Meta Quest 3 and Ubuntu Bridge PC are connected to the same network.

```



\## Essential Unity scripts



The repository includes the following Unity scripts:



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

&#x20; Allows the world-space HUD panel to be repositioned in the Unity scene.



JoystickCommandReader.cs:

&#x20; Reads joystick values and converts them into locomotion command components.



JoystickUdpSender.cs:

&#x20; Sends joystick-based locomotion commands to the Ubuntu Bridge PC.



UdpButtonTestUI.cs:

&#x20; Provides basic UI tests for UDP button commands.



UdpCommandSender.cs:

&#x20; Sends discrete UDP commands, including action commands.



UdpMoveHoldButton.cs:

&#x20; Sends movement commands while a UI button remains pressed.



VirtualJoystick.cs:

&#x20; Implements the virtual joystick interaction behavior.

```



\## Hybrid command logic



The locomotion interface should follow a hybrid transmission strategy:



```text

1\. Send continuous motion commands while the joystick is displaced.

2\. Send one null command when the joystick returns to the center.

3\. Stop sending redundant zero-velocity commands while the joystick remains centered.

```



Null command example:



```text

cmd:0.00,0.00,0.00

```



This logic reduces unnecessary network traffic and preserves a clear relationship between operator intention and robot motion.



\## Build target



The Unity project should be built for:



```text

Platform: Android

Device: Meta Quest 3

XR backend: OpenXR

Interface type: AR-oriented world-space HUD

```



\## Recommended testing sequence



Before deploying to Meta Quest 3:



```text

1\. Test joystick movement inside the Unity editor.

2\. Test UDP command generation using debug logs.

3\. Verify that the Ubuntu Bridge PC receives UDP packets.

4\. Test discrete action buttons.

5\. Test the UDP video receiver using the camera sender.

6\. Deploy the application to Meta Quest 3.

7\. Verify full control and visual feedback inside the headset.

```



\## Troubleshooting



\### Commands are not received by the bridge



Check:



```text

\- Ubuntu Bridge PC IP address in Unity.

\- UDP port 5005.

\- Network connection between Meta Quest 3 and the bridge computer.

\- Firewall settings on the bridge computer.

\- Whether udp\_to\_ros\_bridge.py is running.

```



\### Video is not displayed in Unity



Check:



```text

\- Unity is listening on UDP port 5006.

\- camera\_udp\_sender.py is running.

\- The Quest IP address is correctly configured in camera\_udp\_sender.py.

\- The selected CyberDog camera topic is publishing frames.

\- The video panel is assigned correctly in the Unity scene.

```



\### Joystick does not control the robot



Check:



```text

\- Joystick references in the Unity inspector.

\- UDP sender component configuration.

\- Command format generated by the script.

\- ROS 2 bridge logs.

\- CyberDog control topics.

```



\## Notes



The complete Unity project can be added later if full scene-level reproducibility is required. If the complete project is uploaded, generated folders should remain excluded through `.gitignore`, including:



```text

Library/

Temp/

Build/

Builds/

Logs/

UserSettings/

.vs/

```



For a lightweight reproducibility repository, the essential scripts and setup notes are sufficient to document the AR communication and interaction logic.

