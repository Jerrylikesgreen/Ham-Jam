using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private const int MAX_UPGRADES = 5;
    private const int COST_PER_UPGRADE = 100;

    // ──────────────────────────────
    // Health Upgrade
    // ──────────────────────────────
    private const string KEY_HEALTH = "MinionHealthUpgrades";
    [SerializeField] private GameObject[] healthUpgradeCircles;

    private int healthLevel = 0;

    // ──────────────────────────────
    // Speed Upgrade
    // ──────────────────────────────
    private const string KEY_SPEED = "MinionSpeedUpgrades";
    [SerializeField] private GameObject[] speedUpgradeCircles;
    [SerializeField, Tooltip("Bonus per level (e.g. 0.10 = +10%)")]
    private float speedBonusPerLevel = 0.10f;

    private int speedLevel = 0;

    // ──────────────────────────────
    // Meat Production Upgrade (flat tick reduction)
    // ──────────────────────────────
    private const string KEY_MEAT = "MeatProductionUpgrades";
    [SerializeField] private GameObject[] meatUpgradeCircles;
    [SerializeField, Tooltip("Seconds reduced from tick interval per upgrade level")]
    private float meatTickReductionPerLevel = 0.1f;

    private int meatLevel = 0;

    // ──────────────────────────────
    // Minion Damage Upgrade
    // ──────────────────────────────
    private const string KEY_DAMAGE = "MinionDamageUpgrades";
    [SerializeField] private GameObject[] damageUpgradeCircles;
    [SerializeField, Tooltip("Damage bonus per level (e.g. 0.10 = +10%)")]
    private float damageBonusPerLevel = 0.10f;

    private int damageLevel = 0;

    // ──────────────────────────────
    // Game Time Upgrade
    // ──────────────────────────────
    private const string KEY_GAMETIME = "GameTimeUpgrades";
    [SerializeField] private GameObject[] gameTimeUpgradeCircles;
    [SerializeField, Tooltip("Seconds added to base game time per upgrade level")]
    private float gameTimeBonusPerLevel = 6f;

    private int gameTimeLevel = 0;

    void Awake()
    {
        healthLevel   = LoadLevel(KEY_HEALTH);
        speedLevel    = LoadLevel(KEY_SPEED);
        meatLevel     = LoadLevel(KEY_MEAT);
        damageLevel   = LoadLevel(KEY_DAMAGE);
        gameTimeLevel = LoadLevel(KEY_GAMETIME);

        UpdateAllVisuals();
    }

    private int LoadLevel(string key) => Mathf.Clamp(PlayerPrefs.GetInt(key, 0), 0, MAX_UPGRADES);

    private void UpdateAllVisuals()
    {
        UpdateCircles(healthUpgradeCircles,   healthLevel);
        UpdateCircles(speedUpgradeCircles,    speedLevel);
        UpdateCircles(meatUpgradeCircles,     meatLevel);
        UpdateCircles(damageUpgradeCircles,   damageLevel);
        UpdateCircles(gameTimeUpgradeCircles, gameTimeLevel);
    }

    private void UpdateCircles(GameObject[] circles, int level)
    {
        for (int i = 0; i < circles.Length; i++)
        {
            if (circles[i] != null)
            {
                circles[i].GetComponent<UnityEngine.UI.Image>().color = 
                    (i < level) ? Color.green : Color.white;
            }
        }
    }

    // ──────────────────────────────
    // Purchase Methods (wire each to its button)
    // ──────────────────────────────

    public void PurchaseHealthUpgrade()
    {
        if (healthLevel >= MAX_UPGRADES) return;
        if (TryPurchase())
        {
            healthLevel++;
            PlayerPrefs.SetInt(KEY_HEALTH, healthLevel);
            PlayerPrefs.Save();
            UpdateCircles(healthUpgradeCircles, healthLevel);
            RefreshGoldDisplay();
        }
    }

    public void PurchaseSpeedUpgrade()
    {
        if (speedLevel >= MAX_UPGRADES) return;
        if (TryPurchase())
        {
            speedLevel++;
            PlayerPrefs.SetInt(KEY_SPEED, speedLevel);
            PlayerPrefs.Save();
            UpdateCircles(speedUpgradeCircles, speedLevel);
            RefreshGoldDisplay();
        }
    }

    public void PurchaseMeatUpgrade()
    {
        if (meatLevel >= MAX_UPGRADES) return;
        if (TryPurchase())
        {
            meatLevel++;
            PlayerPrefs.SetInt(KEY_MEAT, meatLevel);
            PlayerPrefs.Save();
            UpdateCircles(meatUpgradeCircles, meatLevel);
            RefreshGoldDisplay();
        }
    }

    public void PurchaseDamageUpgrade()
    {
        if (damageLevel >= MAX_UPGRADES) return;
        if (TryPurchase())
        {
            damageLevel++;
            PlayerPrefs.SetInt(KEY_DAMAGE, damageLevel);
            PlayerPrefs.Save();
            UpdateCircles(damageUpgradeCircles, damageLevel);
            RefreshGoldDisplay();
        }
    }

    public void PurchaseGameTimeUpgrade()
    {
        if (gameTimeLevel >= MAX_UPGRADES) return;
        if (TryPurchase())
        {
            gameTimeLevel++;
            PlayerPrefs.SetInt(KEY_GAMETIME, gameTimeLevel);
            PlayerPrefs.Save();
            UpdateCircles(gameTimeUpgradeCircles, gameTimeLevel);
            RefreshGoldDisplay();
        }
    }

    private bool TryPurchase()
    {
        int gold = PlayerPrefs.GetInt("TotalGold", 0);
        if (gold >= COST_PER_UPGRADE)
        {
            PlayerPrefs.SetInt("TotalGold", gold - COST_PER_UPGRADE);
            PlayerPrefs.Save();
            return true;
        }

        Debug.Log("Not enough gold for upgrade");
        // TODO: optional "Not enough gold" UI popup
        return false;
    }

    private void RefreshGoldDisplay()
    {
        var display = FindObjectOfType<GoldDisplay>();
        if (display != null)
        {
            display.UpdateGoldDisplay();
        }
    }

    // ── Static getters for runtime scripts ──
    public static int   GetHealthUpgradeLevel()    => PlayerPrefs.GetInt("MinionHealthUpgrades", 0);
    public static int   GetSpeedUpgradeLevel()     => PlayerPrefs.GetInt("MinionSpeedUpgrades", 0);
    public static int   GetMeatUpgradeLevel()      => PlayerPrefs.GetInt("MeatProductionUpgrades", 0);
    public static int   GetDamageUpgradeLevel()    => PlayerPrefs.GetInt("MinionDamageUpgrades", 0);
    public static int   GetGameTimeUpgradeLevel()  => PlayerPrefs.GetInt("GameTimeUpgrades", 0);

    public static float GetSpeedMultiplier()
        => 1f + (GetSpeedUpgradeLevel() * (FindObjectOfType<UpgradeManager>()?.speedBonusPerLevel ?? 0.10f));

    public static float GetDamageMultiplier()
        => 1f + (GetDamageUpgradeLevel() * (FindObjectOfType<UpgradeManager>()?.damageBonusPerLevel ?? 0.10f));

    public static float GetMeatTickReduction()
        => GetMeatUpgradeLevel() * (FindObjectOfType<UpgradeManager>()?.meatTickReductionPerLevel ?? 0.1f);

    public static float GetGameTimeBonus()
        => GetGameTimeUpgradeLevel() * (FindObjectOfType<UpgradeManager>()?.gameTimeBonusPerLevel ?? 6f);
}