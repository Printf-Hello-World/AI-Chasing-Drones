# Chasing Drones Using Unity and ML-Agents

As part of my Final Year Project in University, I used ML-Agents and Unity to teach two drones to chase a single target drone using Reinforcement Learning. The RL method used was PPO. 

Unity Version: 2018.4.17
ML-Agents: Release 3

## Environment Set Up

<img src="https://github.com/Printf-Hello-World/AI-Chasing-Drones/assets/59901029/fc1c1f39-0e27-4954-94d6-58da5b1fe94f" width="500">
<img src="https://github.com/Printf-Hello-World/AI-Chasing-Drones/assets/59901029/2b275b76-d105-4f37-8bd2-ec293cc0059b" width="500">
<br />Closed room with obstacles. Mesh colliders are used to detect collisions and each object is given a tag

## Agent Set Up
<img src="https://github.com/Printf-Hello-World/AI-Chasing-Drones/assets/59901029/ed86b0f3-aede-44e2-9d27-187eb338aca5" width="500">
<img src="https://github.com/Printf-Hello-World/AI-Chasing-Drones/assets/59901029/06d45d50-8be6-4b1e-95a3-31bb3248b0b9" width="500">
<br />Agent uses rays to “see” environment, simulated sensor values of position, velocity and rotation are also used as input. Actions are high-level actions like “pitch up”, “yaw right” etc. Actions are floating point numbers between -1 


