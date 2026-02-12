using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Base speed BEFORE upgrades")]
    public float baseMoveSpeed = 3.5f;

    [HideInInspector]
    public Transform[] waypoints;

    // ── NEW: Inspector debug fields ──
    [Header("Runtime Debug (Read Only)")]
    [SerializeField, ReadOnly] private float finalMoveSpeedDisplay;
    [SerializeField, ReadOnly] private float speedMultiplierDisplay;

    private float moveSpeed;   // final calculated speed
    private int currentWaypointIndex = 0;
    private bool reachedEnd = false;

    void Start()
    {
        // Apply upgrade multiplier
        float multiplier = UpgradeManager.GetSpeedMultiplier();
        moveSpeed = baseMoveSpeed * multiplier;

        // Fill debug fields for Inspector
        finalMoveSpeedDisplay = moveSpeed;
        speedMultiplierDisplay = multiplier;

        Debug.Log($"Minion speed set to {moveSpeed} (base {baseMoveSpeed} × {multiplier}x)");
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (currentWaypointIndex >= waypoints.Length)
        {
            if (!reachedEnd)
            {
                reachedEnd = true;
                MinionDamage damage = GetComponent<MinionDamage>();
                if (damage != null)
                {
                    damage.StartAttacking();
                }
            }
            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetWaypoint.position,
            moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex++;
        }
    }
}