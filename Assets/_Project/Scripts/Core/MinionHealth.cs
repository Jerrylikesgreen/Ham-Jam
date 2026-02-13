using UnityEngine;

public class MinionHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Base health BEFORE upgrades")]
    public float baseMaxHealth = 100f;

    [Header("Runtime Debug (Read Only)")]
    [SerializeField, ReadOnly] private float currentHealthDisplay;
    [SerializeField, ReadOnly] private float maxHealthAfterUpgrades;

    private float currentHealth;
    private bool isDead = false;
    private Animator animator;

    public bool IsDead => isDead;  // ← NEW: Public getter so EnemyMovement can read it

    void Start()
    {
        animator = GetComponent<Animator>();

        int upgradeLevel = UpgradeManager.GetHealthUpgradeLevel();
        float multiplier = 1f + (upgradeLevel * 0.10f);
        float finalMaxHealth = baseMaxHealth * multiplier;

        maxHealthAfterUpgrades = finalMaxHealth;
        currentHealth = finalMaxHealth;
        currentHealthDisplay = currentHealth;

        Debug.Log($"[{gameObject.name}] Spawned | Base: {baseMaxHealth} | Final Max: {finalMaxHealth}");
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealthDisplay = currentHealth;

        Debug.Log($"[{gameObject.name}] Took {damage} dmg → health now {currentHealth}");

        if (animator != null)
        {
            animator.SetTrigger("TakeHit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"[{gameObject.name}] Dying → playing death anim, destroy in 2s");

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Destroy(gameObject, 2f);
    }
}