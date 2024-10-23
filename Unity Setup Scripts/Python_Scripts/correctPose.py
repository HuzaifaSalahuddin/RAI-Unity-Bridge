import json
import xml.etree.ElementTree as ET
import math

# Load the transform data from a JSON file
with open("transform_data.json", "r") as f:
    transform_data = json.load(f)

# Parse the URDF file
urdf_file = "Kitchen.urdf"
tree = ET.parse(urdf_file)
root = tree.getroot()

# Update the URDF with new origin positions and rotations
for link in root.findall('link'):
    visual = link.find('visual')
    if visual is not None:
        visual_name = link.get('name')

        # Find the corresponding data in the transform_data
        for obj_data in transform_data['data']:
            if obj_data['objectName'] == visual_name:
                # Get position and rotation from JSON data
                pos = obj_data['position']
                rpy = obj_data['rotationEuler']  # Assuming rotation is provided as Euler angles (in degrees)

                # Convert Euler angles from degrees to radians
                rpy_rad = {
                    'x': math.radians(rpy['x']),
                    'y': math.radians(rpy['y']),
                    'z': math.radians(rpy['z'])
                }

                # Apply the position transformations
                transformed_pos = {
                    'x': pos['z'],  # X -> -Y
                    'y': -pos['x'],  # Y -> Z
                    'z': pos['y']  # Z -> X
                }

                # Apply the Euler angle transformations (now in radians)
                transformed_rpy = (
                    -rpy_rad['z'],  # X (Roll) -> Y (Pitch)
                    rpy_rad['x'],   # Y (Pitch) -> -Z (Yaw)
                    -rpy_rad['y']   # Z (Yaw) -> -X (Roll)
                )

                # Find and modify the origin tag
                origin = visual.find('origin')
                if origin is not None:
                    # Update the xyz and rpy attributes
                    origin.set('xyz', f"{transformed_pos['x']} {transformed_pos['y']} {transformed_pos['z']}")
                    origin.set('rpy', f"{transformed_rpy[0]} {transformed_rpy[1]} {transformed_rpy[2]}")
                break

# Save the modified URDF to a new file
output_urdf_file = "Exp.urdf"
tree.write(output_urdf_file)

print(f"Updated URDF saved to {output_urdf_file}")

