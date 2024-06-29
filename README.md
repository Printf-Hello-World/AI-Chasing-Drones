# Chasing Drones Using Unity and ML-Agents

As part of my Final Year Project in University, I used ML-Agents and Unity to teach two drones to chase a single target drone using Reinforcement Learning. The RL method used was PPO. 

Unity Version: 2018.4.17
ML-Agents: Release 3

## Environment Set Up

![image](https://github.com/Printf-Hello-World/AI-Chasing-Drones/assets/59901029/fc1c1f39-0e27-4954-94d6-58da5b1fe94f)

-Closed room with obstacles
-Mesh colliders are used to detect collisions
-Each object is given a tag

## Agent Set Up

![image](https://github.com/Printf-Hello-World/AI-Chasing-Drones/assets/59901029/ed86b0f3-aede-44e2-9d27-187eb338aca5)
![image](https://github.com/Printf-Hello-World/AI-Chasing-Drones/assets/59901029/24e05bd4-abc7-4077-9d31-c48908a979ce)

-Agent uses rays to “see” environment
-Simulated sensor values of position, velocity and rotation are also used as input
-Actions are high-level actions like “pitch up”, “yaw right” etc
-Actions are floating point numbers between -1 

