
# Phase 2: Double Drones to Fixed Target

The objective of phase 2 is to train two agents to fly towards a fixed target without crashing into each other. The same agent model in phase 1 was used, a duplicate model was created for the second agent and training continued from the end of phase 1. There was no retraining of agents for phase 2.

- **Environment**: Closed room with obstacles and a stationary target cube and an addition of a **second** agent. 
 - **Reward System**: Same reward system as the first phase 1, only added a penalty for crashing into another agent. <br />

# Environment 

<img src="Media/Phase 2 Env.jpg" width=600>


# Results and Discussion
Please watch the results of phase 2 [here](https://youtu.be/-B1QpqQawYQ)
<br />
The agents avoid collisions with one another, they “Take-turns” to approach the target. Basic teamwork observed in this Phase. The additional training was shorter since learning was not from scratch

