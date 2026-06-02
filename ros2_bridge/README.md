# ROS 2 Bridge

This folder contains the ROS 2 bridge scripts used to connect the Unity/Meta Quest 3 interface with the Xiaomi CyberDog robot.

The bridge computer runs Ubuntu with ROS 2 Humble and translates UDP commands from Unity into CyberDog-compatible ROS 2 messages. It also forwards camera frames from ROS 2 image topics to the Meta Quest 3 through UDP.

## Folder structure

```text
ros2_bridge/
├── control/
│   └── udp_to_ros_bridge.py
└── video/
    └── camera_udp_sender.py
```

## Control bridge

The control bridge receives UDP commands from Unity and publishes them to CyberDog ROS 2 topics.

Main script:

```bash
python3 control/udp_to_ros_bridge.py
```

### UDP control input

The bridge listens on:

```text
0.0.0.0:5005
```

Supported UDP messages:

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

## CyberDog namespace

The CyberDog namespace used in the experiments was:

```text
/mi1036358
```

## CyberDog control topics

```text
/mi1036358/body_cmd
/mi1036358/cyberdog_action
```

## Main message types

```text
motion_msgs/msg/SE3VelocityCMD
motion_msgs/msg/ActionRequest
```

## Video sender

The video sender subscribes to a CyberDog camera topic, compresses each frame as JPEG, and sends it to the Meta Quest 3 through UDP.

Main script:

```bash
python3 video/camera_udp_sender.py
```

Default destination:

```text
Quest IP: 192.168.242.18
UDP port: 5006
```

Default active camera topic:

```text
/mi1036358/camera/color/image_raw
```

Prepared depth topics:

```text
/mi1036358/camera/aligned_depth_to_color/image_raw
/mi1036358/camera/depth/image_rect_raw
```

## Basic execution

Source ROS 2 Humble:

```bash
source /opt/ros/humble/setup.bash
```

Source the CyberDog interfaces:

```bash
source ~/cyberdog_if_ws/install/setup.bash
```

Run the control bridge from the repository root:

```bash
python3 ros2_bridge/control/udp_to_ros_bridge.py
```

Run the camera sender from the repository root:

```bash
python3 ros2_bridge/video/camera_udp_sender.py
```

## Notes

The CyberDog-specific message interfaces must be available in the ROS 2 workspace before running the bridge scripts.

The Quest IP address and UDP ports should be adjusted according to the active network configuration.

The current implementation uses two UDP ports:

```text
5005: Unity/Quest control commands to ROS 2 bridge
5006: CyberDog camera stream to Unity/Quest
```