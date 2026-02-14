using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;  // Required for coroutine

public class CastleHealth : MonoBehaviour
{
    [Header("Castle Health")]
    public float maxHealth = 1000f;
    public GameObject levelWonUI;

    [Header("Level Reward")]
    [Tooltip("Gold given to the player when they win this level")]
    public int goldReward = 100;

    [Header("SFX")]
    [Tooltip("AudioSource to play timer tick SFX")]
    public SfxPlayer sfxPlayer;

    [Header("Smoke Effect")]
    [Tooltip("Smoke particle emitter GameObject - starts disabled")]
    public GameObject smokeEmitter;  // Drag your smoke GameObject here

    private float currentHealth;
    private bool rewardGiven = false;
    private bool isDestroyed = false;  // Prevent multiple triggers

    void Start()
    {
        currentHealth = maxHealth;

        // Ensure smoke starts disabled
        if (smokeEmitter != null)
        {
            smokeEmitter.SetActive(false);
        }
        else
        {
            Debug.LogWarning("CastleHealth: No smoke emitter assigned in Inspector!");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;  // Safety: ignore damage after death

        currentHealth -= damage;
        sfxPlayer?.PlayMinionAtk();

        Debug.Log($"Castle Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0 && !rewardGiven)
        {
            rewardGiven = true;
            currentHealth = 0;
            isDestroyed = true;

            // Activate smoke immediately
            if (smokeEmitter != null)
            {
                smokeEmitter.SetActive(true);
                Debug.Log("Castle destroyed → smoke emitter activated!");
            }

            // Give reward right away (gold & progress save)
            GiveGoldReward();

            // Delay the win UI and timescale freeze by 2 seconds
            StartCoroutine(DelayedLevelEnd(2f));
        }
    }

    private IEnumerator DelayedLevelEnd(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        // Now end the level
        Time.timeScale = 0f;

        if (levelWonUI != null)
        {
            levelWonUI.SetActive(true);
        }

        Debug.Log("LEVEL WON! (after 2-second delay)");
    }

    private void GiveGoldReward()
    {
        int totalGold = PlayerPrefs.GetInt("TotalGold", 0);
        totalGold += goldReward;
        PlayerPrefs.SetInt("TotalGold", totalGold);

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName.StartsWith("Level"))
        {
            string levelNumStr = currentSceneName.Replace("Level", "");
            if (int.TryParse(levelNumStr, out int levelNumber))
            {
                PlayerPrefs.SetInt("LastCompletedLevel", levelNumber);
                Debug.Log($"Saved completed level: {levelNumber} (from scene '{currentSceneName}')");
            }
            else
            {
                Debug.LogWarning($"Could not parse level number from '{currentSceneName}'");
            }
        }
        else
        {
            Debug.LogWarning($"Scene '{currentSceneName}' does not start with 'Level' — progress not saved.");
        }

        PlayerPrefs.Save();
    }

    public static int GetTotalGold()
    {
        return PlayerPrefs.GetInt("TotalGold", 0);
    }
}