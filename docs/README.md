# Methodology and Setup Documentation

This folder contains methodological and technical documentation for reproducing the augmented reality teleoperation architecture implemented with Meta Quest 3, Unity, UDP communication, ROS 2, and Xiaomi CyberDog.

The documentation focuses on the configuration, execution, and interpretation of the system architecture. It includes setup guides, network configuration notes, communication workflows, and selected visual reference materials that support the understanding of the implemented AR teleoperation methodology.

## Documentation structure

```text
docs/
├── setup_guides/
├── network_configuration/
└── methodology_notes/
```

## Setup guides

The `setup_guides/` folder contains configuration documents for the main system components:

```text
unity_metaquest_setup.md
ros2_bridge_setup.md
cyberdog_setup.md
```

These documents describe the preparation of the Unity AR interface, the Ubuntu ROS 2 bridge, and the Xiaomi CyberDog platform.

## Network configuration

The `network_configuration/` folder contains technical notes related to communication and connectivity:

```text
udp_ports.md
ip_configuration.md
cyclonedds_notes.md
```

These documents describe the UDP ports, IP address configuration, and Cyclone DDS considerations required for communication between Meta Quest 3, the Ubuntu Bridge PC, and CyberDog.

## Methodology notes

The `methodology_notes/` folder describes the methodological organization of the AR teleoperation system:

```text
teleoperation_workflow.md
control_channel.md
visual_perception_channel.md
```

This folder also contains selected reference images that help explain the implemented system, such as the general architecture diagram and the AR interface layout.

## System reference materials

The visual materials included in this documentation clarify the implementation methodology and system organization. They include the general AR–ROS 2 teleoperation architecture, the Unity AR interface layout, control and perception workflow references, and system configuration views.

These materials support the reproducibility of the implementation by showing how the main components are organized and connected.

## AR teleoperation workflow

The system is structured into two decoupled communication channels:

```text
Control channel:
Meta Quest 3 / Unity AR interface
        ↓ UDP 5005
Ubuntu Bridge PC
        ↓ ROS 2 control topics
Xiaomi CyberDog
```

```text
Visual perception channel:
Xiaomi CyberDog camera
        ↓ ROS 2 image topics
Ubuntu Bridge PC
        ↓ JPEG compression + UDP 5006
Meta Quest 3 / Unity AR interface
```

This organization allows the control and visual feedback subsystems to be configured, tested, and evaluated independently.

## Reproducibility purpose

The documentation in this folder supports the reproduction of the AR teleoperation architecture without requiring the full manuscript.

It provides the technical context required to understand how Unity, Meta Quest 3, UDP communication, ROS 2, and CyberDog are integrated into a functional teleoperation workflow.

The folder is oriented toward methodological documentation, setup procedures, communication notes, and implementation reference material directly related to reproducing the system.