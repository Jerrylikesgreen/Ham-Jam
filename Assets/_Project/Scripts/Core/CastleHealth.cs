using UnityEngine;

public class CastleHealth : MonoBehaviour
{
    [Header("Castle Health")]
    public float maxHealth = 1000f;
    public GameObject levelWonUI;  // Drag LevelWonUI here

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Castle Health: {currentHealth}/{maxHealth}");  // Console feedback

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Time.timeScale = 0f;  // Freeze game
            if (levelWonUI != null)
            {
                levelWonUI.SetActive(true);
            }
            Debug.Log("LEVEL WON!");
        }
    }
}