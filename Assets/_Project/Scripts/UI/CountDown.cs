using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for TextMeshPro

public class CountDown : MonoBehaviour
{
    [Header("Countdown Settings")]
    public TMP_Text countdownText;
    public Image panelBorder;
    public float startTime = 10f;  // Countdown start time in seconds

    private float timer;

    void Start()
    {
        // Initialize timer
        timer = startTime;

        // Optional: immediately update the text at start
        UpdateCountdownText();
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            // Optional: change panel color at 25% remaining
            if (timer < startTime * 0.25f)
                panelBorder.color = Color.red;

            // Clamp timer to 0
            if (timer <= 0f)
            {
                timer = 0f;
                OnCountDownFinished(); // ✅ Call only once when timer hits 0
            }

            UpdateCountdownText();
        }
    }


    private void OnCountDownFinished()
    {
        Debug.Log("Count Down Finished");
        // TODO - Hook up to Event Bus - Still need to create the Event bus. 
    }


    private void UpdateCountdownText()
    {
        // Display as whole numbers
        countdownText.text = Mathf.Ceil(timer).ToString();
    }
}
