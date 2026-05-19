# CyberDog XR Teleoperation

This repository contains the main materials associated with an immersive teleoperation system for the Xiaomi CyberDog using Meta Quest 3, Unity, UDP communication, and ROS 2.

The system integrates an XR-based operator interface, a ROS 2 bridge running on an Ubuntu PC, and a real quadruped robot executing locomotion commands, discrete actions, and visual feedback. The repository is intended to support the reproducibility of the experimental platform, the communication architecture, and the main results reported in the associated research work.

## System overview

The architecture is organized into two main communication channels:

1. **Control channel**  
   Meta Quest 3 → Unity XR Interface → UDP/ROS bridge → ROS 2 Humble → Xiaomi CyberDog with ROS 2 Foxy.

2. **Visual perception channel**  
   CyberDog camera → ROS 2 image topic → OpenCV/CvBridge → JPEG compression → UDP video sender → Unity receiver → Quest display.

## Hardware

- Meta Quest 3
- Xiaomi CyberDog
- Ubuntu bridge PC
- Shared mobile network connection

## Software

- Unity 2022.3.4f1
- OpenXR / Meta Quest support
- Unity Robotics ROS-TCP Connector
- ROS 2 Humble on the bridge PC
- ROS 2 Foxy on CyberDog
- Python 3
- OpenCV
- CvBridge

## Repository structure

```text
docs/              Architecture diagrams and paper-related figures.
unity/             Unity scripts for the XR interface.
ros2_bridge/       ROS 2 and UDP bridge scripts.
data/              Aggregated objective and subjective results.
analysis/          Python scripts for generating plots.
media/             Demo images or video links.
```

## Main ROS 2 topics

This section lists the main ROS 2 topics used in the system.

### Unity-side topics

These topics are generated from the Unity XR interface.

- `/unity/cyberdog/move`: locomotion commands generated from the virtual joystick or UI buttons.
- `/unity/cyberdog/action`: discrete action commands such as stand up, lie down, sit, slow gait, normal gait, and jump.
- `/unity_test`: basic communication test topic between ROS 2 and Unity.

### CyberDog-side topics

These topics are used by the CyberDog ROS 2 control interface.

- `/mi1036358/body_cmd`: velocity command topic used for robot locomotion.
- `/mi1036358/cyberdog_action`: action command topic used for discrete posture or gait actions.

### Camera topics

These topics provide the visual information used by the perception channel.

- `/mi1036358/camera/color/image_raw`: RGB camera stream.
- `/mi1036358/camera/depth/image_rect_raw`: depth camera stream.
- `/mi1036358/camera/aligned_depth_to_color/image_raw`: depth image aligned to the RGB frame.

## Basic execution

Source ROS 2 Humble:

```bash
source /opt/ros/humble/setup.bash
```

Source the CyberDog message interfaces:

```bash
source ~/cyberdog_if_ws/install/setup.bash
```

Run the ROS 2 bridge:

```bash
python3 ros2_bridge/cyberdog_full_control.py
```

Run the video sender:

```bash
python3 ros2_bridge/camera_udp_sender.py
```

## Experimental notes

The experiments were conducted using a shared mobile network with an available bandwidth of approximately 18.67 Mbps. Therefore, video performance, latency, packet reception, and jitter may vary under different network conditions.

## Reproducibility scope

This repository does not include the complete Unity project or all raw experimental logs. Instead, it provides the essential scripts, configuration notes, diagrams, processed data, and analysis files required to understand and reproduce the main components of the proposed system.

## Citation

If you use this repository, please cite the associated manuscript.