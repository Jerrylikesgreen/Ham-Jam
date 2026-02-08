using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject minionPrefab;      // DRAG MeatMinion PREFAB HERE
    public Transform spawnPoint;         // DRAG empty GO or WP0 HERE
    public KeyCode spawnKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            Debug.Log("Space pressed! Attempting spawn...");  // NEW: Confirms input
            SpawnMinion();
        }
    }

    public void SpawnMinion()
    {
        // NEW: Auto-use spawner position if no spawnPoint
        if (spawnPoint == null)
        {
            spawnPoint = transform;  // Spawns at Spawner GO position
            Debug.LogWarning("No Spawn Point! Using Spawner position.");
        }

        if (minionPrefab == null)
        {
            Debug.LogError("❌ NO MINION PREFAB ASSIGNED! Drag MeatMinion.prefab to Inspector.");
            return;
        }

        GameObject newMinion = Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"✅ Spawned {newMinion.name} at {spawnPoint.position}!");  // Success log

        // Assign path
        EnemyMovement movement = newMinion.GetComponent<EnemyMovement>();
        PathManager pathManager = FindObjectOfType<PathManager>();
        if (movement != null && pathManager != null)
        {
            movement.waypoints = pathManager.waypoints;
            Debug.Log("Path assigned to minion.");
        }
        else
        {
            Debug.LogError("❌ No PathManager or EnemyMovement found!");
        }
    }
}
