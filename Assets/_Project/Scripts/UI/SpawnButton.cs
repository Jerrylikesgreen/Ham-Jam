using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    [Header("Spawn Window Reference")]
    [SerializeField] private SpawnWindow spawnWindow;

    public void ToggleSpawnWindow()
    {
        if (spawnWindow == null)
        {
            Debug.LogError("SpawnWindow reference not assigned!");
            return;
        }

        bool newState = !spawnWindow.gameObject.activeSelf;
        spawnWindow.gameObject.SetActive(newState);

        Debug.Log($"Spawn Window visibility set to: {newState}");
    }
}
