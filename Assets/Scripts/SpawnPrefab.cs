using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // Assign this in the Inspector
    public Transform spawnPoint; // Assign this in the Inspector
    public float spawnDelay = 3f; // How often to spawn

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, spawnDelay);
    }

    void SpawnObject()
    {
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, spawnPoint.localPosition, Quaternion.identity);
        }
    }
}
