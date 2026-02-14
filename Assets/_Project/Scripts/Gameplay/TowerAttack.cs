using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerAttack : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damagePerAttack = 10f;
    [SerializeField] private float attackInterval = 1f;

    [Header("SFX")]
    [Tooltip("AudioSource to play timer tick SFX")]
    public SfxPlayer sfxPlayer;



    private List<MinionHealth> minionsInRange = new List<MinionHealth>();
    private bool isAttacking = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        MinionHealth minion = other.GetComponentInParent<MinionHealth>();

        if (minion == null)
        {
            Debug.Log($"[Tower] {other.name} does not have MinionHealth.");
            return;
        }

        if (!minionsInRange.Contains(minion))
        {
            minionsInRange.Add(minion);
            Debug.Log($"[Tower] Minion added. Count: {minionsInRange.Count}");

            if (!isAttacking)
                StartCoroutine(AttackLoop());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        MinionHealth minion = other.GetComponentInParent<MinionHealth>();

        if (minion != null && minionsInRange.Contains(minion))
        {
            minionsInRange.Remove(minion);
            Debug.Log($"[Tower] Minion removed. Count: {minionsInRange.Count}");
        }
    }

    private IEnumerator AttackLoop()
    {
        isAttacking = true;
        Debug.Log("[Tower] Attack loop started.");

        while (minionsInRange.Count > 0)
        {
            minionsInRange.RemoveAll(m => m == null);

            if (minionsInRange.Count == 0)
                break;

            MinionHealth target = minionsInRange[0];

            Debug.Log($"[Tower] Attacking {target.name}");
            target.TakeDamage(damagePerAttack);
            sfxPlayer.PlayTowerAtk();

            yield return new WaitForSeconds(attackInterval);
        }

        Debug.Log("[Tower] Attack loop stopped.");
        isAttacking = false;
    }
}
