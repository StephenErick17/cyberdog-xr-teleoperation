\# Visual Perception Channel



This document describes the visual perception channel used in the augmented reality teleoperation architecture for Xiaomi CyberDog.



The visual perception channel transmits RGB or depth feedback from the CyberDog camera system to the Unity AR interface running on Meta Quest 3. This channel is independent from the control channel and is used to provide real-time visual feedback to the operator inside the AR HUD.



\## Visual perception channel overview



```text

Xiaomi CyberDog camera

&#x20;       ↓ ROS 2 image topics

Ubuntu Bridge PC

&#x20;       ↓ OpenCV/CvBridge + JPEG compression + UDP 5006

Meta Quest 3 / Unity AR interface

```



The perception channel is responsible for:



```text

\- Subscribing to CyberDog RGB or depth image topics.

\- Converting ROS 2 image messages into OpenCV-compatible frames.

\- Resizing the image for network transmission.

\- Compressing each frame as JPEG.

\- Sending the compressed frame through UDP.

\- Receiving the frame in Unity.

\- Reconstructing the frame as a texture.

\- Displaying the visual feedback inside the AR HUD.

```



\## Main implementation files



ROS 2 bridge-side script:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



Unity-side script:



```text

unity/scripts/CameraUdpReceiver.cs

```



\## Camera topics



The implementation considers the following CyberDog camera topics:



```text

/mi1036358/camera/color/image\_raw

/mi1036358/camera/depth/image\_rect\_raw

/mi1036358/camera/aligned\_depth\_to\_color/image\_raw

```



The default RGB topic is:



```text

/mi1036358/camera/color/image\_raw

```



Depth visualization can be enabled by changing the active topic and mode inside:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



\## Video sender pipeline



The camera sender performs the following sequence:



```text

1\. Subscribe to the selected CyberDog ROS 2 image topic.

2\. Convert the ROS 2 image message using CvBridge.

3\. Convert or normalize the frame depending on RGB or depth mode.

4\. Resize the frame to the configured output resolution.

5\. Compress the frame as JPEG.

6\. Send the JPEG byte stream to Meta Quest 3 through UDP.

```



\## Default video configuration



The default configuration used during development was:



```text

Output resolution: 320 x 240

JPEG quality: 55

UDP video port: 5006

Default mode: COLOR

Default topic: /mi1036358/camera/color/image\_raw

```



The Quest IP address must be updated in:



```text

ros2\_bridge/video/camera\_udp\_sender.py

```



Example:



```python

self.quest\_ip = '192.168.242.18'

self.quest\_port = 5006

```



\## RGB mode



In RGB mode, the sender subscribes to the RGB camera topic and converts the image into a format compatible with OpenCV and JPEG encoding.



Default RGB topic:



```text

/mi1036358/camera/color/image\_raw

```



The resulting JPEG frame is sent to Unity through UDP port `5006`.



\## Depth mode



Depth mode can be enabled by selecting one of the depth topics:



```text

/mi1036358/camera/aligned\_depth\_to\_color/image\_raw

/mi1036358/camera/depth/image\_rect\_raw

```



When depth frames are received, the sender normalizes the depth image and applies a color map for visualization before JPEG compression.



This allows the operator to perceive relative distance information inside the AR interface.



\## Unity video reception



On the Unity side, the video receiver listens for UDP packets on port `5006`.



The receiver reconstructs the JPEG byte stream as a Unity texture and displays it in the AR HUD camera panel.



```text

UDP packet

&#x20;       ↓

JPEG byte stream

&#x20;       ↓

Texture reconstruction

&#x20;       ↓

RawImage / camera panel

&#x20;       ↓

AR HUD visualization

```



\## Communication port



The visual perception channel uses:



```text

UDP 5006

```



Direction:



```text

Ubuntu Bridge PC → Meta Quest 3 / Unity

```



\## Basic execution



From the Ubuntu Bridge PC, source ROS 2:



```bash

source /opt/ros/humble/setup.bash

```



Source the CyberDog message interfaces:



```bash

source \~/cyberdog\_if\_ws/install/setup.bash

```



Verify that the camera topic is available:



```bash

ros2 topic list | grep camera

```



Check RGB camera frequency:



```bash

ros2 topic hz /mi1036358/camera/color/image\_raw

```



Run the camera UDP sender:



```bash

python3 ros2\_bridge/video/camera\_udp\_sender.py

```



Then launch the Unity AR application on Meta Quest 3 and verify that the camera stream appears inside the AR HUD.



\## Validation procedure



Recommended validation order:



```text

1\. Verify that CyberDog camera topics are visible.

2\. Verify that the selected image topic is publishing frames.

3\. Confirm that CvBridge and OpenCV are available on the bridge computer.

4\. Update the Meta Quest 3 IP address in camera\_udp\_sender.py.

5\. Run camera\_udp\_sender.py.

6\. Verify that frames are being processed and sent.

7\. Launch the Unity AR application.

8\. Verify that CameraUdpReceiver.cs receives the stream.

9\. Confirm that the frame is displayed inside the AR HUD.

10\. Test visual feedback while the control channel is also active.

```



\## Useful ROS 2 commands



List camera topics:



```bash

ros2 topic list | grep camera

```



Check RGB camera frequency:



```bash

ros2 topic hz /mi1036358/camera/color/image\_raw

```



Check RGB camera topic information:



```bash

ros2 topic info /mi1036358/camera/color/image\_raw

```



Check depth camera topic information:



```bash

ros2 topic info /mi1036358/camera/aligned\_depth\_to\_color/image\_raw

```



Check available camera services:



```bash

ros2 service list | grep camera

```



\## Network considerations



The visual perception channel is more demanding than the control channel because it transmits image data.



Its performance depends on:



```text

\- Available bandwidth.

\- Frame resolution.

\- JPEG quality.

\- Packet size.

\- Network congestion.

\- Distance to the wireless access point.

\- Simultaneous use of the control channel.

\- Processing load on the bridge computer.

\- Frame reconstruction and display time in Unity.

```



During the reported experiments, the system operated over a shared mobile wireless network with an available bandwidth of approximately `18.67 Mbps`.



\## Troubleshooting



\### No image appears in Unity



Check:



```text

\- camera\_udp\_sender.py is running.

\- Unity is listening on UDP port 5006.

\- The Meta Quest 3 IP address is correct in camera\_udp\_sender.py.

\- The selected CyberDog image topic is publishing frames.

\- The video panel is correctly assigned in the Unity scene.

\- Firewall rules are not blocking UDP traffic.

```



\### Camera topic is not available



Check:



```text

\- CyberDog is powered on.

\- CyberDog camera system is active.

\- ROS 2 communication between CyberDog and the bridge computer is working.

\- Cyclone DDS configuration is correct.

\- The camera enable service is available, if required.

```



\### Video is delayed or unstable



Check:



```text

\- Network bandwidth.

\- JPEG quality.

\- Output resolution.

\- Wireless signal strength.

\- Number of connected devices.

\- Processing load on the bridge computer.

\- Whether RGB and depth modes are being tested separately or simultaneously.

```



\## Notes



The visual perception channel is independent from the control channel. Therefore, a temporary degradation in visual feedback should not directly block command transmission.



For more stable visual feedback, consider:



```text

\- Using a dedicated wireless network.

\- Reducing output resolution.

\- Adjusting JPEG quality.

\- Implementing adaptive compression.

\- Monitoring packet loss and frame reception rate.

\- Prioritizing the control channel when network capacity is limited.

```



This channel provides the visual basis for AR teleoperation and allows the operator to observe the CyberDog environment directly inside the Meta Quest 3 interface.

