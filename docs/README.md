\# Methodology and Setup Documentation



This folder contains practical documentation for reproducing the augmented reality teleoperation methodology implemented with Meta Quest 3, Unity, UDP communication, ROS 2, and Xiaomi CyberDog.



The documentation focuses on the methodological and technical workflow required to configure, execute, and validate the system. It describes the communication channels, network configuration, bridge execution, CyberDog setup, and AR interface deployment.



\## Documentation scope



The folder is organized around three documentation areas:



```text

docs/

├── setup\_guides/

├── network\_configuration/

└── methodology\_notes/

```



\## Setup guides



The `setup\_guides/` folder contains configuration documents for the main system components:



```text

unity\_metaquest\_setup.md

ros2\_bridge\_setup.md

cyberdog\_setup.md

```



These documents describe the preparation of the Unity AR interface, the Ubuntu ROS 2 bridge, and the Xiaomi CyberDog platform.



\## Network configuration



The `network\_configuration/` folder contains technical notes related to communication and connectivity:



```text

udp\_ports.md

ip\_configuration.md

cyclonedds\_notes.md

```



These documents describe the UDP ports, IP address configuration, and Cyclone DDS considerations required for communication between Meta Quest 3, the Ubuntu Bridge PC, and CyberDog.



\## Methodology notes



The `methodology\_notes/` folder describes the methodological organization of the teleoperation system:



```text

teleoperation\_workflow.md

control\_channel.md

visual\_perception\_channel.md

```



These documents explain the complete AR teleoperation workflow, the control channel, and the visual perception channel.



\## AR teleoperation workflow



The system is structured into two decoupled communication channels:



```text

Control channel:

Meta Quest 3 / Unity AR interface

&#x20;       ↓ UDP 5005

Ubuntu Bridge PC

&#x20;       ↓ ROS 2 control topics

Xiaomi CyberDog

```



```text

Visual perception channel:

Xiaomi CyberDog camera

&#x20;       ↓ ROS 2 image topics

Ubuntu Bridge PC

&#x20;       ↓ JPEG compression + UDP 5006

Meta Quest 3 / Unity AR interface

```



This organization allows the control and visual feedback subsystems to be configured, tested, and evaluated independently.



\## Reproducibility purpose



The documentation in this folder supports the reproduction of the AR teleoperation architecture without requiring the full manuscript. It provides the technical context required to understand how Unity, Meta Quest 3, UDP communication, ROS 2, and CyberDog are integrated into a functional teleoperation workflow.



The folder is intended for methodological documentation, setup procedures, communication notes, and implementation details directly related to reproducing the system.

