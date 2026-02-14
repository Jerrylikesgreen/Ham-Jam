using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class ResetPlayerPrefsButton : MonoBehaviour
{
    [Header("Safety")]
    [SerializeField] private bool showConfirmationDialog = true;
    [SerializeField]
    private string confirmationMessage =
        "Are you sure you want to reset ALL progress?\n" +
        "This will delete gold, upgrades, completed levels, etc.\n" +
        "This action cannot be undone.";

    [Header("Optional UI Feedback")]
    [SerializeField] private Button resetButton;
    [SerializeField] private string successMessage = "Progress reset! Returning to main menu...";

    public void ResetAllPlayerPrefs()
    {
        if (showConfirmationDialog)
        {
#if UNITY_EDITOR
            if (!EditorUtility.DisplayDialog(
                "Confirm Reset",
                confirmationMessage,
                "Yes, Reset Everything",
                "Cancel"))
            {
                return; // User cancelled
            }
#else
            // WebGL / other platforms - just log or use custom UI
            Debug.LogWarning("Confirmation dialog not available on this platform - resetting directly.");
#endif
        }

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs have been completely reset.");

        if (resetButton != null)
        {
            resetButton.interactable = false;
            TMP_Text tmp = resetButton.GetComponentInChildren<TMP_Text>();
            if (tmp != null) tmp.text = "Reset Done!";
        }

        Invoke(nameof(ReloadMainMenu), 1.5f);
    }

    private void ReloadMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
