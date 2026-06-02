\# Experimental Data



This folder contains the experimental data associated with the technical and user-centered evaluation of the augmented reality teleoperation architecture for Xiaomi CyberDog.



The data support the reproducibility of the main results obtained from the control channel, RGB/depth visual perception channels, and user-centered evaluation.



\## Data organization



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



\## Raw technical data



The `raw/` directory contains acquisition logs generated during the experimental sessions.



```text

raw/control/      Control-channel acquisition logs.

raw/rgb\_video/    RGB video-channel acquisition logs.

raw/depth\_video/  Depth video-channel acquisition logs.

```



These files may include timestamps, packet identifiers, frame identifiers, transmission times, reception times, display times, frame sizes, and technical variables required to reconstruct the reported metrics.



\## Processed data



The `processed/` directory contains cleaned and summarized tables derived from the raw acquisition files.



The processed tables are used to reproduce the descriptive statistics and summary results associated with:



```text

Control-channel latency

RGB video latency

Depth video latency

Packet or frame reception

Packet or frame loss

Effective frequency

Jitter

Video throughput

Global system performance

```



\## User evaluation data



The `user\_evaluation/` directory contains anonymized user-centered evaluation data.



The evaluation includes:



```text

NASA-TLX workload scores

SUS usability scores

Aggregated descriptive statistics

```



Participant-level files are provided only in anonymized form. Personal identifiers, names, emails, signatures, consent forms, and identification numbers are not part of the shared dataset.



\## Main reported results



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



The data structure separates technical acquisition logs, processed performance metrics, and anonymized user evaluation results.



Technical logs are included when they do not contain sensitive information. User-centered data are limited to anonymized or aggregated values.



This folder supports transparent reconstruction of the reported experimental results while preserving participant privacy.

