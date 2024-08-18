using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the object moves
    public float rotationSpeed = 5f; // Speed at which the object rotates
    public float rotationOffset = 0f; // Offset angle for rotation

    public float currentSpeed = 0f; // Stores the current speed of the object
    public float maxSpeed = 15f; // The speed at which the animation runs at 1x speed

    public bool canMove = false, canRotate = true;
    private Vector3 lastPosition; // To store the position from the previous frame

    public Animator animator, legsAnimator;

    void Start()
    {
        // Initialize lastPosition with the current position
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure z is 0 since this is 2D

        if (canMove)
        {
            transform.position = Vector3.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);
        }

        // Calculate the direction vector from the object to the mouse position
        Vector3 direction = mousePosition - transform.position;

        // Calculate the angle between the object's forward vector and the direction to the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation offset
        angle += rotationOffset;

        // Rotate the object to face the mouse cursor with the applied offset
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        if (canRotate)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Calculate the current speed of the object
        currentSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;

        // Update lastPosition to the current position for the next frame
        lastPosition = transform.position;

        // Adjust animation speed based on the current speed
        if (currentSpeed > 0.1f)
        {
            animator.SetBool("isWalk", true);
            legsAnimator.SetBool("isWalk", true);

            // Calculate the animation speed multiplier based on the current speed
            float animationSpeed = Mathf.Clamp(currentSpeed / maxSpeed, 0.1f, 1f);

            // Only modify the speed if in the walking state
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                animator.speed = animationSpeed;
                legsAnimator.speed = animationSpeed;

            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                animator.speed = 1f;
                legsAnimator.speed = animationSpeed;
            }
            else
            {
                // Reset the speed for all other states
                animator.speed = 1f;
                legsAnimator.speed = 1f;
            }



        }
        else
        {
            animator.SetBool("isWalk", false);
            legsAnimator.SetBool("isWalk", false);
            animator.speed = 1f; // Ensure the speed is reset when not moving
        }
    }

    public void CanMove()
    {
        canMove = true;
    }

    public void CantMove()
    {
        canMove = false;
    }
}
