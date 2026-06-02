\# Analysis Scripts



This folder contains the analysis workflow used to process the experimental data associated with the augmented reality teleoperation architecture for Xiaomi CyberDog.



The scripts process technical acquisition logs and user-centered evaluation data to reproduce the descriptive metrics and summary figures associated with the control channel, RGB/depth visual perception channels, NASA-TLX workload evaluation, and SUS usability evaluation.



\## Analysis organization



```text

analysis/

├── control/

├── video/

├── user\_evaluation/

├── global\_summary/

└── output/

```



\## Control-channel analysis



The `control/` directory contains scripts for processing the command transmission logs.



The control-channel analysis includes:



```text

Quest–bridge latency

Bridge internal processing time

Bridge–robot latency

End-to-end control latency

Packet reception rate

Effective command frequency

Jitter

```



\## Video-channel analysis



The `video/` directory contains scripts for processing RGB and depth visual feedback logs.



The video-channel analysis includes:



```text

Emitter–Quest latency

Quest reception–display latency

End-to-end RGB latency

End-to-end depth latency

Frame reception rate

Frame loss rate

Effective display FPS

Jitter

Video throughput

```



\## User-centered evaluation analysis



The `user\_evaluation/` directory contains scripts for processing anonymized NASA-TLX and SUS data.



The user-centered analysis includes:



```text

NASA-TLX subscale statistics

NASA-TLX global score

SUS global score

SUS item-level response distribution

Descriptive statistics for workload and usability

```



\## Global summary analysis



The `global\_summary/` directory contains scripts for generating comparative summaries across the main system channels.



The global analysis includes:



```text

Control latency summary

RGB video latency summary

Depth video latency summary

Global end-to-end latency comparison

Technical and user-centered result synthesis

```



\## Input data



The analysis scripts read data from the `data/` directory.



```text

data/raw/control/

data/raw/rgb\_video/

data/raw/depth\_video/

data/processed/

data/user\_evaluation/

```



\## Output files



Generated tables and figures are exported to:



```text

analysis/output/

```



Output files may include:



```text

control\_latency\_boxplot.pdf

rgb\_depth\_latency\_comparison.pdf

nasa\_tlx\_scores.pdf

sus\_response\_distribution.pdf

global\_latency\_comparison.pdf

summary\_tables.csv

```



\## Python environment



The analysis workflow uses Python and common scientific computing libraries:



```text

python >= 3.8

numpy

pandas

matplotlib

scipy

openpyxl

```



\## Metric definitions



The analysis preserves the metric definitions used in the experimental evaluation:



```text

Mean

Standard deviation

Median

95th percentile

Minimum

Maximum

Reception rate

Loss rate

Effective frequency

Jitter

Throughput

```



\## Reproducibility scope



This folder provides the computational layer required to reconstruct the reported descriptive results from the acquisition logs and anonymized user evaluation data.



The generated outputs are intended for technical verification, methodological transparency, and academic reporting of the AR teleoperation system.

