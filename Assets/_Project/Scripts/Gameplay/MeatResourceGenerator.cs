using UnityEngine;
using TMPro;

public class MeatResourceGenerator : MonoBehaviour
{
    [Header("Meat Settings")]
    [SerializeField, Tooltip("Base interval BEFORE upgrades (seconds)")]
    private float baseTickInterval = 1f;

    [SerializeField, Tooltip("Base meat per tick BEFORE upgrades")]
    private int baseMeatPerTick = 1;

    [Header("UI")]
    [SerializeField] private TMP_Text meatText;

    // ── NEW: Inspector debug fields ──
    [Header("Runtime Debug (Read Only)")]
    [SerializeField, ReadOnly] private float finalTickIntervalDisplay;
    [SerializeField, ReadOnly] private int   finalMeatPerTickDisplay;
    [SerializeField, ReadOnly] private float reductionAppliedDisplay;

    private int meatCount = 0;
    private float timer = 0f;

    // Final values after upgrade
    private float tickInterval;
    private int   meatPerTick;

    void Start()
    {
        ApplyUpgrades();
        UpdateDisplay();
    }

    private void ApplyUpgrades()
    {
        float reduction = UpgradeManager.GetMeatTickReduction();

        tickInterval = baseTickInterval - reduction;
        tickInterval = Mathf.Max(tickInterval, 0.1f); // prevent negative / zero tick rate

        meatPerTick = baseMeatPerTick; // unchanged for now – can add bonus later if wanted

        // Fill debug fields
        finalTickIntervalDisplay   = tickInterval;
        finalMeatPerTickDisplay    = meatPerTick;
        reductionAppliedDisplay    = reduction;

        Debug.Log($"Meat production upgraded | Tick: {tickInterval}s (reduced by {reduction}s) | Per tick: {meatPerTick}");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tickInterval)
        {
            timer -= tickInterval;
            AddMeat(meatPerTick);
        }
    }

    private void AddMeat(int amount)
    {
        meatCount += amount;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (meatText != null)
            meatText.text = meatCount.ToString();
    }

    public int GetMeatCount() => meatCount;

    public bool TrySpendMeat(int amount)
    {
        if (meatCount >= amount)
        {
            meatCount -= amount;
            UpdateDisplay();
            return true;
        }
        return false;
    }
}