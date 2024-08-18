using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbdomenController : MonoBehaviour
{
    public Sprite[] sprites; // Array to hold the 3 different sprites
    public GameObject abdomenPiece, bloodSplatter; // Prefab for spawning additional sprites

    public Sprite[] bloodSplatters;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    private void Awake()
    {
        // Get the SpriteRenderer component on this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Check to ensure sprites are assigned
        if (sprites.Length != 3)
        {
            Debug.LogError("Please assign exactly 3 sprites to the sprites array.");
        }
    }

    // Method to change the sprite
    public void ChangeSprite(int spriteIndex)
    {
        if (spriteIndex >= 0 && spriteIndex < sprites.Length)
        {
            // Change the sprite
            spriteRenderer.sprite = sprites[spriteIndex];

            SpawnAdditionalSprite();
        }
        else
        {
            Debug.LogError("Invalid sprite index. Please provide a valid index between 0 and 2.");
        }
    }

    // Method to spawn an additional sprite at the same position with a random rotation
    private void SpawnAdditionalSprite()
    {
        if (abdomenPiece != null)
        {
            // Instantiate the prefab at the current position
            GameObject spawnedSprite = Instantiate(abdomenPiece, transform.position, Quaternion.identity);

            // Apply a random rotation to the spawned sprite
            float randomRotation = Random.Range(0f, 360f);
            spawnedSprite.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
        }
        else
        {
            Debug.LogError("Sprite prefab not assigned. Please assign a prefab to the spritePrefab field.");
        }

        if (bloodSplatter != null)
        {
            // Instantiate the prefab at the current position
            GameObject spawnedSprite = Instantiate(bloodSplatter, transform.position, Quaternion.identity);

            spawnedSprite.GetComponent<SpriteRenderer>().sprite = bloodSplatters[Random.Range(0, bloodSplatters.Count())];

            // Apply a random rotation to the spawned sprite
            float randomRotation = Random.Range(0f, 360f);
            spawnedSprite.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
        }
        else
        {
            Debug.LogError("Blood prefab not assigned. Please assign a prefab to the bloodSplatter field.");
        }
    }
}
