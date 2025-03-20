using UnityEngine;

public class TextFaceCamera : MonoBehaviour
{
    private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache the reference to the main camera
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            // Rotate the text object to face the camera
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                                mainCamera.transform.rotation * Vector3.up);
        }
    }
}
