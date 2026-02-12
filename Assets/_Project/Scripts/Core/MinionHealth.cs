using UnityEngine;

public class MinionHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Base health BEFORE upgrades")]
    public float baseMaxHealth = 100f;

    // ── NEW: Inspector-visible debug fields (read-only in Play mode) ──
    [Header("Runtime Debug (Read Only)")]
    [SerializeField, ReadOnly] private float currentHealthDisplay;     // Shows live current health
    [SerializeField, ReadOnly] private float maxHealthAfterUpgrades;   // Shows final max health with upgrades

    private float currentHealth;

    void Start()
    {
        // Apply upgrade multiplier
        int upgradeLevel = UpgradeManager.GetHealthUpgradeLevel();
        float multiplier = 1f + (upgradeLevel * 0.10f);  // +10% per level

        float finalMaxHealth = baseMaxHealth * multiplier;
        maxHealthAfterUpgrades = finalMaxHealth;  // ← for Inspector

        currentHealth = finalMaxHealth;
        currentHealthDisplay = currentHealth;     // initial value

        Debug.Log($"Minion spawned | Base: {baseMaxHealth} | Upgrades: {upgradeLevel} | Final Max: {finalMaxHealth}");
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealthDisplay = currentHealth;     // ← updates live in Inspector

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}