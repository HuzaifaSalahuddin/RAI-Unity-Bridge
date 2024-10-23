using UnityEngine;

public class GlobalTransform : MonoBehaviour
{
    // Set desired position and rotation in global coordinates
    //public Vector3 targetPosition = new Vector3(0, 0, 0);
    //public Vector3 targetRotationEulerAngles = new Vector3(0, 0, 0); // Rotation in degrees (Euler angles)
    public Vector3 current_orientation = new Vector3();
    public Vector3 resulting_orientation = new Vector3();
    public Vector3 MY_orientation = new Vector3();
    public Quaternion myQuat = Quaternion.identity;
    public Quaternion globalRotation = Quaternion.identity;
    public Quaternion resultingQuat = Quaternion.identity;
    public float posX;
    public float posY;
    public float posZ;

    void Start()
    {
        // Apply position in global frame
        //transform.position = targetPosition;

    }

    void Update()
    {
        // Optional: Continuously update position and rotation each frame
        // This can be removed if you only want it to apply once at the start
        // Z X Y
        // Create a quaternion for the global rotation
        globalRotation = Quaternion.Euler(transform.eulerAngles);
        //transform.position = targetPosition;
        //transform.rotation = globalRotation;
        current_orientation = transform.position;
        resulting_orientation = myQuat.eulerAngles;
        resultingQuat  = Quaternion.Euler(MY_orientation);
        transform.position = new Vector3(posX, posY, posZ);

        //Debug.Log(current_orientation);
        //transform.Rotate(targetRotationEulerAngles, Space.World);
    }
}


