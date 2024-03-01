# Development Environment Setup

Please refer to the [Development Environment Setup](Setup.md) document for detailed instructions on setting up your development environment in Python and Unity.

# Playing the Game

1. **Open Unity Project:**
- Open the Unity project after completing the setup steps.

2. **Play the Game:**
- Click the play button at the top center of the Unity window.
- Enjoy playing the Snake game!

# Training Machine Learning Agents

1. **Open Unity Project:**
- Open the Unity project after completing the setup steps.

2. **Open Command Prompt:**
- Open a command prompt after completing the setup steps and activating the environment.

3. **Run Training Command:**
-Run the following command to train ML agents:
'''
mlagents-learn Snake\Assets\ML-Agents\snake_training_config.yaml --run-id=<name_of_run>
'''

4. **Start Training:**
- Go to the Unity editor and click the play button at the top center of the window.
- Watch the ML agent learn and improve its performance.

5. **Analyze Results:**
- Navigate into the results folder from the command prompt.
- Run the following command to open the TensorBoard data of the training session:
'''
Tensorboard –logdir .
'''
- Open localhost:6006 in a web browser and look at the model’s training results.

# LLM Analysis

1. **Open Command Prompt:**
- Open a command prompt after completing the setup steps and activating the environment.
2. **Run LLM Game Analysis Command:**
- Run the following command to use chat-gpt to analyze a single game of Snake:
‘’’
python senior_design/LLM/chat.py
‘’’

## Additional Suggestions

For any further assistance or troubleshooting, please refer to the respective documentation of Python, Unity, or the installed packages.
