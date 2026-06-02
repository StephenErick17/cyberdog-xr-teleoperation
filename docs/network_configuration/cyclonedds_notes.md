\# Cyclone DDS Notes



This document summarizes the Cyclone DDS considerations used in the AR teleoperation architecture for Xiaomi CyberDog.



Cyclone DDS was used in the CyberDog ROS 2 environment to support stable communication between the robot and the external Ubuntu Bridge PC. The bridge computer must be able to discover and communicate with the CyberDog ROS 2 topics before running the UDP control and video scripts.



\## Role in the system



The ROS 2 communication layer is located between the Ubuntu Bridge PC and the Xiaomi CyberDog.



```text

Ubuntu Bridge PC

&#x20;       ↓ ROS 2 communication

Xiaomi CyberDog

```



The Unity AR interface does not communicate directly with Cyclone DDS. Instead, Unity communicates with the bridge computer through UDP, and the bridge computer publishes or subscribes to ROS 2 topics.



\## ROS 2 communication flow



For control:



```text

Unity AR interface

&#x20;       ↓ UDP 5005

udp\_to\_ros\_bridge.py

&#x20;       ↓ ROS 2 topic publication

CyberDog control topics

```



For visual feedback:



```text

CyberDog camera topics

&#x20;       ↓ ROS 2 subscription

camera\_udp\_sender.py

&#x20;       ↓ UDP 5006

Unity AR interface

```



\## Expected CyberDog topics



After sourcing the required ROS 2 environments, the bridge computer should be able to detect CyberDog topics such as:



```text

/mi1036358/body\_cmd

/mi1036358/cyberdog\_action

/mi1036358/camera/color/image\_raw

/mi1036358/camera/depth/image\_rect\_raw

/mi1036358/camera/aligned\_depth\_to\_color/image\_raw

```



Use:



```bash

ros2 topic list | grep mi1036358

```



to verify topic discovery.



\## Common warning



During execution, Cyclone DDS may show a warning similar to:



```text

config: //CycloneDDS/Domain/General: 'NetworkInterfaceAddress': deprecated element

```



This warning indicates that the configuration file uses an older Cyclone DDS XML element. It does not necessarily prevent ROS 2 communication, but it suggests that the configuration should be updated when possible.



\## Deprecated configuration example



An older configuration may include:



```xml

<NetworkInterfaceAddress>...</NetworkInterfaceAddress>

```



This element may trigger a deprecation warning in newer Cyclone DDS versions.



\## Recommended interpretation



If the CyberDog topics are visible and communication works correctly, this warning can usually be treated as non-critical during testing.



However, if ROS 2 discovery fails or topics are not visible, the DDS configuration should be reviewed together with the network configuration.



\## Basic verification procedure



1\. Source ROS 2 on the bridge computer:



```bash

source /opt/ros/humble/setup.bash

```



2\. Source the CyberDog message interfaces:



```bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



3\. Verify CyberDog topic discovery:



```bash

ros2 topic list | grep mi1036358

```



4\. Check camera topic frequency:



```bash

ros2 topic hz /mi1036358/camera/color/image\_raw

```



5\. Run the UDP-to-ROS 2 bridge:



```bash

python3 ros2\_bridge/control/udp\_to\_ros\_bridge.py

```



6\. Run the camera UDP sender:



```bash

python3 ros2\_bridge/video/camera\_udp\_sender.py

```



\## Troubleshooting



\### Topics are not visible



Check:



```text

\- CyberDog is powered on.

\- CyberDog and the Ubuntu Bridge PC are connected to the same network.

\- ROS 2 environments are sourced correctly.

\- The ROS\_DOMAIN\_ID is compatible between devices.

\- Cyclone DDS is configured for the correct network interface.

\- Firewall rules are not blocking ROS 2 discovery.

```



\### Camera topic is visible but video is not received in Unity



Check:



```text

\- camera\_udp\_sender.py is running.

\- The selected ROS 2 image topic is active.

\- The Meta Quest 3 IP address is correct in camera\_udp\_sender.py.

\- UDP port 5006 is not blocked.

\- Unity is listening on the expected video port.

```



\### Control bridge runs but robot does not move



Check:



```text

\- udp\_to\_ros\_bridge.py is receiving UDP packets.

\- Unity is sending commands to the correct Ubuntu Bridge PC IP.

\- UDP port 5005 is not blocked.

\- /mi1036358/body\_cmd is available.

\- /mi1036358/cyberdog\_action is available.

\- motion\_msgs interfaces are sourced correctly.

```



\## Notes



Cyclone DDS configuration depends on the active network interface and the ROS 2 environment used by the robot and the bridge computer.



For reproducibility, record the following information before each experimental session:



```text

\- Ubuntu Bridge PC IP address.

\- Xiaomi CyberDog IP address.

\- Meta Quest 3 IP address.

\- ROS\_DOMAIN\_ID value.

\- Active network interface.

\- Cyclone DDS configuration file path, if used.

```



This information helps diagnose communication issues and reproduce the AR teleoperation workflow under different network conditions.

