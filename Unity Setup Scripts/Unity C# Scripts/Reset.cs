// A script to set the pose of entire heirarchy to ZERO.
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[ExecuteInEditMode]
public class Reset : MonoBehaviour
{
    void Start()
    {
        // Reset the position and rotation of the current GameObject (this object)
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity; // Resets rotation to (0, 0, 0)

        // Recursively reset the position and rotation of all child objects
        ResetChildren(transform);
    }

    // A recursive method to reset the position and rotation of all child GameObjects
    void ResetChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Reset the child's position and rotation
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity; // Resets rotation to (0, 0, 0)

            // Recursively call the function to handle all the children of this child
            if (child.childCount > 0)
            {
                ResetChildren(child);
            }
        }
    }
}
