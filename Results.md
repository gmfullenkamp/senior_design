# Project Results and Development Process

## Overview

This document details the journey of developing a reinforcement learning (RL) agent for a 2D snake game in Unity, outlining the challenges faced and the solutions implemented.

## Phase 1: Game Development in Unity

### Initial Game Setup
- **Environment**: Created a new 2D game environment in Unity.
- **Basic Features**:
  - **Walls**: Implemented boundary walls to define the play area.
  - **Snake Mechanics**: Developed the snake's movement and growth mechanics, expanding its length each time it eats food.
  - **Food**: Added food items that randomly spawn within the game area.
  - **Collision**: Integrated collision detection to handle interactions between the snake, walls, and food.

### Implementation Details
- **Programming Language**: Utilized C# for game development.
- **Key Challenges**: Ensuring smooth movement and accurate collision detection.
- [Snake Game Play Example](Snake_Example.png)

## Phase 2: Integrating ML-Agents

### Setting Up ML-Agents
- **Package Installation**: Installed the ML-Agents package in Unity.
- **Adaptation**: Modified the game code to accommodate ML-Agents' syntax and requirements.

### Initial Training Setup
- **Environment Dimensions**: Initially set the training environment to a 60x40 grid.
- **Primary Issue**: The large environment led to excessive randomness, hindering effective learning.

### Adjustments and Improvements
- **Map Size Reduction**: Scaled down the environment to reduce randomness and encourage learning about boundaries.
- **Reward System**:
  - **Positive Reward**: +1 for consuming food.
  - **Negative Reward**: -10 for hitting the snake's own body or walls.
- **Observation**: The initial reward system did not sufficiently motivate the snake to actively seek food.

### Advanced Reward Strategy
- **Proximity-Based Reward**: 
  - +0.04 for moving closer to the food.
  - -0.05 for moving away from the food.
- **Outcome**: This strategy encouraged the snake to actively search for food while avoiding walls.

### Training Evolution
- **Initial Training**: Conducted on a smaller 5x5 grid, achieving a high score of 7 food items consumed.
- **Image Placeholder**: *\[Add training process image here\]*

## Phase 3: Fine-Tuning and Scaling Up

### Expanded Training
- **Larger Map**: Transitioned to a 25x25 grid to test the agent's performance in a more extensive environment.
- **Ongoing Process**: Currently fine-tuning the model on this larger scale to enhance its adaptability and decision-making skills.

## Future Improvements and Advanced Development

### Dynamic Game Difficulty Using LLMs

#### Concept Overview
- **Objective**: To evolve the game environment dynamically, making it more challenging as the RL agent's performance improves.
- **Use of LLMs**: Implementing Large Language Models to analyze the agent's gameplay patterns and adjust game variables accordingly.

#### Implementation Strategy
- **Game Analysis**: Utilize LLMs to interpret the RL agent's strategies and identify areas of proficiency and weakness.
- **Dynamic Adjustments**: Based on the analysis, the game's difficulty will be adjusted in real-time. This could include changing the size of the play area, increasing the speed of the snake, or introducing new obstacles.
- **Feedback Loop**: Establish a continuous feedback mechanism where the RL agent's performance is constantly monitored and used to inform the LLM-driven adjustments.

### Anticipated Challenges
- **Complexity of Integration**: Seamlessly integrating LLMs with the current Unity and ML-Agents framework may present technical challenges.
- **Balance in Difficulty Scaling**: Ensuring that the difficulty adjustments are challenging yet achievable to prevent discouraging the RL agent's learning process.
- **Performance Metrics**: Defining clear metrics to evaluate the success of these dynamic adjustments.

### Long-Term Goals
- **Enhanced AI Adaptability**: Aim to develop an RL agent that can continuously learn and adapt to ever-changing game environments.
- **Innovative Gaming Experience**: Create a unique gaming experience where the game evolves in complexity in response to the player's (or AI's) skill level, potentially paving the way for advanced AI training methodologies and more engaging game designs.

### Potential Research Applications
- This approach may offer insights into adaptive learning systems and their applications in broader areas, such as educational software, adaptive user interfaces, and more.

As we look to the future, these advancements aim not only to elevate the project's technical prowess but also to contribute to the field of AI and game development, demonstrating the practical applications and potential of adaptive AI systems.

## Summary

As of the conclusion of the first semester, significant progress has been made in training a reinforcement learning agent to play a snake game effectively. The journey has involved iterative development, constant fine-tuning of the training environment and reward system, and adapting to the challenges that emerged. The project continues to evolve, with future efforts focused on refining the agent's performance in more complex environments.

