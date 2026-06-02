# Unity AR Interface

This folder contains the Unity-side scripts used to implement the augmented reality teleoperation interface for the Xiaomi CyberDog using Meta Quest 3.

The Unity application was designed as an operator-centered AR interface that integrates locomotion control, discrete robot actions, and real-time RGB/depth visual feedback within a single world-space HUD. Its purpose is to provide a compact and intuitive interaction layer between the human operator, the physical environment, and the quadruped robot.

## Interface concept

The AR interface was implemented in Unity as a World Space Canvas. This design allows the teleoperation panel to be placed within the operator’s augmented environment, maintaining a direct relationship between visual feedback, control elements, and the physical context of the task.

The HUD integrates three main components:

- A camera visualization panel for RGB or depth feedback.
- Two virtual joysticks for continuous locomotion control.
- Discrete action buttons for posture and gait commands.

This layout avoids the fragmentation of control and perception across multiple external screens or devices. Instead, the operator can command the robot and observe its visual feedback within the same augmented interface.

## Folder structure

```text
unity/
└── scripts/
    ├── CameraUdpReceiver.cs
    ├── DraggableWorldUIPanel.cs
    ├── JoystickCommandReader.cs
    ├── JoystickUdpSender.cs
    ├── UdpButtonTestUI.cs
    ├── UdpCommandSender.cs
    ├── UdpMoveHoldButton.cs
    └── VirtualJoystick.cs
```

## Main functions

The Unity-side implementation provides the following functions:

- AR HUD integration using a World Space Canvas.
- Virtual joystick-based locomotion control.
- UDP command transmission from Unity to the ROS 2 bridge.
- Discrete action buttons for posture and gait commands.
- Continuous movement commands while a UI button or joystick remains active.
- Transmission of a null command when the control returns to its neutral state.
- UDP video reception from the ROS 2 bridge.
- Visualization of compressed RGB/depth camera frames inside the AR HUD.
- Draggable world-space interface panel for flexible HUD positioning.

## Locomotion mapping

The locomotion interface uses two virtual joysticks:

```text
Left joystick:
  linear_x   → forward/backward motion
  angular_z  → yaw rotation

Right joystick:
  linear_y   → lateral motion
```

This mapping separates longitudinal/yaw motion from lateral displacement, providing a compact and understandable control scheme for the operator.

## Hybrid command transmission logic

The locomotion interface uses a hybrid transmission strategy.

Continuous motion commands are sent while the joystick remains displaced from its neutral position. When the joystick returns to the center, the system sends a single null command and stops transmitting redundant zero-velocity messages until a new interaction occurs.

This strategy reduces unnecessary network traffic while preserving a clear correspondence between the operator’s intention and the robot’s motion state.

## Discrete action commands

The interface includes discrete action buttons for predefined CyberDog behaviors. These commands are transmitted through UDP and interpreted by the ROS 2 bridge.

Typical actions include:

```text
stand up
lie down
sit
walk
run
predefined gait/action commands
```

The action codes are mapped in the ROS 2 bridge to the corresponding CyberDog gait or action messages.

## Communication

### Control channel

Unity sends UDP control commands to the Ubuntu bridge computer.

Default control port:

```text
5005
```

Message format:

```text
cmd:lx,ly,az
action:x
```

Examples:

```text
cmd:0.25,0.00,0.00
cmd:0.00,0.00,0.40
action:1
```

### Visual perception channel

Unity receives JPEG-compressed RGB or depth camera frames from the ROS 2 bridge through UDP.

Default video port:

```text
5006
```

The received byte stream is reconstructed as a Unity texture and displayed inside the AR HUD camera panel.

## Script description

```text
CameraUdpReceiver.cs       Receives JPEG-compressed RGB/depth video frames through UDP and displays them in Unity.
DraggableWorldUIPanel.cs   Allows the AR interface panel to be moved inside the Unity scene.
JoystickCommandReader.cs   Reads joystick values and converts them into motion command components.
JoystickUdpSender.cs       Sends joystick-based UDP commands to the ROS 2 bridge.
UdpButtonTestUI.cs         Provides basic UI tests for UDP button commands.
UdpCommandSender.cs        Sends discrete UDP commands, including robot action commands.
UdpMoveHoldButton.cs       Sends continuous movement commands while a UI button remains pressed.
VirtualJoystick.cs         Implements the virtual joystick interaction logic.
```

## Unity environment

The implementation was developed for:

```text
Unity 2022.3.4f1
Meta Quest 3
OpenXR
Android build target
World Space Canvas
UDP communication
AR-oriented teleoperation HUD
```

## Reproducibility notes

The complete Unity project is not included in this repository in order to keep the repository lightweight. Instead, the essential C# scripts and documentation are provided to reproduce the main interaction and communication logic of the AR interface.

Before running the Unity scene, the IP address of the Ubuntu bridge computer must be updated in the corresponding UDP sender scripts.

The Unity application must be built for Meta Quest 3 using the Android target platform and the required OpenXR configuration.

If a full reconstruction of the Unity scene is required, the complete Unity project files can be added later in a controlled way, excluding generated folders such as `Library/`, `Temp/`, `Build/`, `Logs/`, and `UserSettings/`.