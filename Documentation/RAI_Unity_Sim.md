# Running a Parallel RAI-UNITY Simulation
In such an environment, the movement of robot in unity is controlled by RAi. This RAi-Unity simulation environment can ve viewed in VR Headset through Quest Link/Air Link (Tested on Oculus Quest 2 (or Oculus Quest Series in general) for now).

- Open your Unity Project and load the scene. On RAI side, load .g files in your environment.
- Now, copy the C# scripts "Flatten.cs", "removeTrash.cs", "MoveObjects.cs" & "SimpleJSON.cs" to the assets folder in your current unity project.
- Drag and attach the removeTrash script to the root of imported URDF game object. This should remove all the scripts, physics components attached throughout its heirarchy.
- Now, attach "Flatten" script to the same root URDF Game Object.
- Make sure the IP address for communication to machine running RAi is set correctly in these scripts. Also make sure that all network firewalls are "turned off" in Windows.
- Attach the "MoveObjects.cs" script to the Main Camera Game Object. This will implement the Http listener for moving the objects according to received frame data from RAI.
- [Pick_Place jupyter notebook](https://github.com/HuzaifaSalahuddin/RAI-Unity-Bridge/blob/main/Example%20RAi%20Scripts/Pick_place.ipynb) implements communication module on RAI's side. Use this notebook to initiate a simulation that drives the robot in Unity controlled through RAI.

# Running a VR Simulation
- Create a new project in Unity and install the following packages from package manager:
        - URDF Importer (see "Unity_BotSimulation")
        - XR Plugin Management
        - XR Interaction Toolkit
- Go to "XR Interaction Toolkit" using Package Manager Window and import "Starter Assets" from under the "Samples" tab.
- Delete the "Main Camera" Game Object. For VR Headset, we are going to add XR Origin. Add GameObject >> XR >> "XR Origin (VR)" to the scene.
- Attach the "MoveObjects.cs" script to the XR Origin (XR Rig) Game Object.
- At this point, your Gameobjects' heirarchy should look something like the following. Make sure you have all these objects in your heirarchy:
  
        -- Light Source
  
        -- XR Interaction Manager
  
        -- XR Origin (XR Rig)
  
        -- YOUR-ROBOT
  
        -- SCENE-OBJECT-01
  
        -- SCENE-OBJECT-02
  
        -- -----------------------
  
        -- ---other scene objects---
  
- Now, go to XR Interaction Manager and add the "Input Action Manager" script to it by clicking on "Add Component". Next, expand the script section in inspector's tab and then add XRI Default Input Actions in Actions Asset.
- Go to Edit >> Project settings and enable Oculus from "XR Plugin Management".
- Make sure you have Meta Quest Link app installed on your Windows (It doesn't work on Linux/Ubuntu). Open the app. 
- The Meta Quest Link app should be logged in with the same account as your headset. Make sure to enable apps "from unknown sources" in the Quest Link app.
- Now, connect your Oculus through cable and enable Quest link in headset.
- Enter into unity play mode from unity.
- You should now be able to see your environment through headset. Use [Pick_Place jupyter notebook](https://github.com/HuzaifaSalahuddin/RAI-Unity-Bridge/blob/main/Example%20RAi%20Scripts/Pick_place.ipynb) with RAi from Ubuntu machine to view an example pick and place simulation through your headset.  


## RAI-Unity Axes Transformations   

The convention of defining XYZ axes is different in RAi and unity. RAi uses the standard convention (e.g z up) while the unity uses non-conventional definition (e.g y up). For the rotation, the RAi uses right-hand convention while the unity uses left-hand convention for rotation. The details for transformation are:


| RAI  | UNITY |
|------|-------|
| X    | Z    |
| Y    | -X     |
| Z    | Y    |

For transforms, we send relative transforms of parent-to-child frames, not the absolute transforms in global frame (as it causes serious problems due to different axes convention) from RAi to unity. Then, these rotations in X, Y and Z axes are synchronized with the local rotations in unity as per the following protocol:


| RAI  | UNITY |
|------|-------|
| X    | -Z    |
| Y    | X     |
| Z    | -Y    |


## Some Important Notes
- For objects other than robot in URDF, the RAi converter appends an "_0" to their names. For correct control/manipulation of the object using RAi's KOMO, you need to specify the frame's name along with an "_0" at the end.
- If you are using mobile robot, then the urdf2rai converter does not recognize the "planar" and various other kinds of joints. You need to modify the file to incorporate those. An example to incorporate "planar joints" -> Add the following line somewhere around line 180:
  ```python
        if att == 'planar':
                print('joint: transXYPhi', end='')

- If you are using "planar joint", then in case of problems with using KinEdit on newly URDF-to-.g converted file, follow this: Find a line similar to the following under planar joint section of your URDF before using URDF2RAi converter and eliminate it. RAi converter probably needs some further modifications before it can incorporate this:
  ```urdf
    <limit effort="30" velocity="1.0" lower="-2.2" upper="0.7" />
