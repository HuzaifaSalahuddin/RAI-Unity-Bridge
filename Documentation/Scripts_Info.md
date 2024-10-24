## RAi-Unity Scripts ##

__Flatten.cs__ 

Converts the heirarchy structure that looks something like this 

- basket_01
  - Collisions
    - unnamed
      - Box
  - Visuals
    - Visuals
      - basket_01
        - basket_01_0

OR

- basket_01
  - Collisions
    - unnamed
      - Box
  - Visuals
    - unnamed
      - basket_01
        - basket_01_0

to something like the following:

- basket_01
  - basket_01_0

 So, it essentially flattens the structure --> Look for update!

__removeTrash.cs__ 

It removes all the scripts/extra physics components attached to your URDF gameobject to make it simple.

__CopyChildNames.cs__ 

This one copies all the children of a "source parent" to "target parent" and convert each of the children (say 'bowl') into something that looks like this:
bowl
 |-Visuals
  |--bowl
 |-Collisions
(Basically used for making outside objects the children of robot's world so that they are ready for export to URDF format)  --> Look for updates (Maybe changing the name of grandchildren?)

__MoveObjects.cs__ 

The main script responsible for getting the data through IP for movement in the world.

__Reset.cs__ 

Attach this script to a gameobject and it will reset the transforms of all its children to zero.

__Scale.cs__

Scales down all the gameobjects in the scene by a specified scaleFactor.

__SaveTransformsToFile.cs__

Saves the current transforms of all gameobjects in the scene to a JSON file.

__SimpleJSON.cs__

Provides useful JSON functionalities.

__URDFlatten.py__ 

This script flattens the directory structure mentioned for meshes in the URDF files (Supports stl and dae files)

__Delete.py__ 

This script deletes all the clutter files produced by unity in form of .prefab, .meta and .asset in the specified directory.

__clean.py__

Something that converts like /path/to/meshes/blah/blah to meshes/blah/blah (Deletes everything before mesh)

__configure.py__

Flattens the directory structure of the specified folder

__Pick_place.ipynb__

This contains the communication module implemented on RAI's side. It sends the relative frame data through network towards Unity. Http listener on Unity Side implemented in __MoveObjects.cs__  receives this data and moves the objects accordingly.
