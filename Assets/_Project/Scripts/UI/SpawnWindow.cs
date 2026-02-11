using UnityEngine;
using UnityEngine.UI;

public class SpawnWindow : MonoBehaviour
{
    [Header("System")]
    [SerializeField] private SpawnSystem spawnSystem;

    [Header("Mob Progress Bars")]
    [SerializeField] private Slider mob1Progress;
    [SerializeField] private Slider mob2Progress;
    [SerializeField] private Slider mob3Progress;

    [Header("Mob Spawn Buttons")]
    [SerializeField] private Button mob1SpawnButton;
    [SerializeField] private Button mob2SpawnButton;
    [SerializeField] private Button mob3SpawnButton;

    [Header("Spawner")]
    [SerializeField] private MinionSpawner spawner;

    void Start()
    {
        // Initialize sliders
        InitializeSlider(mob1Progress, spawnSystem.mob1SpawnTime);
        InitializeSlider(mob2Progress, spawnSystem.mob2SpawnTime);
        InitializeSlider(mob3Progress, spawnSystem.mob3SpawnTime);

        // Disable buttons at start
        mob1SpawnButton.interactable = false;
        mob2SpawnButton.interactable = false;
        mob3SpawnButton.interactable = false;

        // Connect buttons
        mob1SpawnButton.onClick.AddListener(() => SpawnMob1());
        mob2SpawnButton.onClick.AddListener(() => SpawnMob2());
        mob3SpawnButton.onClick.AddListener(() => SpawnMob3());
    }

    void Update()
    {
        // Update sliders from system timers
        mob1Progress.value = spawnSystem.mob1Timer;
        mob2Progress.value = spawnSystem.mob2Timer;
        mob3Progress.value = spawnSystem.mob3Timer;

        // Enable buttons when ready
        mob1SpawnButton.interactable = spawnSystem.mob1Ready;
        mob2SpawnButton.interactable = spawnSystem.mob2Ready;
        mob3SpawnButton.interactable = spawnSystem.mob3Ready;
    }

    void SpawnMob1()
    {
        spawnSystem.ResetMob1();
        mob1SpawnButton.interactable = false;
        spawner.SpawnMinion();
    }

    void SpawnMob2()
    {
        spawnSystem.ResetMob2();
        mob2SpawnButton.interactable = false;
        spawner.SpawnMinion();
    }

    void SpawnMob3()
    {
        spawnSystem.ResetMob3();
        mob3SpawnButton.interactable = false;
        spawner.SpawnMinion();
    }

    private void InitializeSlider(Slider slider, float maxTime)
    {
        slider.minValue = 0;
        slider.maxValue = maxTime;
        slider.value = 0f;
    }
}
