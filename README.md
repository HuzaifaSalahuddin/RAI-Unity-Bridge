# RAI-Unity-Bridge

## System Specs:

### For Unity:
Windows 11
64-bit operating system, x64-based processor
Meta Quest Link App
Unity Editor: 2022.3.41f1

### For RAI:
Ubuntu 22.04.4 LTS
6.5.0-45-generic Kernel
NVIDIA Driver Version: 560.28.03
CUDA Version: 12.6


(1) Create a new project in Unity and install the following packages from package manager:
        -- URDF Importer (see "Unity_BotSimulation")
        -- XR Plugin Management
        -- XR Interaction Toolkit
(2) Import the scene from URDF and .g files in unity and RAi respectively.
(3) Delete the "Main Camera" Game Object. For VR Headset, we are going to add XR Origin. Add GameObject >> XR >> "XR Origin (VR)" to the scene.
(4) Now, copy the C# scripts "Flatten.cs", "removeTrash.cs", "MoveObjects.cs" & "SimpleJSON.cs" to the assets folder in your current unity project.
(5) Drag and attach the removeTrash script to the root of imported URDF game object. This should remove all the scripts, physics components attached throughout its heirarchy.
(6) Now, attach "Flatten" script to the same root URDF Game Object.
(7) Make sure the IP address for communication to machine running RAi is set correctly in these scripts. Also make sure that all network firewalls are "turned off" in Windows.
(8) Attach the "MoveObjects.cs" script to the XR Origin (XR Rig) Game Object.
(9) Go to "XR Interaction Toolkit" using Package Manager Window and import "Starter Assets" from under the "Samples" tab.
(10) At this point, your Gameobjects' heirarchy should look something like the following. Make sure you have all these objects in your heirarchy:
        -- Light Source
        -- XR Interaction Manager
        -- XR Origin (XR Rig)
        -- YOUR-ROBOT
        -- SCENE-OBJECT-01
        -- SCENE-OBJECT-02
        -------------------------
        ---other scene objects---
(11) Now, go to XR Interaction Manager and add the "Input Action Manager" script to it by clicking on "Add Component". Next, expand the script section in inspector's tab and then add XRI Default Input Actions in Actions Asset.
(12) Go to Edit >> Project settings and enable Oculus from "XR Plugin Management".
(13) Make sure you have Meta Quest Link app installed on your Windows (It doesn't work on Linux/Ubuntu). Open the app. 
(14) The Meta Quest Link app should be logged in with the same account as your headset. Make sure to enable apps "from unknown sources" in the Quest Link app.
(15) Now, connect your Oculus through cable and enable Quest link in headset.
(16) Enter into unity play mode from unity.
(17) You should now be able to see your environment through headset. Use Pick_Place jupyter notebook with RAi from Ubuntu machine to view an example pick and place simulation by franka's "Panda arm" through your headset.  


!!_Some_Important_Notes_!!  

! The convention of defining XYZ axes is different in RAi and unity. RAi uses the standard convention (e.g z up) while the unity uses non-conventional definition (e.g y up). For the rotation, the RAi uses right-hand convention while the unity uses left-hand convention for rotation. The details for transformation are:
RAI  |  UNITY
X    |  Z   
Y    |  -X
Z    |  Y

! For rotation we send relative rotation of parent-to-child frames, not the absolute rotation in global frame (as it causes serious problems due to different axes convention) from RAi to unity. Then, these rotations in X, Y and Z axes are synchronized with the local rotations in unity as per the following protocol:
RAI  |  UNITY
X    |  -Z   
Y    |  X
Z    |  -Y

! For objects other than robot in URDF, the RAi converter appends an "_0" to their names. For correct control/manipulation of the object using RAi's KOMO, you need to specify the frame's name along with an "_0" at the end.
! The URDF2RAI converter does not recognize the "planar" and various other kinds of joints. You need to modify the file to incorporate those. An example to incorporate "planar joints":
        # Add the following line somewhere around line 180
        if att == 'planar':
                print('joint: transXYPhi', end='')

! If you are using "planar joint", then in case of problems with using KinEdit on newly URDF-to-.g converted file, follow this: Find a line similar to the following under planar joint section of your URDF before using URDF2RAi converter and eliminate it. RAi converter probably needs some further modifications before it can incorporate this:
   <limit effort="30" velocity="1.0" lower="-2.2" upper="0.7" />
