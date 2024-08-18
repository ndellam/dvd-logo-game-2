using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSpawner : MonoBehaviour
{
    public GameObject warningPrefab; // Prefab to spawn at the intersection point
    public Vector2 rectangleCenter; // Center of the rectangle
    public float rectangleWidth = 10f; // Width of the rectangle
    public float rectangleHeight = 5f; // Height of the rectangle

    private void OnDrawGizmos()
    {
        // Draw the rectangle as a gizmo
        Gizmos.color = Color.red;

        // Calculate half-width and half-height
        float halfWidth = rectangleWidth / 2;
        float halfHeight = rectangleHeight / 2;

        // Calculate rectangle corners relative to the center
        Vector2 topLeft = rectangleCenter + new Vector2(-halfWidth, halfHeight);
        Vector2 topRight = rectangleCenter + new Vector2(halfWidth, halfHeight);
        Vector2 bottomLeft = rectangleCenter + new Vector2(-halfWidth, -halfHeight);
        Vector2 bottomRight = rectangleCenter + new Vector2(halfWidth, -halfHeight);

        // Draw the rectangle perimeter as lines
        Gizmos.DrawLine(topLeft, topRight); // Top edge
        Gizmos.DrawLine(topRight, bottomRight); // Right edge
        Gizmos.DrawLine(bottomRight, bottomLeft); // Bottom edge
        Gizmos.DrawLine(bottomLeft, topLeft); // Left edge
    }

    private void Start()
    {
        SpawnWarning();
    }

    public void SpawnWarning()
    {
        // Calculate the vector from the current position to the rectangle's center
        Vector2 direction = rectangleCenter - (Vector2)transform.position;

        // Find the intersection point between the vector and the rectangle
        Vector2 intersectionPoint = GetIntersectionPoint(transform.position, direction);

        if (intersectionPoint != Vector2.zero)
        {
            // Spawn the prefab at the intersection point
            GetComponent<DVDLogoMovement>().warningObject = Instantiate(warningPrefab, intersectionPoint, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No intersection found between the vector and the rectangle.");
        }
    }

    private Vector2 GetIntersectionPoint(Vector2 origin, Vector2 direction)
    {
        // Calculate the edges of the rectangle
        float halfWidth = rectangleWidth / 2;
        float halfHeight = rectangleHeight / 2;

        float left = rectangleCenter.x - halfWidth;
        float right = rectangleCenter.x + halfWidth;
        float top = rectangleCenter.y + halfHeight;
        float bottom = rectangleCenter.y - halfHeight;

        // Normalize the direction vector
        direction.Normalize();

        // List to store potential intersection points
        List<Vector2> intersectionPoints = new List<Vector2>();

        // Check intersection with the left edge
        if (direction.x != 0)
        {
            float tLeft = (left - origin.x) / direction.x;
            Vector2 pointLeft = origin + tLeft * direction;
            if (pointLeft.y >= bottom && pointLeft.y <= top)
            {
                intersectionPoints.Add(pointLeft);
            }
        }

        // Check intersection with the right edge
        if (direction.x != 0)
        {
            float tRight = (right - origin.x) / direction.x;
            Vector2 pointRight = origin + tRight * direction;
            if (pointRight.y >= bottom && pointRight.y <= top)
            {
                intersectionPoints.Add(pointRight);
            }
        }

        // Check intersection with the top edge
        if (direction.y != 0)
        {
            float tTop = (top - origin.y) / direction.y;
            Vector2 pointTop = origin + tTop * direction;
            if (pointTop.x >= left && pointTop.x <= right)
            {
                intersectionPoints.Add(pointTop);
            }
        }

        // Check intersection with the bottom edge
        if (direction.y != 0)
        {
            float tBottom = (bottom - origin.y) / direction.y;
            Vector2 pointBottom = origin + tBottom * direction;
            if (pointBottom.x >= left && pointBottom.x <= right)
            {
                intersectionPoints.Add(pointBottom);
            }
        }

        // Find the closest intersection point
        Vector2 closestPoint = Vector2.zero;
        float minDistance = Mathf.Infinity;

        foreach (Vector2 point in intersectionPoints)
        {
            float distance = Vector2.Distance(origin, point);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }
}
