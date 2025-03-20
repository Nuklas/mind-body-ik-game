using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;             // The player's transform to follow
    public float smoothSpeed = 10f;      // How smoothly the camera follows the player
    public Vector3 offset = new Vector3(5f, 8f, -5f);  // Camera offset for diagonal top-down view
    public Vector3 lookOffset = new Vector3(0f, 0f, 0f); // Where the camera looks (offset from player position)
    
    // Fixed rotation values
    public Vector3 rotationAngles = new Vector3(45f, 45f, 0f); // Fixed camera rotation (x,y,z in degrees)
    
    private Vector3 velocity = Vector3.zero;
    
    private void Start()
    {
        // If no target is specified, try to find the player
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning("No player found with tag 'Player'. Please assign a target to the CameraFollow script.");
            }
        }
        
        // Apply fixed rotation at start
        transform.rotation = Quaternion.Euler(rotationAngles);
    }
    
    private void LateUpdate()
    {
        if (target == null)
            return;
        
        // Calculate desired position - only position changes, rotation stays fixed
        Vector3 desiredPosition = target.position + offset;
        
        // Use SmoothDamp for smoother camera movement without jittering
        // Smaller smoothSpeed values make camera movement more gradual
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / smoothSpeed);
        
        // Keep camera rotation fixed - don't use LookAt as it changes rotation
        transform.rotation = Quaternion.Euler(rotationAngles);
    }
}