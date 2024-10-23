import re

def update_urdf_file(urdf_file_path, output_file_path=None):
    # Read the URDF file
    with open(urdf_file_path, 'r') as file:
        urdf_content = file.read()

    # Step 1: Replace mesh filenames to keep the directory structure intact
    updated_content = re.sub(r'(<mesh\s+filename="meshes/)[^"]+(/[^"]+\.(stl|dae)")', r'\1\2', urdf_content)

    # Step 2: Remove any double slashes introduced
    updated_content = re.sub(r'(<mesh\s+filename="meshes/)/*([^"]+)', r'\1\2', updated_content)

    # Save the updated URDF to a new file, or overwrite if no output path is given
    if output_file_path is None:
        output_file_path = urdf_file_path  # Overwrite original file

    with open(output_file_path, 'w') as file:
        file.write(updated_content)

    print(f"Updated URDF saved to: {output_file_path}")

# Usage example:
urdf_file = 'tiagoIQ.urdf'
update_urdf_file(urdf_file)

