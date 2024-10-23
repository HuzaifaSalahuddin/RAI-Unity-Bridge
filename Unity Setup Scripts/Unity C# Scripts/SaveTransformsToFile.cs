using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class TransformData
{
    public string objectName;
    public Vector3 position;
    public Vector3 rotationEuler;
}

[System.Serializable]
public class TransformDataList
{
    public List<TransformData> data;

    public TransformDataList()
    {
        data = new List<TransformData>();
    }
}

public class SaveTransformsToFile : MonoBehaviour
{
    public string fileName = "transform_data.json";

    void Start()
    {
        // Create an object to store the transform data list
        TransformDataList transformDataList = new TransformDataList();

        // Get the root GameObjects in the scene
        foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            // Recursively get all children and store their transform data
            SaveObjectTransform(rootObj, transformDataList);
        }

        // Convert the list to JSON format
        string jsonData = JsonUtility.ToJson(transformDataList, true);

        // Save the JSON to a file
        string filePath = Path.Combine(Application.dataPath, fileName);
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Transform data saved to " + filePath);
    }

    // Recursive function to save transforms of a GameObject and its children
    void SaveObjectTransform(GameObject obj, TransformDataList transformDataList)
    {
        // Save this object's absolute (world) transform
        TransformData data = new TransformData
        {
            objectName = obj.name,
            position = obj.transform.position,  // Absolute position in world space
            rotationEuler = obj.transform.rotation.eulerAngles  // Euler angles in world space
        };
        transformDataList.data.Add(data);

        // Recursively save the children
        foreach (Transform child in obj.transform)
        {
            SaveObjectTransform(child.gameObject, transformDataList);
        }
    }

}
