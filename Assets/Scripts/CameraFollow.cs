using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target; // The target the camera follows
    public float smoothSpeed = 0.125f; // How quickly the camera moves towards the target
    public Vector3 offset; // The camera's offset from the target

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset; // Calculate the desired position of the camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Smoothly move the camera towards the desired position
        transform.position = smoothedPosition; // Update the camera's position
    }
}