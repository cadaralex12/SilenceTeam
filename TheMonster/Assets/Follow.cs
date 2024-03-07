using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Get the position of the main camera
            Vector3 cameraPosition = mainCamera.transform.position;

            // Set the position of the text object to follow the camera
            transform.position = new Vector3(cameraPosition.x, cameraPosition.y, transform.position.z);
        }
        else
        {
            Debug.LogWarning("Main camera reference is missing.");
        }
    }
}
