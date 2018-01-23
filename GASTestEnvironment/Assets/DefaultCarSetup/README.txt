Using the Default Car Setup:

InputManager
************
Copy the InputManager.asset file from the Input Setup folder. Use this to replace the existing InputManager.asset file in your project's ProjectSettings folder.

Controls
********
This control scheme can read input from a logitech steering wheel, using the gear shifter to change between forward and reverse gears.

Keyboard controls are as follows:
W - Acceleration
S - Brake
A - Left
D - Right
F - Forward gear
R - Reverse gear

Setup
*****
Use the prefab template as a starting point to test input controls. 

NOTE: The vehicle will always start in Neutral, so make sure to press F or R or use the gear shift to begin driving.

Modification
************
To modify the car model being used, simply replace the car body model and the four wheel models with different ones. Align the new model to the CarController object and add a collider to the new body model. Also make sure that the four wheels are place at the locations of the four wheel colliders. Then map the four wheel models to the script UI of the CarControlCS in the CarController object.

To modify the control parameters of the car controller, modify the values in the CarControlCS script UI.

To modify the wheel physics including friction, slip, etc, change the values in the wheel collider objects.