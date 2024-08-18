using UnityEngine;

public class RotateToFaceMouse : MonoBehaviour
{
    public Transform targetTransform;  // The transform that this object should follow
    public float rotationSpeed = 5f;   // Speed at which the object rotates to follow the mouse
    public float rotationOffset = -90f;  // Offset angle for rotation

    public bool canRotate = true;

    void Update()
    {
        // Set the position of this object to the target transform's position
        if (targetTransform != null)
        {
            transform.position = targetTransform.position;
        }

        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure z is 0 since this is 2D

        // Calculate the direction vector from the object to the mouse position
        Vector3 direction = mousePosition - transform.position;

        // Calculate the angle between the object's forward vector and the direction to the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation offset
        angle += rotationOffset;

        // Create a target rotation based on the calculated angle with the offset
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        if (canRotate)
        {
            // Rotate the object towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
