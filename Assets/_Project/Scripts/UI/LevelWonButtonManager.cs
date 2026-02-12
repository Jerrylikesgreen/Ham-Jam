using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWonButtonManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string upgradeStoreSceneName = "Upgradestore";  // ← Add this field

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // NEW: Call this from your "Go to Store" / "Upgrades" button
    public void LoadUpgradeStore()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(upgradeStoreSceneName);
    }
}
