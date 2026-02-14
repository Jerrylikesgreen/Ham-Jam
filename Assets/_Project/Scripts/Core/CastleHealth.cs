using UnityEngine;
using UnityEngine.SceneManagement;

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


    private float currentHealth;
    private bool rewardGiven = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        sfxPlayer.PlayMinionAtk();

        Debug.Log($"Castle Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0 && !rewardGiven)
        {
            rewardGiven = true;
            currentHealth = 0;

            GiveGoldReward();

            Time.timeScale = 0f;
            if (levelWonUI != null)
            {
                levelWonUI.SetActive(true);
            }

            Debug.Log("LEVEL WON!");
        }
    }

    private void GiveGoldReward()
    {
        // Add gold
        int totalGold = PlayerPrefs.GetInt("TotalGold", 0);
        totalGold += goldReward;
        PlayerPrefs.SetInt("TotalGold", totalGold);

        // NEW: Save which level was just completed
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName.StartsWith("Level"))
        {
            string levelNumStr = currentSceneName.Replace("Level", "");
            if (int.TryParse(levelNumStr, out int levelNumber))
            {
                PlayerPrefs.SetInt("LastCompletedLevel", levelNumber);
                Debug.Log($"Saved completed level: {levelNumber} (from scene name '{currentSceneName}')");
            }
            else
            {
                Debug.LogWarning($"Could not parse level number from scene name: {currentSceneName}");
            }
        }
        else
        {
            Debug.LogWarning($"Current scene '{currentSceneName}' does not start with 'Level' – level progress not saved.");
        }

        PlayerPrefs.Save();  // One save at the end is enough
    }

    // Optional helpers (you can keep or remove AddGold if unused)
    public static int GetTotalGold()
    {
        return PlayerPrefs.GetInt("TotalGold", 0);
    }

}