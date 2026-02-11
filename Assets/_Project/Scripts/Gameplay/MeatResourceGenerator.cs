using UnityEngine;
using TMPro;

public class MeatResourceGenerator : MonoBehaviour
{
    [Header("Meat Settings")]
    [SerializeField] private float tickInterval = 1f;   // Time between ticks
    [SerializeField] private int meatPerTick = 1;       // How much per tick

    [Header("UI")]
    [SerializeField] private TMP_Text meatText;

    private int meatCount = 0;
    private float timer = 0f;

    void Start()
    {
        UpdateDisplay();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tickInterval)
        {
            timer -= tickInterval; // keeps overflow smooth
            AddMeat(meatPerTick);
        }
    }

    private void AddMeat(int amount)
    {
        meatCount += amount;
        UpdateDisplay();
        Debug.Log($"Meat ticked. Current meat: {meatCount}");
    }

    private void UpdateDisplay()
    {
        if (meatText != null)
            meatText.text = meatCount.ToString();
    }

    public int GetMeatCount()
    {
        return meatCount;
    }

    public bool TrySpendMeat(int amount)
    {
        if (meatCount >= amount)
        {
            meatCount -= amount;
            UpdateDisplay();
            Debug.Log($"Spent {amount}. Remaining: {meatCount}");
            return true;
        }

        Debug.Log("Not enough meat.");
        return false;
    }
}
