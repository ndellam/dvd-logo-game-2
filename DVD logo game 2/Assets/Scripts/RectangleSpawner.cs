using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSpawner : MonoBehaviour
{
    public GameObject[] logoPrefabs;
    public Vector2 center = Vector2.zero; // Center of the rectangle
    public float width = 10f; // Width of the rectangle
    public float height = 5f; // Height of the rectangle
    public float cornerBuffer = 1f; // Buffer zone from the corners where logos can't spawn
    public float initialSpawnRate = 1f; // Initial rate of spawning logos per second

    public int currentSize1Logos = 0;

    public UIManager uImanager;

    public AudioManager audioManager;

    public void SpawnLogos(int amountToSpawn = 2)
    {
        StartCoroutine(SpawnLogosCoroutine(amountToSpawn));
    }

    IEnumerator SpawnLogosCoroutine(int amountToSpawn)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            SpawnLogo(0, GetRandomPointOnPerimeter(), false);

            yield return new WaitForSeconds(2f);
        }
    }

    public void SpawnLogo(int indexToSpawn, Vector2 spawnPoint, bool hasHitFirstBorder)
    {
        // Instantiate the logo prefab
        GameObject logo = Instantiate(logoPrefabs[indexToSpawn], spawnPoint, Quaternion.identity, this.transform);

        // Calculate direction from the logo to the center
        Vector2 direction = (center - spawnPoint).normalized;

        // Set the logo's movement direction
        DVDLogoMovement logoMovement = logo.GetComponent<DVDLogoMovement>();
        if (logoMovement != null)
        {
            logoMovement.SetDirection(direction);
            logoMovement.rectangleSpawner = this;
            logoMovement.firstBorderHit = hasHitFirstBorder;
            logoMovement.canCollideWithNonBorder = hasHitFirstBorder;
            logoMovement.uImanager = uImanager;
            logoMovement.audioManager = audioManager;
        }
    }

    public Vector2 GetRandomPointOnPerimeter()
    {
        // Randomly select a side of the rectangle
        int side = Random.Range(0, 4);
        Vector2 point = Vector2.zero;
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        float horizontalSpawnRange = halfWidth - cornerBuffer;
        float verticalSpawnRange = halfHeight - cornerBuffer;

        switch (side)
        {
            case 0: // Top edge
                point = new Vector2(Random.Range(center.x - horizontalSpawnRange, center.x + horizontalSpawnRange), center.y + halfHeight);
                break;
            case 1: // Bottom edge
                point = new Vector2(Random.Range(center.x - horizontalSpawnRange, center.x + horizontalSpawnRange), center.y - halfHeight);
                break;
            case 2: // Left edge
                point = new Vector2(center.x - halfWidth, Random.Range(center.y - verticalSpawnRange, center.y + verticalSpawnRange));
                break;
            case 3: // Right edge
                point = new Vector2(center.x + halfWidth, Random.Range(center.y - verticalSpawnRange, center.y + verticalSpawnRange));
                break;
        }
        return point;
    }

    // Draw gizmo for rectangle
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Color of the gizmo

        // Calculate half-width and half-height
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        // Calculate rectangle corners relative to the center
        Vector2 topLeft = center + new Vector2(-halfWidth, halfHeight);
        Vector2 topRight = center + new Vector2(halfWidth, halfHeight);
        Vector2 bottomLeft = center + new Vector2(-halfWidth, -halfHeight);
        Vector2 bottomRight = center + new Vector2(halfWidth, -halfHeight);

        // Draw the valid spawn area on each side, avoiding the corners
        DrawValidSpawnLine(new Vector2(topLeft.x + cornerBuffer, topLeft.y), new Vector2(topRight.x - cornerBuffer, topRight.y)); // Top edge
        DrawValidSpawnLine(new Vector2(bottomLeft.x + cornerBuffer, bottomLeft.y), new Vector2(bottomRight.x - cornerBuffer, bottomRight.y)); // Bottom edge
        DrawValidSpawnLine(new Vector2(topLeft.x, topLeft.y - cornerBuffer), new Vector2(bottomLeft.x, bottomLeft.y + cornerBuffer)); // Left edge
        DrawValidSpawnLine(new Vector2(topRight.x, topRight.y - cornerBuffer), new Vector2(bottomRight.x, bottomRight.y + cornerBuffer)); // Right edge
    }

    private void DrawValidSpawnLine(Vector2 start, Vector2 end)
    {
        Gizmos.DrawLine(start, end);
    }
}
