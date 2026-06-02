\# Analysis Scripts



This folder contains the Python scripts used to process the experimental data and reproduce the main descriptive analyses and figures associated with the AR teleoperation architecture for Xiaomi CyberDog.



The analysis workflow is intended to support reproducibility of the technical and user-centered results reported in the associated manuscript.



\## Analysis scope



The scripts in this folder are intended to process data from the following evaluation components:



1\. \*\*Control channel\*\*

&#x20;  - Quest–bridge latency.

&#x20;  - Bridge internal processing time.

&#x20;  - Bridge–robot latency.

&#x20;  - End-to-end control latency.

&#x20;  - Packet reception rate.

&#x20;  - Effective command frequency.

&#x20;  - Jitter.



2\. \*\*RGB video channel\*\*

&#x20;  - Emitter–Quest latency.

&#x20;  - Quest reception–display latency.

&#x20;  - End-to-end RGB latency.

&#x20;  - Frame reception rate.

&#x20;  - Effective display FPS.

&#x20;  - Jitter.

&#x20;  - Frame loss.



3\. \*\*Depth video channel\*\*

&#x20;  - Emitter–Quest latency.

&#x20;  - Quest reception–display latency.

&#x20;  - End-to-end depth latency.

&#x20;  - Frame reception rate.

&#x20;  - Effective display FPS.

&#x20;  - Jitter.

&#x20;  - Frame loss.



4\. \*\*User-centered evaluation\*\*

&#x20;  - NASA-TLX descriptive statistics.

&#x20;  - SUS descriptive statistics.

&#x20;  - Workload and usability summary plots.



\## Suggested script organization



```text

analysis/

├── control/

│   ├── analyze\_control\_latency.py

│   └── plot\_control\_latency.py

│

├── video/

│   ├── analyze\_rgb\_video.py

│   ├── analyze\_depth\_video.py

│   └── plot\_video\_comparison.py

│

├── user\_evaluation/

│   ├── analyze\_nasa\_tlx.py

│   ├── analyze\_sus.py

│   └── plot\_user\_evaluation.py

│

└── global\_summary/

&#x20;   └── plot\_global\_latency\_comparison.py

```



\## Input data



The scripts are expected to read experimental data from the `data/` folder.



Recommended input locations:



```text

data/raw/control/

data/raw/rgb\_video/

data/raw/depth\_video/

data/processed/

data/user\_evaluation/

```



\## Output files



Generated plots and processed tables can be exported to:



```text

analysis/output/

```



Recommended output examples:



```text

control\_latency\_boxplot.pdf

rgb\_depth\_latency\_comparison.pdf

nasa\_tlx\_scores.pdf

sus\_response\_distribution.pdf

global\_latency\_comparison.pdf

```



\## Python environment



Recommended Python packages:



```text

python >= 3.8

numpy

pandas

matplotlib

scipy

```



Optional packages:



```text

openpyxl

```



\## Basic usage



From the repository root:



```bash

python analysis/control/analyze\_control\_latency.py

python analysis/video/analyze\_rgb\_video.py

python analysis/video/analyze\_depth\_video.py

python analysis/user\_evaluation/analyze\_nasa\_tlx.py

python analysis/user\_evaluation/analyze\_sus.py

```



\## Reproducibility notes



The analysis scripts should preserve the same metric definitions reported in the manuscript:



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

```



For visual consistency, generated figures should use clear labels, readable axes, and export formats suitable for academic publication, such as PDF, PNG, or SVG.



Participant-level user evaluation files must be anonymized before being processed or shared.

