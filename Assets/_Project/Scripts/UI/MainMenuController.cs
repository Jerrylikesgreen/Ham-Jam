using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string playSceneName = "Level1";  // Editable in Inspector

    [Header("UI Panels")]
    [SerializeField] private GameObject levelSelectUI;
    [SerializeField] private GameObject creditsUI;

    /// <summary>
    /// Called by Play button - Loads the assigned scene
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(playSceneName);
    }

    /// <summary>
    /// Called by Level Select button - Shows LevelSelectUI, hides main menu
    /// </summary>
    public void ShowLevelSelect()
    {
        gameObject.SetActive(false);  // Hide main menu
        if (levelSelectUI != null)
        {
            levelSelectUI.SetActive(true);
        }
    }

    /// <summary>
    /// Called by Credits button - Shows CreditsUI, hides main menu
    /// </summary>
    public void ShowCredits()
    {
        gameObject.SetActive(false);  // Hide main menu
        if (creditsUI != null)
        {
            creditsUI.SetActive(true);
        }
    }

    /// <summary>
    /// Called by Quit button - Exits application (Stops Play mode in Editor)
    /// </summary>
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // Extra: Stops Editor play mode
        #else
            Application.Quit();
        #endif
    }
    /// <summary>
/// Called by Back/Return buttons on sub-screens
/// Shows main menu and hides the calling panel
/// </summary>
public void ShowMainMenu(GameObject currentSubPanel)
{
    if (currentSubPanel != null)
    {
        currentSubPanel.SetActive(false);
    }
    gameObject.SetActive(true);   // show main menu panel
}
}