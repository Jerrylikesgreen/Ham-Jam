using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [Header("Countdown Settings")]
    [Tooltip("Base time BEFORE upgrades (seconds)")]
    public float baseStartTime = 60f;

    public TMP_Text countdownText;
    public Image panelBorder;

    // ── NEW: Inspector debug tracker (optional but useful) ──
    [Header("Runtime Debug (Read Only)")]
    [SerializeField, ReadOnly] private float finalStartTimeDisplay;
    [SerializeField, ReadOnly] private float timeBonusAppliedDisplay;

    private float timer;

    void Start()
    {
        // Apply upgrade bonus
        float bonus = UpgradeManager.GetGameTimeBonus();
        timer = baseStartTime + bonus;

        // Fill debug fields
        finalStartTimeDisplay = timer;
        timeBonusAppliedDisplay = bonus;

        Debug.Log($"Game time set to {timer}s (base {baseStartTime} + {bonus}s from upgrades)");

        UpdateCountdownText();
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer < (baseStartTime + UpgradeManager.GetGameTimeBonus()) * 0.25f)
                panelBorder.color = Color.red;

            if (timer <= 0f)
            {
                timer = 0f;
                OnCountDownFinished();
            }

            UpdateCountdownText();
        }
    }

    private void OnCountDownFinished()
    {
        Debug.Log("Count Down Finished");
        // TODO - Hook up to Event Bus
    }

    private void UpdateCountdownText()
    {
        countdownText.text = Mathf.Ceil(timer).ToString();
    }
}
