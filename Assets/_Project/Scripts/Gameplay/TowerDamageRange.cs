using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class TowerDamageRange : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damage = 20f;
    public float attackInterval = 1f;

    private float attackTimer;
    private HashSet<MinionHealth> targets = new();

    private void Update()
    {
        // Remove null references (if enemies died)
        targets.RemoveWhere(t => t == null);

        if (targets.Count == 0)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackInterval;
        }
    }

    private void Attack()
    {
        // Basic: attack first available target
        foreach (var target in targets)
        {
            if (target != null)
            {
                target.TakeDamage(damage);
                Debug.Log($"[Tower] Dealt {damage} to {target.name}");
                break; // single target tower
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out MinionHealth health))
        {
            targets.Add(health);
            Debug.Log($"{other.name} entered range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out MinionHealth health))
        {
            targets.Remove(health);
            Debug.Log($"{other.name} exited range.");
        }
    }
}
