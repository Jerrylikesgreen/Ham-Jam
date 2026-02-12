using UnityEngine;
using System.Collections;

public class MinionDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Base damage BEFORE upgrades")]
    public float baseDamagePerAttack = 10f;

    public float attackInterval = 2f;

    // ── NEW: Inspector debug tracker ──
    [Header("Runtime Debug (Read Only)")]
    [SerializeField, ReadOnly] private float finalDamageDisplay;
    [SerializeField, ReadOnly] private float damageMultiplierDisplay;

    private CastleHealth castle;
    private bool isAttacking = false;
    private float finalDamagePerAttack;

    void Start()
    {
        castle = FindObjectOfType<CastleHealth>();
        if (castle == null)
        {
            Debug.LogError("MinionDamage: No CastleHealth found!");
        }

        // Apply upgrade multiplier
        float multiplier = UpgradeManager.GetDamageMultiplier();
        finalDamagePerAttack = baseDamagePerAttack * multiplier;

        // Fill debug fields
        finalDamageDisplay = finalDamagePerAttack;
        damageMultiplierDisplay = multiplier;

        Debug.Log($"Minion damage set to {finalDamagePerAttack} (base {baseDamagePerAttack} × {multiplier}x)");
    }

    public void StartAttacking()
    {
        if (isAttacking || castle == null) return;

        isAttacking = true;
        Debug.Log($"{gameObject.name} started attacking castle!");
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (isAttacking && castle != null)
        {
            castle.TakeDamage(finalDamagePerAttack);  // ← use upgraded value
            yield return new WaitForSeconds(attackInterval);
        }
    }

    void OnDestroy()
    {
        isAttacking = false;
        StopAllCoroutines();
    }
}
