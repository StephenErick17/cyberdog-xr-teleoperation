\# Methodology and Setup Documentation



This folder contains practical documentation for reproducing the AR teleoperation methodology implemented with Meta Quest 3, Unity, UDP communication, ROS 2, and Xiaomi CyberDog.



The purpose of this folder is not to store paper figures or publication graphics. Instead, it provides setup guides, communication notes, network configuration details, and methodological descriptions required to reproduce the teleoperation workflow.



\## Documentation scope



The documentation in this folder is oriented toward implementation and reproducibility. It may include:



\- Unity and Meta Quest 3 setup notes.

\- ROS 2 bridge configuration.

\- CyberDog topic and interface notes.

\- UDP communication configuration.

\- Network setup and IP configuration.

\- Cyclone DDS notes.

\- Control-channel workflow.

\- Visual perception-channel workflow.

\- Experimental protocol notes.



\## Suggested folder organization



```text

docs/

├── setup\_guides/

│   ├── unity\_setup.md

│   ├── meta\_quest\_setup.md

│   ├── ros2\_bridge\_setup.md

│   └── cyberdog\_setup.md

│

├── network\_configuration/

│   ├── udp\_ports.md

│   ├── ip\_configuration.md

│   └── cyclonedds\_notes.md

│

└── methodology\_notes/

&#x20;   ├── control\_channel.md

&#x20;   ├── visual\_perception\_channel.md

&#x20;   └── teleoperation\_workflow.md

```



\## Methodological orientation



The repository documents a modular AR teleoperation workflow composed of two decoupled channels:



```text

Control channel:

Meta Quest 3 / Unity AR interface

&#x20;       ↓ UDP 5005

ROS 2 bridge

&#x20;       ↓ ROS 2 control topics

Xiaomi CyberDog

```



```text

Visual perception channel:

Xiaomi CyberDog camera

&#x20;       ↓ ROS 2 image topics

ROS 2 bridge

&#x20;       ↓ JPEG compression + UDP 5006

Meta Quest 3 / Unity AR interface

```



This structure allows the command flow and the visual feedback flow to be configured, tested, and evaluated independently.



\## What should be documented here



Use this folder for practical files such as:



```text

How to configure the Meta Quest 3 application.

How to set the bridge computer IP address in Unity.

How to run the UDP-to-ROS 2 bridge.

How to configure the camera UDP sender.

How to verify ROS 2 topics.

How to test UDP communication.

How to reproduce the teleoperation workflow.

```



\## What should not be stored here



Avoid using this folder as a repository of manuscript-only material, such as:



```text

Final paper figures.

Temporary diagram exports.

Unrelated screenshots.

Large videos.

Draft images used only for publication layout.

```



If a figure or diagram is necessary to reproduce the system architecture or setup, it can be included. However, the priority of this folder is methodological documentation, not manuscript formatting.



\## Reproducibility notes



The documentation should help another researcher or developer reproduce the architecture without needing to inspect the full paper.



When implementation details change, the corresponding setup or methodology notes should be updated to remain consistent with the Unity scripts and ROS 2 bridge scripts.

