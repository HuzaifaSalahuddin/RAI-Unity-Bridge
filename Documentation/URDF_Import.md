# How to use URDF Importer

## Basic URDF Import Steps
1. Download the Unity Editor 2022.3.41f1. The earlier versions don't seem to work with these specs.
2. Install the URDF importer unity package by following instructions [here](https://github.com/Unity-Technologies/URDF-Importer).
3. Place the urdf folder in [correct location](https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/urdf_importer/urdf_appendix.md#file-hierarchy).
4. Open Urdf file of your model and make sure the line pointing to location of mesh files relative to urdf file is correct. If you see something like "model://", remove it and rectify the location.
5. Import the URDF following the instructions from github and make sure "Articulation Body" component is assigned to it inside unity.
6. Make sure that the imported Game object contains "world" somewhere in the heirarchy. If not, then manually do it. All the robot links (including base link/ base footprint) should be the children of world frame.

## Running URDF Simulation with URDF Importer Package
7. Now, in order to avoid undesired behaviour of robot, you need to follow the following steps:
        - Disable "Gravity" from inspector's tab after clicking on the "robot name" you imported in the heirarchy tab.
        - Set the root of articulation body to "immovable".
        - In the parent and all the children of the robot articulation body, set the 'drive type' to "target" manually for all of them.
        - Click on the "robot name" you imported from heirarchy tab and then in the inspector's tab under the Controller (Script) section of it, set all the parameters to 0 (In case of manual control, you would be required to change the values of "Force limit" and "Speed").
8. If you write a script for robot, make sure to drag the script from Assets and attach it to your robot articulation body in the heirarchy tab. In case of successful attachment, you should be able to see the script in inspector's tab when clicked on your robot in heirarchy tab.
9. In the top left menu "within the scene window", make sure the rotation is set to "pivot" instead of "center".
10. Now if you press the run button, you should be able to control your robot using the arrow keys!


### Some Useful Resources for Well-Known Robots:
        https://github.com/Daniella1/urdf_files_dataset/tree/main
        https://github.com/robot-descriptions/awesome-robot-descriptions
        https://github.com/robot-descriptions/robot_descriptions.py?tab=readme-ov-file#loaders
        https://github.com/Gepetto/example-robot-data/tree/master/robots/tiago_description
        https://github.com/stephane-caron/awesome-open-source-robots
        https://github.com/ami-iit/awesome-urdf
