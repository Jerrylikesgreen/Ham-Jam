using UnityEngine;
using UnityEngine.UI;
using TMPro;   // For TMP_Dropdown
using System.Collections.Generic;

public class DisplayMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Button backButton;       // <-- Back button
    [SerializeField] private GameObject displayPanel; // The panel to deactivate

    private Resolution[] resolutions;

    private void Start()
    {
        SetupResolutions();
        LoadSettings();

        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        backButton.onClick.AddListener(ClosePanel); // <-- Hook back button
    }

    // ---------------- RESOLUTION ----------------

    private void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetBrightness(float value)
    {
        RenderSettings.ambientIntensity = value;
        PlayerPrefs.SetFloat("Brightness", value);
    }

    private void LoadSettings()
    {
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutionDropdown.value);
        resolutionDropdown.value = resolutionIndex;
        SetResolution(resolutionIndex);

        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        float brightness = PlayerPrefs.GetFloat("Brightness", 1f);
        brightnessSlider.value = brightness;
        SetBrightness(brightness);
    }

    // ---------------- BACK BUTTON ----------------

    private void ClosePanel()
    {
        displayPanel.SetActive(false);
    }
}
