using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;
using SimpleJSON; // Import SimpleJSON namespace

public class MoveObjects : MonoBehaviour
{
    public HttpListener listener;
    public Thread listenerThread;
    public ConcurrentQueue<Dictionary<string, float[]>> dataQueue = new ConcurrentQueue<Dictionary<string, float[]>>();
    public int port = 8000; // Default port, can be overridden
    private Dictionary<string, GameObject> controlledObjects = new Dictionary<string, GameObject>();
    private SynchronizationContext syncContext;

    void Start()
    {
        syncContext = SynchronizationContext.Current; // To marshal the tasks to main thread 
        // Set up HTTP listener
        if (listener == null)
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/coordinates/");
            listener.Start();
            listenerThread = new Thread(ListenForConnections);
            listenerThread.Start();
            Debug.Log($"Listening for HTTP requests on port {port}...");
        }
    }

    void ListenForConnections()
    {
        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;

            if (request.HttpMethod == "POST")
            {
                // Read the incoming JSON data
                using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                {
                    string jsonData = reader.ReadToEnd();

                    // Parse the JSON data
                    Dictionary<string, float[]> parsedData = ParseJsonData(jsonData); // jsonData = {'gameobject': gameobjectTransforms.tolist(), '':[], '':[], ....}

                    if (parsedData != null)
                    {
                        // Use SynchronizationContext to execute InitializeGameObjects on the main thread
                        syncContext.Post(_ =>
                        {
                            InitializeGameObjects(parsedData);
                            dataQueue.Enqueue(parsedData);
                        }, null);
                    }
                    else
                    {
                        Debug.LogWarning("Failed to parse JSON data.");
                    }
                }

                // Send a response to the client
                HttpListenerResponse response = context.Response;
                response.StatusCode = 200;
                response.Close();
            }
        }
    }
    
    // Creates a dictionary of references to gameobjects mentioned in incoming data dict
    void InitializeGameObjects(Dictionary<string, float[]> objectDataDict)
    {
        foreach (var entry in objectDataDict)
        {
            string objectName = entry.Key;

            if (!controlledObjects.ContainsKey(objectName))
            {
                GameObject obj = GameObject.Find(objectName);
                if (obj != null)
                {
                    controlledObjects[objectName] = obj;
                }
                else
                {
                    Debug.LogWarning($"GameObject with the name '{objectName}' not found.");
                }
            }
        }
    }

    void LateUpdate()
    {
        if (dataQueue.TryDequeue(out Dictionary<string, float[]> objectDataDict))
        {
            // objectDataDict contains the string for name of Gameobj, while controlledObjects references to the actual object from the string
            foreach (var entry in objectDataDict)
            {
                if (controlledObjects.TryGetValue(entry.Key, out GameObject obj))
                {
                    MoveObject(obj, entry.Value); // Get the required Gameobj out from references dictionary and pass it to MoveObject
                }
                else
                {
                    //Debug.LogWarning($"GameObject '{entry.Key}' not found in the controlled objects.");
                }
            }
        }
    }

    // Transforms the object with given name to given configuration
    void MoveObject(GameObject obj, float[] transformData)
    {
        
        Vector3 position = new Vector3(-transformData[1], transformData[2], transformData[0]);
        Vector3 localRotation = new Vector3(transformData[4], -transformData[5], -transformData[3]);

            // x goes to z
            // Y goes to -x
            // z goes to Y

        obj.transform.localPosition = position;
        obj.transform.localEulerAngles = localRotation;
    }

    // Converts the incoming JSON data to dictionary usable by C# 
    Dictionary<string, float[]> ParseJsonData(string jsonData)
    {
        try
        {
            // Parse the incoming JSON string to a JSONNode
            JSONNode rootNode = JSON.Parse(jsonData);

            if (rootNode == null)
            {
                Debug.LogError("Parsed root node is null.");
                return null;
            }

            Dictionary<string, float[]> dataDict = new Dictionary<string, float[]>();

            foreach (var key in rootNode.Keys)
            {
                JSONArray valuesArray = rootNode[key].AsArray;

                if (valuesArray != null)
                {
                    float[] values = new float[valuesArray.Count];
                    for (int i = 0; i < valuesArray.Count; i++)
                    {
                        values[i] = valuesArray[i].AsFloat;
                    }

                    dataDict[key] = values;
                }
                else
                {
                    Debug.LogWarning($"Value for '{key}' is not a valid array.");
                }
            }

            return dataDict;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse JSON data: {e.Message}");
            return null;
        }
    }

    // Printing dictionary might be required for debugging purposes
    void PrintDictionary(Dictionary<string, float[]> dictionary)
    {
        foreach (var kvp in dictionary)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {string.Join(", ", kvp.Value)}");
        }
    }

    void OnDestroy()
    {
        if (listener != null)
        {
            listener.Stop();
            listenerThread.Abort();
        }
    }
}
