# Phase 1: Single Drone to Fixed Target

The objective of Phase 1 is to train a single drone to track and fly towards a fixed target in the environment.

- **Environment**: Closed room with obstacles and a stationary target cube.
- **Reward System**: Positive reward for reaching the target and keeping it in the drone's field of view, and penalties for collisions and taking too long, a time penalty is also imposed. <br />

| Reward Value  | Purpose |
| ------------- | ------------- |
|+1  | Reward for Reaching the Target |
| -1  |  Penalty for collisions with the environment |
| -1/Episode Length  | An existential penalty that encourages the drone to fly around, remaining stationary till the end of the episode will incur the greatest negative penalty  |
| -distance (normalised)  | Reward for approaching the target|
|-angle (normalised)  | Reward for turning towards the target direction  |

# Results and Discussion
Please watch the results after the phase 1 training phase [here](https://youtu.be/j9JLnqrBFVs)
<br />
The agent does not memorize the spawn positions of the target cube and is able to fly towards the cube successfully and consistently during each episode.



