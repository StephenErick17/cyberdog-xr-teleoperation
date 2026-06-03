# Experimental Data

This folder contains the experimental data associated with the technical and user-centered evaluation of the augmented reality teleoperation architecture for Xiaomi CyberDog.

The data are organized to support methodological transparency and independent reconstruction of the reported performance metrics. The folder includes raw acquisition logs, processed video records, and user-evaluation files related to NASA-TLX and SUS.

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
└── user_evaluation/
```

## Raw technical data

The `raw/` directory contains the original acquisition logs generated during the experimental sessions.

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

```

## Reproducibility scope

The data structure separates raw acquisition logs, processed technical records, and user-centered evaluation files.

The raw and processed datasets allow independent verification of the communication behavior of the control channel and the RGB/depth perception channels. The user-evaluation files complement the technical data by documenting the workload and usability assessment associated with the AR teleoperation interface.