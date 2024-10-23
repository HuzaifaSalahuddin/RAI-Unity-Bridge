using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class RemoveTrash : MonoBehaviour
{
    void Start()
    {
        // Call the cleanup method when the script starts
        CleanupAllComponents(transform);
    }

    void CleanupAllComponents(Transform root)
    {
        // Loop through all children and the root itself
        foreach (Transform child in root)
        {
            // Recursively clean up child objects
            CleanupAllComponents(child);
        }

        // Get all components attached to this GameObject
        var components = root.GetComponents<Component>();

        // Loop through each component to remove scripts and articulation body
        foreach (var component in components)
        {
            // Remove any script components
            if (component is MonoBehaviour)
            {
                DestroyImmediate(component, true);
                continue;
            }

        }

        // Loop through again for physics-related components
        foreach (var component in components)
        {
            // Remove any collision or physics-related components
            if (component is Collider || component is Rigidbody || component is Joint) //|| component is PhysicsMaterial2D)
            {
                DestroyImmediate(component, true);
                continue;
            }

            // Remove ArticulationBody components
            if (component is ArticulationBody)
            {
                DestroyImmediate(component, true);
                continue;
            }
            
        }
    }
}
