import os
import shutil

def copy_files_to_meshes(src_dir, dest_dir):
    """
    Copy all files from src_dir (and its subdirectories) to dest_dir, 
    flattening the directory structure (i.e., no subdirectories in dest_dir).
    
    Parameters:
    - src_dir: Source directory to copy files from
    - dest_dir: Destination directory to copy files to
    """
    if not os.path.exists(dest_dir):
        os.makedirs(dest_dir)

    for root, dirs, files in os.walk(src_dir):
        for file in files:
            src_file = os.path.join(root, file)
            dest_file = os.path.join(dest_dir, file)
            # Check if the file already exists in the destination directory
            if not os.path.exists(dest_file):
                shutil.copy2(src_file, dest_file)
            else:
                # Handle file name conflicts by appending a number
                base, ext = os.path.splitext(file)
                counter = 1
                while os.path.exists(dest_file):
                    dest_file = os.path.join(dest_dir, f"{base}_{counter}{ext}")
                    counter += 1
                shutil.copy2(src_file, dest_file)
                print(f"File {file} already exists. Renamed to {base}_{counter-1}{ext}")

if __name__ == "__main__":
    # Change these paths as needed
    source_directory = "meshes_OLD"
    destination_directory = "meshes"
    
    copy_files_to_meshes(source_directory, destination_directory)
    print(f"All files from {source_directory} have been copied to {destination_directory}")
