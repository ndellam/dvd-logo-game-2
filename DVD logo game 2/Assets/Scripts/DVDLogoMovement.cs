using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DVDLogoMovement : MonoBehaviour
{
    public float baseSpeed = 5f, currentSpeed;
    public Vector2 moveDirection;
    public bool firstBorderHit = false; // Flag to track the first border collision
    public RectangleSpawner rectangleSpawner;
    public int sizeTier = 1;
    private bool hasSpawned = false; // Flag to ensure spawning happens only once
    public bool canCollideWithNonBorder = false; // Flag to determine if it can collide with non-border objects
    private Collider2D firstBorderCollider; // To store the first border collider

    public UIManager uImanager;
    public GameObject warningObject;

    bool hasBeenHitByPlayer;

    public AudioManager audioManager;

    public AudioClip hitClip, resetComboClip;
    public AudioClip[] combineClip;

    // Method to set the movement direction from external scripts
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Start()
    {
        currentSpeed = baseSpeed;
    }
    void Update()
    {
        // Move the object in the set direction
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(this.name + " collided with " + collision.gameObject);
        // Check if the object collided with an object in the "Reflect" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Reflect"))
        {
            if (collision.gameObject.CompareTag("Logo") && canCollideWithNonBorder)
            {
                DVDLogoMovement dVDLogoMovement = collision.gameObject.GetComponent<DVDLogoMovement>();
                if (sizeTier < rectangleSpawner.logoPrefabs.Count() && dVDLogoMovement.sizeTier == sizeTier && (hasBeenHitByPlayer || dVDLogoMovement.hasBeenHitByPlayer))
                {
                    if (!dVDLogoMovement.hasSpawned)
                    {
                        
                        rectangleSpawner.SpawnLogo(sizeTier, collision.contacts[0].point, true);
                        if (sizeTier == 1)
                        {
                            rectangleSpawner.SpawnLogos(2);
                        }
                        else
                        {
                            rectangleSpawner.SpawnLogos(1);
                        }
                        uImanager.UpdateCombo();
                        uImanager.AddScore(sizeTier);


                        rectangleSpawner.SpawnComboText(collision.contacts[0].point);

                        audioManager.PlayClipAscendingPitchByCombo(combineClip[Random.Range(0, combineClip.Count())]);

                        hasSpawned = true; // Mark that spawning has occurred
                    }

                    else
                    {
                        Destroy(collision.gameObject);
                        Destroy(this.gameObject);
                    }
                }

                else
                {
                    ContactPoint2D contact = collision.contacts[0];
                    Vector2 collisionNormal = contact.normal;
                    moveDirection = Vector2.Reflect(moveDirection, collisionNormal).normalized;
                }
            }

            else if (collision.gameObject.CompareTag("Border"))
            {
                if (firstBorderHit)
                {
                    // Reflect the movement direction based on the collision normal after the first hit
                    ContactPoint2D contact = collision.contacts[0];
                    Vector2 collisionNormal = contact.normal;
                    moveDirection = Vector2.Reflect(moveDirection, collisionNormal).normalized;
                }
                else
                {
                    // If this is the first hit, don't change direction but mark that it has been hit
                    firstBorderHit = true;
                    firstBorderCollider = collision.collider; // Store the first border collider
                }

                currentSpeed = baseSpeed;

                if (hasBeenHitByPlayer)
                {
                    if (uImanager.combo > 1)
                    {
                        audioManager.PlayClip(resetComboClip);
                    }
                    uImanager.ResetCombo();
                }

                hasBeenHitByPlayer = false;
                
            }
            else if (canCollideWithNonBorder) // Check if it can collide with non-border objects
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    currentSpeed = 5f;

                    hasBeenHitByPlayer = true;

                }
                // Handle reflection with other objects in the "Reflect" layer
                ContactPoint2D contact = collision.contacts[0];
                Vector2 collisionNormal = contact.normal;
                moveDirection = Vector2.Reflect(moveDirection, collisionNormal).normalized;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the object has exited the first border's collider
        if (firstBorderHit && collision.collider == firstBorderCollider)
        {
            canCollideWithNonBorder = true; // Allow collision with non-border objects after exiting the first border
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Reflect"))
        {
            if (collision.gameObject.CompareTag("Attack") && canCollideWithNonBorder)
            {

                Vector2 directionFromParent = (transform.position - collision.transform.parent.position).normalized;

                //Vector2 directionTowardsCursor = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;


                // Set the moveDirection to this new direction
                moveDirection = directionFromParent;

                //moveDirection = directionTowardsCursor;

                currentSpeed = 5f;

                hasBeenHitByPlayer = true;

                audioManager.PlayClip(hitClip);

            }
        }

        else if (collision.gameObject == warningObject)
        {
            Destroy(collision.gameObject);
        }
    }
}
