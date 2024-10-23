import os
import fnmatch

def remove_files_with_extensions(folder_path, extensions):
    # Iterate through all files in the folder
    for root, dirs, files in os.walk(folder_path):
        for file in files:
            # Check if the file matches any of the specified extensions
            if any(fnmatch.fnmatch(file, f'*{ext}') for ext in extensions):
                file_path = os.path.join(root, file)
                try:
                    os.remove(file_path)
                    print(f"Deleted: {file_path}")
                except OSError as e:
                    print(f"Error deleting {file_path}: {e}")

# List of extensions to delete
extensions_to_delete = ['.prefab', '.asset', '.meta', '*.meta']

# Specify the folder path
folder_path = 'meshes'

# Call the function
remove_files_with_extensions(folder_path, extensions_to_delete)

