# ROS 2 Bridge

This folder contains the ROS 2 bridge scripts used to connect the Unity AR teleoperation interface running on Meta Quest 3 with the Xiaomi CyberDog robot.

The bridge computer acts as an intermediate communication, translation, and perception-processing layer. It receives UDP control packets from Unity, converts them into CyberDog-compatible ROS 2 messages, and forwards visual feedback from CyberDog camera topics to the AR interface through UDP video streaming.

## Role in the architecture

The bridge is the middleware layer between the AR client and the native CyberDog ROS 2 environment.

```text
Meta Quest 3 / Unity AR interface
        ↓ UDP control packets
Ubuntu Bridge PC
        ↓ ROS 2 control topics
Xiaomi CyberDog
```

For visual feedback, the data flow is reversed:

```text
Xiaomi CyberDog camera
        ↓ ROS 2 image topics
Ubuntu Bridge PC
        ↓ JPEG compression + UDP video stream
Meta Quest 3 / Unity AR interface
```

This decoupled design separates the control channel from the visual perception channel, making the system easier to debug, evaluate, and reproduce.

## Folder structure

```text
ros2_bridge/
├── control/
│   └── udp_to_ros_bridge.py
└── video/
    └── camera_udp_sender.py
```

## Control bridge

The control bridge receives UDP messages from the Unity AR interface and publishes them to the native CyberDog ROS 2 control topics.

Main script:

```bash
python3 ros2_bridge/control/udp_to_ros_bridge.py
```

The script listens for UDP packets on:

```text
0.0.0.0:5005
```

It supports two types of messages:

```text
cmd:lx,ly,az
action:x
```

Where:

```text
cmd:lx,ly,az   Continuous locomotion command.
action:x       Discrete posture or gait action command.
```

Examples:

```text
cmd:0.25,0.00,0.00
cmd:0.00,0.00,0.40
cmd:0.00,0.15,0.00
action:1
action:3
```

## Locomotion command mapping

The continuous command format is:

```text
cmd:lx,ly,az
```

The values are mapped to the CyberDog velocity command as follows:

```text
lx → linear_x
ly → linear_y
az → angular_z
```

These components are published through the CyberDog body command topic using the corresponding ROS 2 message type.

## Discrete action mapping

Discrete action commands are received as:

```text
action:x
```

The bridge maps each action code to a CyberDog gait or predefined action request. Typical commands include:

```text
stand up
lie down
sit
walk
run
predefined gait/action commands
```

The mapping is implemented inside:

```text
ros2_bridge/control/udp_to_ros_bridge.py
```

## CyberDog namespace

The CyberDog namespace used in the experimental implementation was:

```text
/mi1036358
```

## Main ROS 2 control topics

```text
/mi1036358/body_cmd
/mi1036358/cyberdog_action
```

## Main ROS 2 message types

```text
motion_msgs/msg/SE3VelocityCMD
motion_msgs/msg/ActionRequest
```

The CyberDog-specific message interfaces must be available in the ROS 2 workspace before running the bridge.

## Visual perception bridge

The visual perception bridge subscribes to CyberDog camera topics, converts ROS image messages into OpenCV frames using CvBridge, resizes the image, compresses each frame as JPEG, and sends the resulting byte stream to Unity through UDP.

Main script:

```bash
python3 ros2_bridge/video/camera_udp_sender.py
```

Default destination:

```text
Quest IP: 192.168.242.18
UDP port: 5006
```

The Quest IP address must be updated according to the active network configuration.

## Camera topics

Default RGB topic:

```text
/mi1036358/camera/color/image_raw
```

Prepared depth topics:

```text
/mi1036358/camera/aligned_depth_to_color/image_raw
/mi1036358/camera/depth/image_rect_raw
```

## Video processing pipeline

```text
ROS 2 image topic
        ↓
CvBridge conversion
        ↓
OpenCV frame processing
        ↓
Image resizing
        ↓
JPEG compression
        ↓
UDP transmission
        ↓
Unity AR HUD visualization
```

## Communication ports

```text
UDP 5005: Unity AR control commands to ROS 2 bridge.
UDP 5006: CyberDog RGB/depth visual feedback to Unity AR interface.
```

## Basic execution

Source ROS 2 Humble:

```bash
source /opt/ros/humble/setup.bash
```

Source the CyberDog message interfaces:

```bash
source ~/cyberdog_if_ws/install/setup.bash
```

Run the control bridge:

```bash
python3 ros2_bridge/control/udp_to_ros_bridge.py
```

Run the camera sender:

```bash
python3 ros2_bridge/video/camera_udp_sender.py
```

## Network notes

The experiments were conducted using a shared mobile wireless network. The reported available bandwidth was approximately:

```text
18.67 Mbps
```

Because the control and visual perception channels share the same network, video performance may vary depending on bandwidth, congestion, distance to the access point, and packet loss.

The control channel uses lightweight UDP packets, while the visual perception channel requires image conversion, JPEG compression, and frame transmission. For this reason, the visual channel is more sensitive to network restrictions.

## Middleware notes

The bridge computer used ROS 2 Humble on Ubuntu 22.04. The CyberDog platform operated with ROS 2 Foxy on its embedded NVIDIA Jetson Xavier NX platform.

Cyclone DDS was used in the CyberDog ROS 2 environment to support stable topic communication between the robot and the external bridge computer.

## Reproducibility notes

Before running the scripts, verify that:

```text
1. The CyberDog and the Ubuntu bridge computer are connected to the same network.
2. The Meta Quest 3 can reach the Ubuntu bridge computer IP address.
3. The CyberDog ROS 2 topics are visible from the bridge computer.
4. The CyberDog-specific message interfaces are sourced.
5. The Quest IP address is correctly configured in the video sender.
6. The bridge computer IP address is correctly configured in the Unity UDP sender scripts.
```

Useful ROS 2 commands:

```bash
ros2 topic list
ros2 topic echo /mi1036358/body_cmd
ros2 topic echo /mi1036358/cyberdog_action
ros2 topic hz /mi1036358/camera/color/image_raw
```

This folder provides the essential bridge scripts required to reproduce the control and visual perception communication logic of the AR teleoperation architecture.