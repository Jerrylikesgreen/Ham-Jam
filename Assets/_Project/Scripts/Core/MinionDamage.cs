using UnityEngine;
using System.Collections;

public class MinionDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damagePerAttack = 10f;
    public float attackInterval = 2f;

    private CastleHealth castle;
    private bool isAttacking = false;

    void Start()
    {
        castle = FindObjectOfType<CastleHealth>();
        if (castle == null)
        {
            Debug.LogError("MinionDamage: No CastleHealth found in scene!");
        }
    }

    public void StartAttacking()
    {
        if (isAttacking || castle == null) return;

        isAttacking = true;
        Debug.Log($"{gameObject.name} started attacking castle!");
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (isAttacking && castle != null)
        {
            castle.TakeDamage(damagePerAttack);
            yield return new WaitForSeconds(attackInterval);
        }
    }

    // Stop attacking when minion dies (called by Health script's Destroy)
    void OnDestroy()
    {
        isAttacking = false;
        StopAllCoroutines();
    }
}
