using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flatten : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject robot = this.gameObject;
        RenameAndReparent(robot);
    }

    // Recursive function to rename and reparent the entire hierarchy
    void RenameAndReparent(GameObject obj)
    {
        // Traverse through all children recursively
        foreach (Transform link in obj.GetComponentsInChildren<Transform>(true))
        {
            // Step 1: Reparent and rename Visuals/unnamed and Collisions if they exist
            Transform visuals = link.Find("Visuals/unnamed");
            if (visuals == null)
            {
                visuals = link.Find("Visuals/Visuals");
            }
            if (visuals == null)
            {
                visuals = link.Find("Visuals");
            }
            Transform collision = link.Find("Collisions");

            if (visuals != null)
            {
                // Rename the "unnamed" child under Visuals to the current link's name
                visuals.name = link.name;

                // Reparent the "unnamed" (now renamed) to the parent of the link
                visuals.SetParent(link.parent); // Move visuals to replace the original link in the hierarchy

                // Move all other children of the link to the new parent (visuals)
                List<Transform> childrenToKeep = new List<Transform>();

                foreach (Transform child in link)
                {
                    if (child != visuals && child != collision)
                    {
                        childrenToKeep.Add(child); // Add the other children to the list
                    }
                }

                // Reparent the other children to the renamed visuals
                foreach (Transform child in childrenToKeep)
                {
                    child.SetParent(visuals);
                }

                // Destroy the original Collision and Visuals objects
                if (collision != null)
                {
                    Destroy(collision.parent.gameObject); // Destroy the entire Collision GameObject
                }

                Destroy(link.gameObject); // Destroy the original link GameObject after reparenting

                // Recursively process the children of the renamed visuals
                RenameAndReparent(visuals.gameObject);
            }
        }

        // Step 2: Rename and replace a parent with its child if they share the same name
        foreach (Transform link in obj.GetComponentsInChildren<Transform>(true))
        {
            // Look for cases where the child has the same name as the parent
            if (link.childCount > 0)
            {
                Transform firstChild = link.GetChild(0); // The first child
                if (firstChild.name == link.name)
                {
                    // Move the firstChild up to the same level as link
                    firstChild.SetParent(link.parent); // Reparent the firstChild to link's parent

                    // Now destroy the original link GameObject
                    Destroy(link.gameObject);
                }
            }
        }

    }
}

