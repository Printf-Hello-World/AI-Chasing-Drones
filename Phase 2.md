
# Phase 1: Single Drone to Fixed Target

The objective of Phase 1 is to train a single drone to track and fly towards a fixed target in the environment.

- **Environment**: Closed room with obstacles and a stationary target cube.
- **Reward System**: Positive reward for reaching the target and keeping it in the drone's field of view, and penalties for collisions. <br />

| Reward Value  | Purpose |
| ------------- | ------------- |
|+1  | Reward for Reaching the Target |
| -1  |  Penalty for collisions with the environment |
| +1 | Reward for “looking” at the target (i.e., when a Ray hits the target)|
<br />
<img src="Media/Phase 1 initial Reward.JPG" >

# Initial Results
Please watch the initial results of phase 1 [here](https://youtu.be/-IEKjgh9jKM)
<br />
The Agent spirals around target instead of flying towards it fully, it exploited the reward system but led to undesired behaviour. The reward system has to be improved.

# Improved Reward
- **Reward System**: Positive reward for reaching the target and keeping it in the drone's field of view, and penalties for collisions, added an "existential" penalty which encourages the agent to reach the target faster. the distance and angle calculations were also improved.
  
| Reward Value  | Purpose |
| ------------- | ------------- |
|+1  | Reward for Reaching the Target |
| -1  |  Penalty for collisions with the environment |
| -1/Episode Length  | An existential penalty that encourages the drone to fly around, remaining stationary till the end of the episode will incur the greatest negative penalty  |
| -distance (normalised)  | Reward for approaching the target|
|-angle (normalised)  | Reward for turning towards the target direction  |

<img src="Media/Phase 1 Reward.jpg" >

# Results and Discussion
Please watch the results of phase 1 [here](https://youtu.be/j9JLnqrBFVs)
<br />
The agent does not memorize the spawn positions of the target cube and is able to fly towards the cube successfully and quickly during each episode. The same agent (neural network) will be used for phase 2.
