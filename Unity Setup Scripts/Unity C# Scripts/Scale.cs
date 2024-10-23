using UnityEngine;
[ExecuteInEditMode]

public class ScaleDivider : MonoBehaviour
{
    // Factor to divide the scale by
    public float scaleFactor = 3.5f;

    void Start()
    {
        // Get all GameObjects in the scene
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        // Loop through each GameObject and modify its scale
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.activeInHierarchy) // Only modify active objects
            {
                // Divide the local scale by the scaleFactor
                Vector3 originalScale = obj.transform.localScale;
                obj.transform.localScale = originalScale / scaleFactor;
            }
        }

        Debug.Log("All GameObjects' scales have been divided by " + scaleFactor);
    }
}
