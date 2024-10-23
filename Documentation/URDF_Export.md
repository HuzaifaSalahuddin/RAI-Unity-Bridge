# Instructions On Creating RAI-Importable URDF-Scenes in Unity

## Basic Scene Annotation
- Follow the basic urdf import steps from "Unity_BotSimulations" to import a robot from URDF file. 

**!!IMPORTANT!!**
"DO NOT ENTER UNITY PLAY MODE" if you are in a process of creating a URDF scene. It damages the export process. It is highly suggested to first create and export the URDF scene. You may run the simulation after you are satisfied with the scene creation and have it exported to URDF.

- To reduce complexity and help in debugging, we shall be creating the URDF for the "scene only", without a robot (You may import a robot separately again through its URDF). So, delete all the children of world. Create the Scene howsoever you like. Make sure all the objects are in the correct locations. Keep moving them until you are satisfied. At this point, you need to make sure that these objects DO NOT HAVE any more children (If they have, then take them out so that their transforms are preserved)

- Now copy all the objects and make them the children of an empty gameobject, say its name is "Objects". The transform (pos + rot) of this "Objects" should be completely ZERO. 

- Now, attach the script CopyChildNames and paste it to "Objects" and put the name "Objects" in the source Parent public field. This should copy all these objects with required URDF transformations and paste the result under the world Game Object which must be present somewhere in heirarchy of robot object you imported using URDF importer.

- So, now each of your object under "world" should have a heirarchy which looks something like this:
        --YOUR_object
          |--visuals
          |--collisions

- Go to the root of your URDF robot object and rename it as per your scene.

- Now, you are done and can export the object in URDF format by clicking on "Export robot to URDF" under "URDF Robot" script attached to root of your URDF scene object.

- Now, export the Robot to URDF (In case of Tiago/Mobile Robots, go and remove the limit line for planar joints in the exported URDF -> Known to Cause Issues)

## Save Correct Transform Data for Correction

- Now, go back to your Unity Editor and delete the entire URDF object. So, now you have scene with simple object meshes.

- Attach the script SaveTransformstoJSON something like that to the "Objects" which contains the copy of all the objects in the scene as its children. Enter the file name in which you would like to save the transform data of all objects in your scene.

- Enter into Unity Play Mode and you should see the message saying that the transforms data has been saved to transform_data.JSON file. Exit the Play Mode.

- At this point, you should have STL files for all your gameobjects in the scene. Otherwise, things won't work out properly.

## Preparation of STL Files

- If you do not have the STL files for your objects, then you will need to use Pro-builder tools to create the STL files.

- Again, consider the "Objects" and set the transform pos + rot of all its children equal to zero (You can attach script Reset.cs to it for this purpose for doing it in ONE GO! -- Remove it after your work is done) -> For preparation to export.

- With all these objects selected, go to Tools > Probuilder > Probuilder Window and select "Probuilderize".

- Then export all these objects from Tools > Probuilder > Export > Export STL ASCII. all of them should be exported in a new folder called meshes (Create it in a different directory than robot meshes folder)

- Now, you can delete the "Objects" gameobject and any remaining clutter that you might have produced while doing URDF Export.

## Applying Transforms Correction to URDF File

- Go to the folder where you have exported the URDF and paste the xxx.JSON file over there that you created earlier. Copy the script "correctPose.py" in the same directory. Open the script and change the name of "output_urdf_file" as per your exported URDF filename.

- Run the script and it should produce the corrected version of URDF file.

- Now, you need to convert this URDF file to .g format in order to make it RAi-importable. Make sure you have built kinEdit and added its path to environment, available from bin directory of Marc's RAi bare codebase. Copy the exported URDF file to the folder containing urdf2rai.py.

(17) Now, copy this URDF file along with the meshes folder (in form of STL files that you generated earlier) and paste it in /path/to/urdf2rai.py etc.. for URDF2RAI conversion. 

(10) Open the terminal from current directory and run the following two commands:
        > python3 urdf2rai.py <your_file>.urdf > z.1.g
        > kinEdit -file z.1.g -cleanOnly

(11) This will allow you to view your object. Press "Enter" to animate your Robot (Well you don't have it right now LOL). Press "x" to export the file in .g format. Then, press q to exit the configuration viewer.

(12) Run the command: mv z.g <your_desired_filename>.g

(13) Copy both the URDF and created file to RAI-ROBOTMODELS path or your Python Notebook directory and you should now be able to import these objects as RAI objects.

(14) Now, you have created your scene. It is time to import the robot URDFs and .g files. Do it separately in both RAi and Unity. 

(15) Also, create a new .URDF file in which you combine both scene and robot urdfs. This is just for RAi to get the parent-child relationship amongst various frames in the scene and nothing else. You may name it as "scene.urdf". 

There are three main tasks that you need to complete for properly exporting your scene in URDF format:
(1)
(2)
(3)
