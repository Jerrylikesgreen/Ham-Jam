using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [Header("Countdown Settings")]
    [Tooltip("Base time BEFORE upgrades (seconds)")]
    public float baseStartTime = 60f;

    [Header("UI")]
    public TMP_Text countdownText;
    [Tooltip("Panel that will turn red during Rush")]
    public Image rushPanelBorder;
    
    [Header("Music")]
    [Tooltip("Reference to the MusicPlayer to switch tracks during Rush")]
    public MusicPlayer musicPlayer;

 

    [Header("LevelLost Ref")]
    public GameObject levelLost;

    [Header("SFX")]
    [Tooltip("AudioSource to play timer tick SFX")]
    public AudioSource sfxPlayer;


    [Tooltip("Timer tick clip (used for Rush alert)")]
    public AudioClip timerTickClip;
    public AudioClip gameLostTrack;

    [Header("Runtime Debug (Read Only)")]
    [SerializeField, ReadOnly] private float finalStartTimeDisplay;
    [SerializeField, ReadOnly] private float timeBonusAppliedDisplay;

    private float timer;
    private bool rushTriggered = false;

    void Start()
    {
        float bonus = UpgradeManager.GetGameTimeBonus();
        timer = baseStartTime + bonus;

        finalStartTimeDisplay = timer;
        timeBonusAppliedDisplay = bonus;

        Debug.Log($"Game time set to {timer}s (base {baseStartTime} + {bonus}s from upgrades)");

        UpdateCountdownText();
    }

    void Update()
    {
        if (timer <= 0f) return;

        timer -= Time.deltaTime;

        // Trigger Rush if below 35% remaining
        if (!rushTriggered && timer <= (finalStartTimeDisplay * 0.35f))
        {
            Rush();
            rushTriggered = true;
        }

        // Turn panel red in Rush
        if (rushTriggered && rushPanelBorder != null)
        {
            rushPanelBorder.color = Color.red;
        }

        if (timer <= 0f)
        {
            timer = 0f;
            OnCountDownFinished();
        }

        UpdateCountdownText();
    }

    private void Rush()
    {
        Debug.Log("Rush Triggered! Less than 35% time remaining.");

        // Smoothly switch to Rush music
        if (musicPlayer != null)
        {
            musicPlayer.PlayRushTrack();
        }

        // Play timer tick SFX once
        if (sfxPlayer != null && timerTickClip != null)
        {
            sfxPlayer.PlayOneShot(timerTickClip);
            Debug.Log("✅ Rush SFX played");
        }
    }

    private void OnCountDownFinished()
    {
        Debug.Log("Count Down Finished");
        levelLost.SetActive(true);
        musicPlayer.PlayTrack(gameLostTrack);
    }

    private void UpdateCountdownText()
    {
        if (countdownText != null)
            countdownText.text = Mathf.Ceil(timer).ToString();
    }
}
