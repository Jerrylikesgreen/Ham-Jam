using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip menuMusicClip;
    public AudioClip levelMusicClip;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float menuVolume = 0.8f;
    [Range(0f, 1f)] public float levelVolume = 0.8f;
    [Range(0f, 1f)] public float storeVolume = 0.4f;        // ← Half volume in store

    [Header("Fade Settings")]
    public float fadeDuration = 2f;

    private AudioSource menuSource;
    private AudioSource levelSource;

    private string currentScene;

    void Awake()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SetupSources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void SetupSources()
    {
        menuSource = gameObject.AddComponent<AudioSource>();
        menuSource.clip = menuMusicClip;
        menuSource.loop = true;
        menuSource.volume = 0f;

        levelSource = gameObject.AddComponent<AudioSource>();
        levelSource.clip = levelMusicClip;
        levelSource.loop = true;
        levelSource.volume = 0f;
    }

    void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(CrossfadeToScene(scene.name));
    }

    private IEnumerator CrossfadeToScene(string sceneName)
    {
        bool isMenu  = sceneName == "MainMenu";
        bool isStore = sceneName == "UpgradeStore";

        AudioSource targetSource = isMenu ? menuSource : levelSource;
        float targetVol = isMenu ? menuVolume : (isStore ? storeVolume : levelVolume);

        AudioSource otherSource = isMenu ? levelSource : menuSource;

        // Make sure target is playing
        if (!targetSource.isPlaying)
            targetSource.Play();

        // Crossfade
        float elapsed = 0f;
        float startTargetVol = targetSource.volume;
        float startOtherVol  = otherSource.volume;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;

            targetSource.volume = Mathf.Lerp(startTargetVol, targetVol, t);
            otherSource.volume  = Mathf.Lerp(startOtherVol, 0f, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap final values
        targetSource.volume = targetVol;
        otherSource.volume = 0f;

        // Stop the one we faded out
        if (otherSource.volume <= 0.01f)
            otherSource.Stop();

        currentScene = sceneName;

        string volDesc = isMenu ? "Menu" : (isStore ? "Store (half)" : "Level (full)");
        Debug.Log($"Crossfaded to {volDesc} music → {sceneName}");
    }

    // Optional: public setters for future sliders
    public void SetMenuVolume(float v)  { menuVolume = v;  if (currentScene == "MainMenu") menuSource.volume = v; }
    public void SetLevelVolume(float v) { levelVolume = v; if (currentScene != "MainMenu" && currentScene != "UpgradeStore") levelSource.volume = v; }
    public void SetStoreVolume(float v) { storeVolume = v; if (currentScene == "UpgradeStore") levelSource.volume = v; }
}
