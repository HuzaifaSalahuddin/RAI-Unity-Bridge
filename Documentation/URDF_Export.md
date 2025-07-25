# Instructions On Creating RAI-Importable URDF-Scenes in Unity

## Basic Scene Annotation
- Follow the basic urdf import steps from "URDF_Import" to import a robot from URDF file. 

**IMPORTANT!**

*"DO NOT ENTER UNITY PLAY MODE" if you are in a process of creating a URDF scene. It damages the export process. It is highly suggested to first create and export the URDF scene. You may run the simulation after you are satisfied with the scene creation and have it exported to URDF.*

- Create the Scene howsoever you like. Make sure all the objects are in the correct locations. Keep moving them until you are satisfied. At this point, you need to make sure that these objects DO NOT HAVE any more children (If they have, then take them out so that their transforms are preserved). Also, there should not be any spaces within the names of gameobjects. If a gameobject is named as "X Y", rename it as "X_Y".

- Now copy all the objects (avoid copying light sources) and make them the children of an empty gameobject, put its name as "Objects". The transform (pos + rot) of this "Objects" should be completely ZERO. 

- Now, attach the script CopyChildNames and paste it to "Objects" and put the name "Objects" in the source Parent public field. This should copy all these objects with required URDF transformations and paste the result under the world Game Object which must be present somewhere in heirarchy of robot object you imported using URDF importer. Immediately remove the script after that to avoid getting multiple copies of objects. Do not delete the "Objects" gameobject. You will need this later on!
- So, now each of your object under "world" should have a heirarchy which looks something like this:
        --YOUR_object
          |--visuals
          |--collisions

- Go to the root of your URDF robot object and rename it as per your scene.

- Now, you are done and can export the object in URDF format by clicking on "Export robot to URDF" under "URDF Robot" script attached to root of your URDF scene object.

- Now, export the Robot to URDF (In case of Tiago/Mobile Robots, go and remove the limit line for planar joints in the exported URDF -> Known to Cause Issues)

*After the first export, if you feel like exporting once more to pass-on some adjustments/updates that you made in your URDF object, you cannot do that with the same URDF object you just exported because first export fully messes up the overall configuration of URDF object. So, you need to start from the beginning all over again. Therefore, make sure to export the URDF scene only once you are fully satisfied with all the adjustments.*

## Apply Correction to Transform Data

- Now, go back to your Unity Editor and delete the entire URDF object. So, now you have scene with simple object meshes.

- Attach the script SaveTransformstoJSON to the "Objects" which contains the copy of all the objects in the scene as its children. Enter the file name in which you would like to save the transform data of all objects in your scene.

- Enter into Unity Play Mode and you should see the message saying that the transforms data has been saved to transform_data.JSON file. Exit the Play Mode.

- Go to the folder where you have exported the URDF and paste the xxx.JSON file over there that you created earlier. Copy the script "correctPose.py" in the same directory. Open the script and change the name of "output_urdf_file" as per your exported URDF filename.

- Run the script and it should produce the corrected version of URDF file.

- At this point, you should have STL files for all your gameobjects in the scene. Other file formats seem to create weird problems.

## Preparation of STL Files

- If you do not have the STL files for your objects, then you will need to use Pro-builder tools to create the STL files. Install the Probuilder Tools from Package Manager before continuing.

- Again, consider the "Objects" and set the transform pos + rot of all its children equal to zero (You can attach script Reset.cs to it for this purpose for doing it in ONE GO! -- Remove it after your work is done) -> For preparation to export.

- With all these objects selected, go to Tools > Probuilder > Probuilder Window and select "Probuilderize".

- Then export all these objects from Tools > Probuilder > Export > Export STL ASCII. all of them should be exported in a new folder called meshes (Create it in a different directory than robot meshes folder)

- Now, you can delete the "Objects" gameobject and any remaining clutter that you might have produced while doing URDF Export.

## Preparation of .g File for RAi
- Now, you need to convert this URDF file to .g format in order to make it RAi-importable. Make sure you have built kinEdit and added its path to environment, available from bin directory of Marc's RAi bare codebase. Copy the exported URDF file to the folder containing urdf2rai.py.

- Now, copy this URDF file along with the meshes folder (in form of STL files that you generated earlier) and paste it in /path/to/urdf2rai.py etc.. for URDF2RAI conversion. 

- Make sure you have already built [kinEdit](https://github.com/MarcToussaint/rai-maintenance/blob/master/help/kinEdit.md). Follow the steps similar to what is described [here](https://github.com/MarcToussaint/rai-robotModels/blob/master/panda/HOWTO.sh) for conversion from URDF to .g file.

- Copy both the URDF and created file to RAI-ROBOTMODELS path or your Python Notebook directory and you should now be able to import these objects as RAI objects.

## Finalize things!
So now, you are done with the creation of .g and .urdf files for your scene with a robot. Import them inside RAi using the [Pick_Place](../Example%20RAi%20Scripts/Pick_place.ipynb) notebook and verify if everything works as intended. Then, you can go back to Unity and delete the extra copied objects other than robot links in your URDF gameobject. Also, you may copy the all the children gameobjects of "Objects" out of this gameobject. At this point, it is safe to delete "Objects". So, now you should have a robot which is a URDF gameobject and other objects which are simple gameobjects without any URDF compoenents attached to them. You may save the scene now and then proceed to [RAI_Unity_Sim](RAI_Unity_Sim.md) if you want a running simulation connecting both RAI and Unity.

