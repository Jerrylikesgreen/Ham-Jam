using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject minion1;
    public GameObject minion2;
    public GameObject minion3;
    public Transform spawnPoint;

    /// <summary>
    /// Spawns a minion based on index:
    /// 1 → minion1
    /// 2 → minion2
    /// 3 → minion3
    /// </summary>
    public void SpawnMinion(int index)
    {
        // Auto-use spawner position if no spawnPoint
        if (spawnPoint == null)
        {
            spawnPoint = transform;
            Debug.LogWarning("No Spawn Point! Using Spawner position.");
        }

        GameObject prefabToSpawn = null;

        switch (index)
        {
            case 1:
                prefabToSpawn = minion1;
                break;
            case 2:
                prefabToSpawn = minion2;
                break;
            case 3:
                prefabToSpawn = minion3;
                break;
            default:
                Debug.LogError($" Invalid minion index: {index}. Must be 1, 2, or 3.");
                return;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError($" Prefab for minion index {index} is not assigned!");
            return;
        }

        // Instantiate the minion
        GameObject newMinion = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        Debug.Log($" Spawned {newMinion.name} at {spawnPoint.position} (index {index})");

        // Assign path if available
        EnemyMovement movement = newMinion.GetComponent<EnemyMovement>();
        PathManager pathManager = FindObjectOfType<PathManager>();
        if (movement != null && pathManager != null)
        {
            movement.waypoints = pathManager.waypoints;
            Debug.Log(" Path assigned to minion.");
        }
        else
        {
            Debug.LogError(" No PathManager or EnemyMovement found!");
        }
    }
}
