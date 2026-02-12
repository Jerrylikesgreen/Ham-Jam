using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text goldText;           // Drag your top-right TMP_Text here

    [Header("Optional Formatting")]
    [SerializeField] private string prefix = "Gold: ";    // e.g. "Gold: 420" or just "420"
    [SerializeField] private bool updateEveryFrame = false; // Usually false is enough

    void Start()
    {
        UpdateGoldDisplay();
    }

    // Call this whenever gold changes (or every frame if you want)
    public void UpdateGoldDisplay()
    {
        if (goldText == null)
        {
            Debug.LogWarning("Gold Text reference missing on GoldDisplay!");
            return;
        }

        int currentGold = PlayerPrefs.GetInt("TotalGold", 0);
        goldText.text = prefix + currentGold.ToString();
    }

    // Optional: if you want live updates (e.g. after purchase)
    void Update()
    {
        if (updateEveryFrame)
        {
            UpdateGoldDisplay();
        }
    }
}
