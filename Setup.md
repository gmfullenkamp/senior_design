# Development Environment Setup

This document provides instructions for setting up the development environment required for our project. The setup involves installing Python, the Unity game engine, and several Python packages necessary for working with machine learning and Unity.

## Step 1: Install Python 3.8.9

1. Download Python 3.8.9 from the official [Python website](https://www.python.org/downloads/release/python-389/).
2. Run the installer and follow the on-screen instructions. Ensure you check the option to 'Add Python 3.8.9 to PATH' during installation.

## Step 2: Install the Newest Unity Version

1. Visit the [Unity Download Page](https://unity.com/download).
2. Download Unity Hub, which is a management tool for Unity versions and projects.
3. Install Unity Hub and open it.
4. Inside Unity Hub, navigate to the 'Installs' tab and click on the 'Add' button to add a new Unity version.
5. Choose the latest version of Unity for installation.

## Step 3: Set Up Python Virtual Environment

1. Open a command prompt or terminal.
2. Create a virtual environment for the project:
```cmd
python -m venv venv
```
3. Activate the virtual environment:
- On Windows:
  ```cmd
  venv\Scripts\activate
  ```
- On macOS\Linux:
  ```cmd
  source venv/bin/activate
  ```

## Step 4: Install Required Python Packages

1. Upgrade `pip` to the latest version:
```cmd
python -m pip install --upgrade pip
```
2. Install necessary Python packages:
```cmd
pip install torch mlagents packaging onnx protobuf==3.20.2
```
3. After completing these steps, your development environment will be ready for working on the project. If you encounter any issues, please refer to the respective documentation of Python, Unity, or the packages for troubleshooting.
