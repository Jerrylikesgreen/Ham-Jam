using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;

    [HideInInspector]
    public Transform[] waypoints;   // Will be assigned by spawner

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (currentWaypointIndex >= waypoints.Length)
        {
            // Reached the castle → damage player lives
            Debug.Log("Meat monster reached the castle!");
            Destroy(gameObject);
            return;
        }

        // Move toward current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(
            transform.position, 
            targetWaypoint.position, 
            moveSpeed * Time.deltaTime);

        // When close enough, go to next waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex++;
        }
    }
}