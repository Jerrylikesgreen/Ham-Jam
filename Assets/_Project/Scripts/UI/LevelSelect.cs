using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    [Header("Level Scene Names")]
    [SerializeField] private string level1Scene;
    [SerializeField] private string level2Scene;
    [SerializeField] private string level3Scene;

    void Start()
    {
        // Attach button click listeners
        if (level1Button != null) level1Button.onClick.AddListener(() => LoadLevel(level1Scene));
        if (level2Button != null) level2Button.onClick.AddListener(() => LoadLevel(level2Scene));
        if (level3Button != null) level3Button.onClick.AddListener(() => LoadLevel(level3Scene));
    }

    /// <summary>
    /// Loads the level by scene name.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    private void LoadLevel(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"Loading Level: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name not set for this level button!");
        }
    }
}
