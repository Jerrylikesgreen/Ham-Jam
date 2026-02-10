using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;

    [HideInInspector]
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private bool reachedEnd = false;  // NEW: Prevent multiple attack calls

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (currentWaypointIndex >= waypoints.Length)
        {
            if (!reachedEnd)
            {
                reachedEnd = true;
                // NEW: Start damaging instead of destroy
                MinionDamage damageScript = GetComponent<MinionDamage>();
                if (damageScript != null)
                {
                    damageScript.StartAttacking();
                }
            }
            return;  // Stop moving, keep attacking
        }

        // Move toward current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetWaypoint.position,
            moveSpeed * Time.deltaTime);

        // Advance waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex++;
        }
    }
}