\# Experimental Data



This folder contains the experimental data associated with the technical and user-centered evaluation of the AR teleoperation architecture for Xiaomi CyberDog.



The data are organized to support the reproducibility of the main results reported in the associated manuscript, including control-channel latency, RGB/depth video performance, packet or frame reception, effective frequency, jitter, and user-centered usability/workload metrics.



\## Data scope



The experimental evaluation was organized into two complementary levels:



1\. \*\*Objective technical evaluation\*\*

&#x20;  - Control-channel latency.

&#x20;  - Quest–bridge latency.

&#x20;  - Bridge internal processing time.

&#x20;  - Bridge–robot latency.

&#x20;  - End-to-end control latency.

&#x20;  - RGB video latency.

&#x20;  - Depth video latency.

&#x20;  - Packet or frame reception rate.

&#x20;  - Packet or frame loss rate.

&#x20;  - Effective frequency.

&#x20;  - Jitter.

&#x20;  - Video throughput.



2\. \*\*User-centered evaluation\*\*

&#x20;  - NASA-TLX workload scores.

&#x20;  - SUS usability scores.

&#x20;  - Aggregated descriptive statistics.



\## Suggested folder organization



```text

data/

├── raw/

│   ├── control/

│   ├── rgb\_video/

│   └── depth\_video/

│

├── processed/

│   ├── control\_latency\_summary.csv

│   ├── rgb\_video\_summary.csv

│   ├── depth\_video\_summary.csv

│   ├── packet\_frame\_statistics.csv

│   └── global\_performance\_summary.csv

│

└── user\_evaluation/

&#x20;   ├── nasa\_tlx\_anonymized.csv

&#x20;   ├── sus\_anonymized.csv

&#x20;   ├── nasa\_tlx\_summary.csv

&#x20;   └── sus\_summary.csv

```



\## Raw data



The `raw/` folder may include acquisition logs generated during the experimental sessions. These files can contain timestamps, packet identifiers, frame identifiers, transmission times, reception times, display times, frame sizes, or other technical variables required to reconstruct the reported metrics.



Recommended organization:



```text

raw/control/      Control-channel acquisition logs.

raw/rgb\_video/    RGB video-channel acquisition logs.

raw/depth\_video/  Depth video-channel acquisition logs.

```



\## Processed data



The `processed/` folder contains cleaned or summarized tables used to reproduce the main descriptive statistics and figures.



Recommended files:



```text

control\_latency\_summary.csv

rgb\_video\_summary.csv

depth\_video\_summary.csv

packet\_frame\_statistics.csv

global\_performance\_summary.csv

```



\## User evaluation data



The `user\_evaluation/` folder contains anonymized NASA-TLX and SUS results.



Only anonymized data should be included. Do not upload participant names, emails, identification numbers, signatures, consent forms, or any information that could directly identify a participant.



Recommended files:



```text

nasa\_tlx\_anonymized.csv

sus\_anonymized.csv

nasa\_tlx\_summary.csv

sus\_summary.csv

```



\## Main reported results



The following values summarize the main results reported in the manuscript:



```text

Control channel:

&#x20; Mean end-to-end latency: 9.37 ms

&#x20; P95 latency: 18.45 ms

&#x20; Reception rate: 100%



RGB video channel:

&#x20; Mean end-to-end latency: 21.09 ms

&#x20; Effective display rate: 17.39 FPS

&#x20; Frame reception rate: 91.10%



Depth video channel:

&#x20; Mean end-to-end latency: 26.99 ms

&#x20; Effective display rate: 15.98 FPS

&#x20; Frame reception rate: 91.10%



User evaluation:

&#x20; NASA-TLX global score: 22.20

&#x20; SUS score: 90.30

```



\## Privacy and reproducibility



Technical acquisition logs can be included when they do not contain sensitive information.



Participant-level questionnaire data must be anonymized before being uploaded. If raw user forms contain personal information, only processed or anonymized versions should be shared.



This folder is intended to improve the transparency and reproducibility of the reported experimental results.

