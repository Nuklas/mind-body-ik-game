using UnityEngine;

namespace MyGame
{
    public class TextFaceCamera : MonoBehaviour
    {
        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            if (mainCamera != null)
            {
                // Rotate the text object to face the camera
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                                mainCamera.transform.rotation * Vector3.up);
            }
        }
    }
}