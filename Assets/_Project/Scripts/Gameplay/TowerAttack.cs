using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerAttack : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damagePerAttack = 10f;
    [SerializeField] private float attackInterval = 1f;

    [Header("SFX")]
    [Tooltip("Audio player for tower attack SFX")]
    public SfxPlayer sfxPlayer;

    [Header("Targeting Line (Visible in Game)")]
    [SerializeField] private Color lineColor = Color.red;
    [SerializeField, Tooltip("Width of the dotted line")]
    private float lineWidth = 0.1f;
    [SerializeField, Tooltip("Length of each dash segment")]
    private float dashLength = 0.3f;
    [SerializeField, Tooltip("Gap between dashes")]
    private float dashGap = 0.2f;

    private List<MinionHealth> minionsInRange = new List<MinionHealth>();
    private bool isAttacking = false;
    private MinionHealth currentTarget;

    // LineRenderer for the dotted line
    private LineRenderer lineRenderer;

    private void Awake()
    {
        // Create LineRenderer at runtime (one per tower)
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        SetupLineRenderer();
    }

    private void SetupLineRenderer()
{
    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

    // NEW: Match sorting layer/order to towers
    SpriteRenderer towerSprite = GetComponent<SpriteRenderer>();
    if (towerSprite != null)
    {
        lineRenderer.sortingLayerName = towerSprite.sortingLayerName;      // e.g. "Towers"
        lineRenderer.sortingOrder = towerSprite.sortingOrder + 1;          // +1 so line draws on top of tower
    }
    else
    {
        // Fallback if no SpriteRenderer on tower
        lineRenderer.sortingLayerName = "Default";
        lineRenderer.sortingOrder = 10;  // Adjust higher if needed
    }

    lineRenderer.startColor = lineColor;
    lineRenderer.endColor = lineColor;
    lineRenderer.startWidth = lineWidth;
    lineRenderer.endWidth = lineWidth;
    lineRenderer.useWorldSpace = true;
    lineRenderer.positionCount = 0;
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        MinionHealth minion = other.GetComponentInParent<MinionHealth>();
        if (minion == null) return;

        if (!minionsInRange.Contains(minion))
        {
            minionsInRange.Add(minion);

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

            if (currentTarget == minion)
                currentTarget = null;
        }
    }

    private IEnumerator AttackLoop()
    {
        isAttacking = true;

        while (minionsInRange.Count > 0)
        {
            minionsInRange.RemoveAll(m => m == null);

            if (minionsInRange.Count == 0) break;

            // Pick target (first in range - change to closest/lowest HP if wanted)
            currentTarget = minionsInRange[0];

            currentTarget.TakeDamage(damagePerAttack);
            sfxPlayer?.PlayTowerAtk();

            yield return new WaitForSeconds(attackInterval);
        }

        isAttacking = false;
        currentTarget = null;
        HideLine();
    }

    private void LateUpdate()
    {
        // Update the dotted line every frame while attacking
        if (isAttacking && currentTarget != null && currentTarget.transform != null)
        {
            DrawDottedLine(transform.position, currentTarget.transform.position);
        }
        else
        {
            HideLine();
        }
    }

    private void DrawDottedLine(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        if (distance < 0.1f)
        {
            HideLine();
            return;
        }

        // Calculate how many dashes fit
        float dashGapTotal = dashLength + dashGap;
        int segmentCount = Mathf.FloorToInt(distance / dashGapTotal) * 2 + 2;

        lineRenderer.positionCount = segmentCount;
        float step = distance / (segmentCount - 1);

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i * step;
            Vector3 point = Vector3.Lerp(start, end, t / distance);

            // Only place points for dash segments (skip gaps)
            if ((i % 2) == 0)  // even indices = dash start/end
            {
                lineRenderer.SetPosition(i, point);
            }
            else
            {
                // For gaps, repeat last dash point to "skip"
                lineRenderer.SetPosition(i, lineRenderer.GetPosition(i - 1));
            }
        }
    }

    private void HideLine()
    {
        lineRenderer.positionCount = 0;
    }

    void OnDestroy()
    {
        if (lineRenderer != null)
            Destroy(lineRenderer);
    }
}