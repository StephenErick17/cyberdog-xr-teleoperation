# Unity XR Interface

This folder contains the Unity-side components used to implement the immersive XR interface for CyberDog teleoperation with Meta Quest 3.

The Unity interface is responsible for generating locomotion commands, discrete action commands, and receiving the visual feedback stream displayed to the operator inside the headset.

## Main functions

- Virtual joystick-based locomotion control.
- Discrete action buttons for posture and gait commands.
- ROS 2 or UDP communication with the bridge computer.
- In-headset visualization of the robot camera stream.

## Main Unity topics

When using the ROS-TCP communication approach, the Unity interface publishes or subscribes to the following topics:

- `/unity/cyberdog/move`
- `/unity/cyberdog/action`
- `/unity_test`

## Unity version

- Unity 2022.3.4f1
- Meta Quest 3
- OpenXR
- Unity Robotics ROS-TCP Connector

## Notes

The complete Unity project is not included in this repository to keep it lightweight. Instead, the essential C# scripts and setup notes are provided to reproduce the main communication logic.
