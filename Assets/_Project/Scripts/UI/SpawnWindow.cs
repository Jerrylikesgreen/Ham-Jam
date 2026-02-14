using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // ── NEW: Meat system integration ──
    [Header("Meat Costs & UI")]
    [SerializeField] private MeatResourceGenerator meatGenerator;          // Drag your MeatResourceGenerator here
    [SerializeField] private int mob1MeatCost = 10;
    [SerializeField] private int mob2MeatCost = 25;
    [SerializeField] private int mob3MeatCost = 50;

    [SerializeField] private TMP_Text notEnoughMeatText;                   // Drag your "Not enough meat!" TMP_Text here
    [SerializeField] private float messageDisplayTime = 2f;                // How long to show message

    void Start()
    {
        // Existing init
        InitializeSlider(mob1Progress, spawnSystem.mob1SpawnTime);
        InitializeSlider(mob2Progress, spawnSystem.mob2SpawnTime);
        InitializeSlider(mob3Progress, spawnSystem.mob3SpawnTime);

        mob1SpawnButton.interactable = false;
        mob2SpawnButton.interactable = false;
        mob3SpawnButton.interactable = false;

        // Connect buttons (changed to use cost check)
        mob1SpawnButton.onClick.AddListener(() => TrySpawnMob(1));
        mob2SpawnButton.onClick.AddListener(() => TrySpawnMob(2));
        mob3SpawnButton.onClick.AddListener(() => TrySpawnMob(3));

        // Hide message at start
        if (notEnoughMeatText != null)
            notEnoughMeatText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Existing slider & button ready logic (unchanged)
        mob1Progress.value = spawnSystem.mob1Timer;
        mob2Progress.value = spawnSystem.mob2Timer;
        mob3Progress.value = spawnSystem.mob3Timer;

        mob1SpawnButton.interactable = spawnSystem.mob1Ready;
        mob2SpawnButton.interactable = spawnSystem.mob2Ready;
        mob3SpawnButton.interactable = spawnSystem.mob3Ready;
    }

    // ── NEW: Combined spawn attempt with cost check ──
    private void TrySpawnMob(int mobIndex)
    {
        int cost = GetCostForMob(mobIndex);

        if (meatGenerator == null)
        {
            Debug.LogError("MeatResourceGenerator not assigned in SpawnWindow!");
            return;
        }

        if (meatGenerator.TrySpendMeat(cost))
        {
            // Success → spawn & reset cooldown
            ResetMobTimer(mobIndex);
            spawner.SpawnMinion(mobIndex);  // Your teammate's spawn logic
            Debug.Log($"Spawned mob {mobIndex} for {cost} meat");
        }
        else
        {
            // Fail → show message
            ShowNotEnoughMessage();
        }
    }

    private int GetCostForMob(int mobIndex)
    {
        return mobIndex switch
        {
            1 => mob1MeatCost,
            2 => mob2MeatCost,
            3 => mob3MeatCost,
            _ => 0
        };
    }

    private void ResetMobTimer(int mobIndex)
    {
        // Mirror your original reset calls
        switch (mobIndex)
        {
            case 1: spawnSystem.ResetMob1(); mob1SpawnButton.interactable = false; break;
            case 2: spawnSystem.ResetMob2(); mob2SpawnButton.interactable = false; break;
            case 3: spawnSystem.ResetMob3(); mob3SpawnButton.interactable = false; break;
        }
    }

    private void ShowNotEnoughMessage()
    {
        if (notEnoughMeatText == null) return;

        notEnoughMeatText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideMessage));           // Cancel any previous hide
        Invoke(nameof(HideMessage), messageDisplayTime);
    }

    private void HideMessage()
    {
        if (notEnoughMeatText != null)
            notEnoughMeatText.gameObject.SetActive(false);
    }

    private void InitializeSlider(Slider slider, float maxTime)
    {
        slider.minValue = 0;
        slider.maxValue = maxTime;
        slider.value = 0f;
    }
}