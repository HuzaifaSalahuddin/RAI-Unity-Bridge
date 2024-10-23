using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using UnityEngine;

// This class is for articulating the robots. For moving objects other than robots, please take a look at MoveObjects script.
public class Articulate : MonoBehaviour
{
    public HttpListener listener;
    public Thread listenerThread;
    public ConcurrentQueue<JointAngles> dataQueue = new ConcurrentQueue<JointAngles>(); // This queue is for storing the incoming data and then one by one, dequeueing it to transform the robot joints.
    public ArticulationBody[] articulations; // To get the articulation bodies attached to all separate links of robot
    public int port = 5000; // Default port, can be overridden
    public void Initialize(int port)
    {
        this.port = port;
        Start(); // Start method is called manually to use the provided port
    }

    void Start()
    {
        articulations = GetComponentsInChildren<ArticulationBody>();
        
        if(listener == null)
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/coordinates/");
            listener.Start();
            listenerThread = new Thread(ListenForConnections);
            listenerThread.Start();
            Debug.Log($"Listening for HTTP requests on port {port}...");
        }
        
    }

    void Update()
    {
        if (dataQueue.TryDequeue(out JointAngles jointAngles))
        {
            articulate(articulations, jointAngles);
        }
    }

    public void ListenForConnections()
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
                    JointAngles jointAngles = JsonUtility.FromJson<JointAngles>(jsonData);

                    // Log or use the received coordinates
                    if (jointAngles != null && jointAngles.joint_state != null)
                    {
                        //Debug.Log($"Received coordinates: {string.Join(", ", jointAngles.joint_state)}");
                        dataQueue.Enqueue(jointAngles);
                    }
                    else
                    {
                        Debug.LogWarning("Received invalid data.");
                    }
                }

                // Respond to the client
                HttpListenerResponse response = context.Response;
                response.StatusCode = 200;
                response.Close();
            }
        }
    }

    public void OnDestroy()
    {
        listener.Stop();
        listenerThread.Abort();
    }

    [Serializable]
    public class JointAngles
    {
        public float[] joint_state;  // Adjust the type based on the actual data
    }

    public void articulate(ArticulationBody[] articulations, JointAngles jointAngles)
    {

        if (articulations.Length > 0)
        {
            // Create a list to hold all joint positions in the correct order
            List<float> jointPositions = new List<float>();

            // Create a list to hold the DOF start indices
            List<int> dofStartIndices = new List<int>();
            
            // Retrieve the DOF start indices for the entire hierarchy
            articulations[0].GetDofStartIndices(dofStartIndices);

            for (int i = 0; i < articulations.Length; i++)
            {
                ArticulationBody articulation = articulations[i];
                int dofStartIndex = dofStartIndices[articulation.index];
                Debug.Log("Name: " + articulation.name);
                Debug.Log("JointType: " + articulation.jointType);

                Debug.Log("DOF Count: " + articulation.dofCount);
                // Add the appropriate joint state values to the list
                for (int j = 0; j < articulation.dofCount; j++)
                {
                    // Convert the list to a readable string format and log it
                    string jointPositionsString = string.Join(", ", jointAngles.joint_state);
                    Debug.Log("Joint Positions: " + jointPositionsString);
                    jointPositions.Add(jointAngles.joint_state[dofStartIndex + j]);
                }
            }
            // Set the joint positions for the root articulation body, propagating to the entire hierarchy
            articulations[0].SetJointPositions(jointPositions);

        }

    }

}

