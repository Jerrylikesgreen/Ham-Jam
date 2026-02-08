using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class TowerDamageRange : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damage = 20f;          // Damage per attack
    public float attackInterval = 1f;   // Time between attacks in seconds

    private float attackTimer;
    private List<MinionHealth> targets = new(); // preserves entry order

    private void Update()
    {
        // Remove destroyed or null enemies
        targets.RemoveAll(t => t == null);

        if (targets.Count == 0)
            return;

        // Countdown timer
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackInterval;
        }
    }

    private void Attack()
    {
        // Attack the first enemy that entered the area
        for (int i = 0; i < targets.Count; i++)
        {
            var target = targets[i];
            if (target != null)
            {
                target.TakeDamage(damage);
                Debug.Log($"[Tower] Dealt {damage} to {target.name} (first in range)");
                break; // single target tower
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out MinionHealth health))
        {
            // Add to the end of the list (preserves entry order)
            targets.Add(health);
            Debug.Log($"{other.name} entered tower range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out MinionHealth health))
        {
            // Remove from list when leaving range
            targets.Remove(health);
            Debug.Log($"{other.name} exited tower range.");
        }
    }

}
