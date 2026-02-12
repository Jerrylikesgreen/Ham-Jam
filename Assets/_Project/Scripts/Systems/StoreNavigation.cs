using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreNavigation : MonoBehaviour
{
    [Header("Fallback Scenes")]
    [SerializeField] private string fallbackSceneIfNoNext = "MainMenu";     // If next level doesn't exist
    [SerializeField] private string levelPrefix = "Level";                  // So Level1 → Level2 → Level3 etc.

    /// <summary>
    /// Called by your "Next Level" / "Continue" button in the Upgradestore scene
    /// Loads the next logical level based on the last completed one
    /// </summary>
    public void GoToNextLevel()
    {
        // Get the last completed level number (saved when winning a level)
        int lastCompleted = PlayerPrefs.GetInt("LastCompletedLevel", 0);

        // Calculate next level number
        int nextLevelNumber = lastCompleted + 1;

        // Build scene name (Level1, Level2, etc.)
        string nextSceneName = levelPrefix + nextLevelNumber;

        // Check if that scene actually exists in Build Settings
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Time.timeScale = 1f;  // Just in case it was paused
            SceneManager.LoadScene(nextSceneName);
            Debug.Log($"Loading next level: {nextSceneName}");
        }
        else
        {
            // No more levels → fallback to main menu (or show "Game Complete" screen later)
            Debug.LogWarning($"Next level '{nextSceneName}' not found in Build Settings. Falling back to {fallbackSceneIfNoNext}");
            SceneManager.LoadScene(fallbackSceneIfNoNext);
        }
    }
}
