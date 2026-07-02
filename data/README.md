# Experimental Data

This folder contains the experimental data associated with the technical, user-centered, communication, and robustness evaluation of the augmented reality teleoperation architecture for Xiaomi CyberDog.

The data are organized to support methodological transparency and independent reconstruction of the reported performance metrics. The folder includes raw acquisition logs, processed video records, user-evaluation files, network robustness datasets, and TCP-versus-UDP comparison data.

## Data organization

```text
data/
├── raw/
│   ├── control/
│   ├── rgb_video/
│   └── depth_video/
│
├── processed/
│   ├── rgb/
│   └── depth/
│
├── user_evaluation/
│
├── NetworkRobustness/
│   ├── C1_bandwidth_reduction_10Mbps/
│   ├── C2_packet_loss_3pct/
│   ├── C3_delay_100ms/
│   ├── C4_combined_5Mbps_100ms_3loss/
│   └── C5_distance_10m/
│
└── TCPvsUDP/
    ├── UDP/
    └── ROS_TCP_Endpoint/
```

## Raw technical data

The `raw/` directory contains the original acquisition logs generated during the main experimental sessions.

```text
raw/control/      Control-channel acquisition logs.
raw/rgb_video/    RGB video transmission and reception logs.
raw/depth_video/  Depth video transmission and reception logs.
```

The raw files include technical variables required to reconstruct the reported metrics, such as packet or frame identifiers, timestamps, transmission events, reception events, display events, and frame-size information.

## Control data

The control-channel logs are located in:

```text
data/raw/control/
```

The files include the commands generated from the Meta Quest 3 / Unity AR interface and the corresponding trace reconstructed at the bridge and robot side.

These data support the reconstruction of:

```text
Quest-bridge latency
Bridge internal processing time
Bridge-robot latency
End-to-end control latency
Packet reception rate
Effective command frequency
Jitter
```

## RGB video data

The RGB video logs are located in:

```text
data/raw/rgb_video/
data/processed/rgb/
```

The raw files contain the original sender and receiver logs. The processed files contain unified records used to reconstruct the reported RGB video metrics.

These data support the reconstruction of:

```text
RGB video latency
Frame reception rate
Frame loss rate
Effective display FPS
Jitter
Video transmission behavior
```

## Depth video data

The depth video logs are located in:

```text
data/raw/depth_video/
data/processed/depth/
```

The raw files contain the original sender and receiver logs. The processed files contain unified records used to reconstruct the reported depth video metrics.

These data support the reconstruction of:

```text
Depth video latency
Frame reception rate
Frame loss rate
Effective display FPS
Jitter
Video transmission behavior
```

## User-centered evaluation data

The `user_evaluation/` directory contains the user-centered evaluation files associated with workload and usability assessment.

```text
NASA-TLX  Workload evaluation.
SUS       System usability evaluation.
```

The user-evaluation files are included for methodological transparency. The shared files do not include direct participant-identifiable information such as names, emails, signatures, identification numbers, or consent forms.

## Network robustness data

The `NetworkRobustness/` directory contains additional control-channel experiments conducted under different network conditions.

```text
data/NetworkRobustness/
├── C1_bandwidth_reduction_10Mbps/
├── C2_packet_loss_3pct/
├── C3_delay_100ms/
├── C4_combined_5Mbps_100ms_3loss/
└── C5_distance_10m/
```

Each condition contains:

```text
quest_control_sent.csv
bridge_log.csv
robot_log.csv
metadata.yaml
```

These datasets support the evaluation of the UDP-based control channel under reduced bandwidth, induced packet loss, induced delay, combined degradation, and increased physical distance.

The original baseline experiment is not duplicated inside `NetworkRobustness/`, because the nominal control-channel behavior is documented in the main control dataset.

## TCP versus UDP comparison data

The `TCPvsUDP/` directory contains the experimental data associated with the comparison between the UDP-based control channel and the ROS-TCP-Endpoint-based control channel.

```text
data/TCPvsUDP/
├── UDP/
│   ├── quest_control_sent.csv
│   ├── bridge_or_ros_tcp_log.csv
│   ├── robot_control_recv.csv
│   └── metadata.yaml
│
└── ROS_TCP_Endpoint/
    ├── quest_control_sent.csv
    ├── bridge_or_ros_tcp_log.csv
    ├── robot_control_recv.csv
    └── metadata.yaml
```

These datasets support the reconstruction of communication metrics such as command latency, message propagation behavior, valid command correspondence, temporal stability, and effective command frequency for both communication approaches.

The comparison documents the practical transition from the initial ROS-TCP-Endpoint approach to the final UDP-based bridge architecture adopted in the AR-ROS 2 teleoperation system.

## Reproducibility scope

The data structure separates raw acquisition logs, processed technical records, user-centered evaluation files, network robustness experiments, and TCP-versus-UDP communication comparison data.

The raw and processed datasets allow independent verification of the communication behavior of the control channel and the RGB/depth perception channels. The user-evaluation files complement the technical data by documenting the workload and usability assessment associated with the AR teleoperation interface.

The `NetworkRobustness/` and `TCPvsUDP/` folders extend the reproducibility scope by documenting additional experimental evidence related to communication robustness and protocol-selection criteria for the final architecture.