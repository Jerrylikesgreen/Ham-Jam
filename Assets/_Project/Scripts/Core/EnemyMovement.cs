using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Base speed BEFORE upgrades")]
    public float baseMoveSpeed = 3.5f;

    [HideInInspector] public Transform[] waypoints;

    [Header("Runtime Debug (Read Only)")]
    [SerializeField, ReadOnly] private float finalMoveSpeedDisplay;
    [SerializeField, ReadOnly] private float speedMultiplierDisplay;

    private float moveSpeed;
    private int currentWaypointIndex = 0;
    private bool reachedEnd = false;
    private Animator animator;
    private MinionHealth health;  // ← NEW: Reference to check isDead

    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<MinionHealth>();  // ← NEW

        float multiplier = UpgradeManager.GetSpeedMultiplier();
        moveSpeed = baseMoveSpeed * multiplier;

        finalMoveSpeedDisplay = moveSpeed;
        speedMultiplierDisplay = multiplier;

        Debug.Log($"[{gameObject.name}] Speed set to {moveSpeed} (base {baseMoveSpeed} × {multiplier}x)");
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        // NEW: Stop all movement logic if dead
        if (health != null && health.IsDead)
        {
            return;
        }

        bool isMoving = currentWaypointIndex < waypoints.Length;

        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }

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