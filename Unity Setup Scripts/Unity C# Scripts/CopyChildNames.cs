using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Robotics.UrdfImporter;

namespace Unity.Robotics.UrdfImporter
{
    [ExecuteInEditMode]
    public class CopyChildNames : MonoBehaviour
    {
        // Input fields for source and target GameObject names
        public string sourceParentName = "Objects"; // The name of the GameObject whose children's names will be copied
        public string targetParentName = "world"; // The name of the GameObject under which new children will be created

        // This method will be called automatically when the script is loaded or when values change in the Inspector
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.delayCall += CreateChildObjects; // Use delayCall to defer execution
            }
        }

        // Function to create child objects and attach scripts
        public void CreateChildObjects()
        {
            // Find the source parent GameObject by searching the entire scene hierarchy
            GameObject sourceParent = FindGameObjectByName(sourceParentName);
            if (sourceParent == null)
            {
                Debug.LogError($"Source GameObject with name '{sourceParentName}' not found!");
                return;
            }

            // Find the target parent GameObject by searching the entire scene hierarchy
            GameObject targetParent = FindGameObjectByName(targetParentName);
            if (targetParent == null)
            {
                Debug.LogError($"Target GameObject with name '{targetParentName}' not found!");
                return;
            }

            // Get all child GameObjects of the source parent
            List<string> childNames = new List<string>();
            foreach (Transform child in sourceParent.transform)
            {
                childNames.Add(child.name);
            }

            // Create new empty GameObjects with the same names as children of the target parent
            foreach (string childName in childNames)
            {
                GameObject newChild = new GameObject(childName); // Create a new empty GameObject with the same name
                newChild.transform.SetParent(targetParent.transform); // Set it as a child of the target parent

                // Defer the attachment of scripts to avoid issues with Unity lifecycle methods
                EditorApplication.delayCall += () =>
                {
                    newChild.AddComponent<UrdfLink>();
                    UrdfVisuals urdfVisualsComponent = newChild.AddComponent<UrdfVisuals>();
                    newChild.AddComponent<UrdfCollisions>();

                    // Create two additional empty children named "Visuals" and "Collisions"
                    GameObject visualsChild = new GameObject("Visuals");
                    visualsChild.transform.SetParent(newChild.transform); // Set "Visuals" as a child of the new GameObject

                    GameObject collisionsChild = new GameObject("Collisions");
                    collisionsChild.transform.SetParent(newChild.transform); // Set "Collisions" as a child of the new GameObject

                    // Attach the respective scripts to the "Visuals" and "Collisions" children
                    UrdfVisual urdfVisualComponent = visualsChild.AddComponent<UrdfVisual>();
                    collisionsChild.AddComponent<UrdfCollision>();

                    // Set the UrdfVisuals type to "Mesh" programmatically if the component allows it
                    if (urdfVisualComponent != null)
                    {
                        urdfVisualComponent.geometryType = Unity.Robotics.UrdfImporter.GeometryTypes.Mesh; // Make sure to specify the full namespace
                    }

                    // Find the corresponding child GameObject from the source parent
                    Transform sourceChild = sourceParent.transform.Find(childName);
                    if (sourceChild != null)
                    {
                        // Copy the global transform values from the source child to the "Visuals" GameObject
                        visualsChild.transform.position = sourceChild.position;
                        visualsChild.transform.rotation = sourceChild.localRotation;
                        visualsChild.transform.localScale = sourceChild.lossyScale;

                        // Instantiate the corresponding child GameObject
                        GameObject copiedChild = Instantiate(sourceChild.gameObject);
                        copiedChild.transform.SetParent(visualsChild.transform); // Set it as a child of the "Visuals" GameObject
                        copiedChild.name = sourceChild.name; // Maintain the original name

                        // Reset the transform values of the copied child to default (0, 0, 0)
                        copiedChild.transform.localPosition = Vector3.zero;
                        copiedChild.transform.localRotation = Quaternion.identity;
                        copiedChild.transform.localScale = Vector3.one;
                    }
                    else
                    {
                        Debug.LogWarning($"Child GameObject with name '{childName}' not found in source parent!");
                    }
                };
            }

            Debug.Log($"Successfully created {childNames.Count} new GameObjects under '{targetParentName}', attached scripts, and added 'Visuals' and 'Collisions' children with their respective components");
        }

        // Helper method to find a GameObject by name in the entire scene hierarchy
        private GameObject FindGameObjectByName(string name)
        {
            Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>(); // Get all transforms in the scene
            foreach (Transform transform in allTransforms)
            {
                if (transform.name == name)
                {
                    return transform.gameObject;
                }
            }
            return null; // Return null if not found
        }
    }
}
