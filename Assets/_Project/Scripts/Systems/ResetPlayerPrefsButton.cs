using UnityEngine;
using UnityEngine.UI;           // if you want to disable button after reset (optional)
using UnityEngine.SceneManagement;

public class ResetPlayerPrefsButton : MonoBehaviour
{
    [Header("Safety")]
    [SerializeField] private bool showConfirmationDialog = true;    // Recommended: ask before wiping
    [SerializeField] private string confirmationMessage = 
        "Are you sure you want to reset ALL progress?\n" +
        "This will delete gold, upgrades, completed levels, etc.\n" +
        "This action cannot be undone.";

    [Header("Optional UI Feedback")]
    [SerializeField] private Button resetButton;                    // Drag your button here (optional)
    [SerializeField] private string successMessage = "Progress reset! Returning to main menu...";

    public void ResetAllPlayerPrefs()
    {
        if (showConfirmationDialog)
        {
            // Simple native dialog (works in builds + editor)
            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (!UnityEditor.EditorUtility.DisplayDialog(
                "Confirm Reset",
                confirmationMessage,
                "Yes, Reset Everything",
                "Cancel"))
            {
                return; // User cancelled
            }
            #else
            // For mobile/console - you could use a custom UI popup instead
            Debug.LogWarning("Confirmation dialog not shown on this platform - resetting directly.");
            #endif
        }

        // Actually wipe everything
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs have been completely reset.");

        // Optional: visual feedback
        if (resetButton != null)
        {
            resetButton.interactable = false;
            resetButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Reset Done!";
        }

        // Optional: reload main menu to refresh any displayed values
        Invoke(nameof(ReloadMainMenu), 1.5f); // small delay so player sees feedback
    }

    private void ReloadMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // or SceneManager.LoadScene("MainMenu"); if you want to force reload
    }
}
