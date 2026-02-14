using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button menuButton;        // Main menu button
    [SerializeField] private GameObject mainMenuPanel; // Main menu panel (Options)
    [SerializeField] private GameObject soundMenuPanel; // Sound menu panel
    [SerializeField] private GameObject displayMenuPanel; // Display menu panel

    [Header("Camera Reference")]
    [SerializeField] private CameraControler cameraMovement; // Your camera movement script

    private bool isMenuOpen = false;

    private void Start()
    {
        // Ensure panels start hidden
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (soundMenuPanel != null) soundMenuPanel.SetActive(false);
        if (displayMenuPanel != null) displayMenuPanel.SetActive(false);

        // Bind main menu button
        if (menuButton != null)
            menuButton.onClick.AddListener(ToggleMenu);
        else
            Debug.LogWarning("MenuButton not assigned!");
    }

    /// <summary>
    /// Toggles the main menu panel
    /// </summary>
    private void ToggleMenu()
    {
        // Make sure this is triggered by the button, not clicks elsewhere
        if (!EventSystem.current.IsPointerOverGameObject())
            return;

        isMenuOpen = !isMenuOpen;

        // Show/hide main menu
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(isMenuOpen);

        // Lock/unlock camera movement
        if (cameraMovement != null)
            cameraMovement.LockMovement(isMenuOpen);

        Debug.Log($"Menu toggled. isMenuOpen = {isMenuOpen}");
    }

    /// <summary>
    /// Opens a submenu (Sound or Display) and keeps main menu visible
    /// </summary>
    public void OpenSubMenu(GameObject submenu)
    {
        if (submenu == null) return;

        // Hide all submenus first
        if (soundMenuPanel != null) soundMenuPanel.SetActive(false);
        if (displayMenuPanel != null) displayMenuPanel.SetActive(false);

        // Show the requested submenu
        submenu.SetActive(true);

        // Keep camera locked while any menu is open
        if (cameraMovement != null)
            cameraMovement.LockMovement(true);

        Debug.Log($"Opened submenu: {submenu.name}");
    }

    /// <summary>
    /// Closes any open submenu, returns to main menu
    /// </summary>
    public void CloseSubMenu(GameObject submenu)
    {
        if (submenu == null) return;

        submenu.SetActive(false);

        // Keep camera locked if main menu is still open
        if (cameraMovement != null)
            cameraMovement.LockMovement(isMenuOpen);

        Debug.Log($"Closed submenu: {submenu.name}");
    }

    /// <summary>
    /// Close the main menu entirely
    /// </summary>
    public void CloseMainMenu()
    {
        isMenuOpen = false;

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (soundMenuPanel != null)
            soundMenuPanel.SetActive(false);

        if (displayMenuPanel != null)
            displayMenuPanel.SetActive(false);

        if (cameraMovement != null)
            cameraMovement.LockMovement(false);

        Debug.Log("Main menu closed.");
    }
}
