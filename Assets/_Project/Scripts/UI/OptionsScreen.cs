using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsScreen : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button soundMenuButton;
    [SerializeField] private Button displayPanelButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Panels")]
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject displayPanel;

    private void Start()
    {
        soundMenuButton.onClick.AddListener(OpenSoundPanel);
        displayPanelButton.onClick.AddListener(OpenDisplayPanel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void OpenSoundPanel()
    {
        soundPanel.SetActive(true);
        displayPanel.SetActive(false);
    }

    private void OpenDisplayPanel()
    {
        soundPanel.SetActive(false);
        displayPanel.SetActive(true);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Make sure scene name matches exactly
    }
}
