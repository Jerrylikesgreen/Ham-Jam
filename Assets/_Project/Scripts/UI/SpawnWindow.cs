using UnityEngine;
using UnityEngine.UI;

public class SpawnWindow : MonoBehaviour
{
    [Header("Mob Progress Bars")]
    [SerializeField] private Slider mob1Progress;
    [SerializeField] private Slider mob2Progress;
    [SerializeField] private Slider mob3Progress;

    [Header("Mob Spawn Buttons")]
    [SerializeField] private Button mob1SpawnButton;
    [SerializeField] private Button mob2SpawnButton;
    [SerializeField] private Button mob3SpawnButton;

    [Header("Spawn Times (seconds)")]
    [SerializeField] private float mob1SpawnTime = 10f;
    [SerializeField] private float mob2SpawnTime = 10f;
    [SerializeField] private float mob3SpawnTime = 10f;

    [Header("Spawn Enabled Flags")]
    [SerializeField] private bool mob1SpawnCountEnabled = true;
    [SerializeField] private bool mob2SpawnCountEnabled = true;
    [SerializeField] private bool mob3SpawnCountEnabled = true;



    [Header("Spawner")]
    [SerializeField] private MinionSpawner spawner;

    private float mob1Timer = 0f;
    private float mob2Timer = 0f;
    private float mob3Timer = 0f;

    void Start()
    {
        // Initialize sliders
        InitializeSlider(mob1Progress, mob1SpawnTime);
        InitializeSlider(mob2Progress, mob2SpawnTime);
        InitializeSlider(mob3Progress, mob3SpawnTime);

        // Disable buttons initially
        mob1SpawnButton.interactable = false;
        mob2SpawnButton.interactable = false;
        mob3SpawnButton.interactable = false;

        // Connect button signals
        mob1SpawnButton.onClick.AddListener(() => OnSpawnButtonPressed(ref mob1SpawnCountEnabled, ref mob1Timer, mob1Progress, mob1SpawnButton, "Mob 1"));
        mob2SpawnButton.onClick.AddListener(() => OnSpawnButtonPressed(ref mob2SpawnCountEnabled, ref mob2Timer, mob2Progress, mob2SpawnButton, "Mob 2"));
        mob3SpawnButton.onClick.AddListener(() => OnSpawnButtonPressed(ref mob3SpawnCountEnabled, ref mob3Timer, mob3Progress, mob3SpawnButton, "Mob 3"));
    }

    void Update()
    {
        TickMob(ref mob1Timer, mob1SpawnTime, mob1Progress, ref mob1SpawnCountEnabled, mob1SpawnButton, "Mob 1");
        TickMob(ref mob2Timer, mob2SpawnTime, mob2Progress, ref mob2SpawnCountEnabled, mob2SpawnButton, "Mob 2");
        TickMob(ref mob3Timer, mob3SpawnTime, mob3Progress, ref mob3SpawnCountEnabled, mob3SpawnButton, "Mob 3");
    }

    private void InitializeSlider(Slider slider, float maxTime)
    {
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = maxTime;
            slider.value = 0f;
        }
    }

    private void TickMob(ref float timer, float maxTime, Slider slider, ref bool spawnCountEnabled, Button spawnButton, string mobName)
    {

        if (!spawnCountEnabled) return;

        timer += Time.deltaTime;

        if (slider != null)
            slider.value = Mathf.Min(timer, maxTime);

        if (timer >= maxTime)
        {
            spawnCountEnabled = false;
            if (spawnButton != null)
                spawnButton.interactable = true;

            Debug.Log($"{mobName} ready to spawn! Button enabled: {spawnButton?.interactable}");
        }
    }

    private void OnSpawnButtonPressed(ref bool spawnCountEnabled, ref float timer, Slider slider, Button spawnButton, string mobName)
    {
        Debug.Log($"{mobName} spawn button pressed!");

        // Reset timer and progress bar
        timer = 0f;
        if (slider != null)
            slider.value = 0f;

        // Resume countdown
        spawnCountEnabled = true;

        // Disable button until next countdown completes
        if (spawnButton != null)
            spawnButton.interactable = false;

        Debug.Log($"{mobName} after button pressed. Timer: {timer}, Enabled: {spawnCountEnabled}, Slider: {slider?.value}, Button: {spawnButton?.interactable}");

        // --- Spawn the minion ---
        if (spawner != null)
        {
            spawner.SpawnMinion();
        }
        else
        {
            Debug.LogError("No MinionSpawner assigned in SpawnWindow!");
        }
    }
}