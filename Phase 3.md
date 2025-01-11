![image](https://github.com/user-attachments/assets/c8b594f2-fb0c-443b-9814-56455e4146c8)# Phase 3: Double Chasing Drones to Single Target Drone

The objective of Phase 3 is to train the 2 agents from phase 2 to work together to chase and catch a single target which is also an agent. This becomes a multi agent competitive scenario with two different teams (chasing and target agent) which compete with each other. Each time a team is training, the other team is relying on its inference (ie it is not training) and they take turns to train. Over time, this allows both teams to learn from each other and more behaviour can emerge.

- **Environment**: Additional obstacles in the environment and replacing the target cube into another trainable agent drone.
<img src="Media/Phase 3 Environment.JPG"  width = 600>
  
- **Reward System**: The reward system for the chasing agents are the same, but the new target agent has a different reward system. <br />

| Reward Value  | Purpose |
| ------------- | ------------- |
|-1  | Penalty for getting caught by the chaser agents |
| -1  |  Penalty for collisions with the environment |
| +1/episode length | An existential reward that encourages the target drone to not get caught. |
<br />
The target agent only has a positive reward which is to not get caught, the longer it does not get caught, the more reward it gets. It also has the addition of raycasts behind it, this is to allow it to "see" behind, and use these inputs to make decisions.
<img src="Media/Target Agent.jpg" >

# Results (0-2 Million Training Steps)
Please watch the results of phase 3 from 0-2 Million steps [here](https://youtu.be/ViTmpB69Gwk)
<br />
Early results showed that the chasing agents could easily track and catch the target agent. This was probably because the chasing agents had already been trained on stationary targets in phase 1 and phase 2 while the target agent was trained from scratch only in phase 3.

# Results (4-5 Million Training Steps)
Please watch the results of phase 3 from 4-5 Million steps [here](https://youtu.be/8kWEqvtb4mU)
<br />
Target Agent uses the obstacles to evade the chaser agents and the chaser agents fail to track the target after losing line of sight. This could be an exploration problem by the chaser agents

# Results (More than 6 Million Training Steps)
Please watch the results of phase 3 from 6 Million steps [here](https://youtu.be/Je3Mptr3Prw)
<br />
The simulation converged into a chasing scenario in a circle Chaser agents learn how to track and chase a moving target drone. It also seems that the target drone does not need to rely on the obstacles to evade the chasing drones. The chaser agents also adopt a staggered way of chasing the target drone and work together in some instances to anticipate the motion of the target drone


# Conclusion
The PPO algorithm was robust enough to create a drone chasing scenario with multiple agents and could create collaborative drone agents. The Multi-Agent competition also led to the evolution of more complex behaviour.
<img src="Media/Evolution of Behaviour.jpg"  width = 600>

