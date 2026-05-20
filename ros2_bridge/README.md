# ROS 2 Bridge

This folder contains the ROS 2 bridge scripts used to connect the Unity/Meta Quest 3 interface with the Xiaomi CyberDog robot.

The bridge computer runs Ubuntu with ROS 2 Humble and translates external commands into CyberDog-compatible ROS 2 messages.

## Main functions

- Receive locomotion commands from Unity or UDP.
- Publish velocity commands to the CyberDog body command topic.
- Publish discrete action commands to the CyberDog action topic.
- Forward camera frames from ROS 2 image topics to Unity through UDP.

## CyberDog namespace

The CyberDog namespace used in the experiments was:

```text
/mi1036358
```

## Main CyberDog topics

These are the main ROS 2 topics used to send commands to the real CyberDog.

```text
/mi1036358/body_cmd
/mi1036358/cyberdog_action
```

## Main message types

The bridge uses CyberDog-specific ROS 2 message types:

```text
motion_msgs/msg/SE3VelocityCMD
motion_msgs/msg/ActionRequest
```

## Camera topics

These are the main ROS 2 image topics used for the visual perception channel.

```text
/mi1036358/camera/color/image_raw
/mi1036358/camera/depth/image_rect_raw
/mi1036358/camera/aligned_depth_to_color/image_raw
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
python3 cyberdog_full_control.py
```

Run the video sender:

```bash
python3 camera_udp_sender.py
```

## Notes

The CyberDog-specific message interfaces must be available in the ROS 2 workspace before running the bridge scripts.