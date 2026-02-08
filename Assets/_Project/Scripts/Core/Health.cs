using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO later: death particles, sound, give player gold, etc.
        Destroy(gameObject);
    }

    // Optional: for debugging
    private void OnMouseDown()
    {
        TakeDamage(30f);   // Click to test damage
    }
}