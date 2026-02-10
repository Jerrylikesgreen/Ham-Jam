using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWonButtonManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";  // Editable in Inspector

    /// <summary>
    /// Called by Restart button - Loads MainMenu scene and resumes time
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;  // Reset game time (was paused on win)
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
