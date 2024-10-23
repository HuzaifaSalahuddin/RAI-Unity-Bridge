import xml.etree.ElementTree as ET

def modify_mesh_filenames(urdf_file_path):
    # Parse the URDF file
    tree = ET.parse(urdf_file_path)
    root = tree.getroot()

    # Iterate over all <mesh> elements in the URDF file
    for mesh in root.findall(".//mesh"):
        filename = mesh.get("filename")
        if filename:
            # Find the substring 'meshes/' and keep everything after it
            index = filename.find("meshes/")
            if index != -1:
                new_filename = filename[index:]  # Keep only the part after 'meshes/'
                mesh.set("filename", new_filename)
                print(f"Updated filename: {filename} -> {new_filename}")

    # Save the modified URDF file
    tree.write(urdf_file_path)
    print(f"Modified URDF file saved: {urdf_file_path}")

# Provide the path to your URDF file here
urdf_file_path = "tiago_no_hand.urdf"
modify_mesh_filenames(urdf_file_path)
