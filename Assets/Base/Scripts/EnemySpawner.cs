using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectToSpawn;  // The object to spawn
    public float spawnInterval = 10f;  // Time interval between spawns
    public float spawnRadius = 5f;
    public Transform[] spawnLocations;
    bool isSpawning = true;
    private void Start()
    {
        // Start the coroutine to handle the spawning
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (isSpawning)
        {
            Vector3 spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
            GameObject spawn = Instantiate(objectToSpawn,
                spawnLocation,
                transform.rotation, transform);

            // Wait for the specified spawn interval before the next spawn
            yield return new WaitForSeconds(spawnInterval);

            // Decrease the spawn interval over time to emit more objects faster
            spawnInterval = Mathf.Max(1f, spawnInterval * 0.90f);  // Ensuring a minimum interval of 2 seconds
        }
    }
    public void EndGameExplodeUFOs()
    {
        isSpawning = false;
        foreach (EnemyShip enemy in transform.GetComponentsInChildren<EnemyShip>())
        {
            enemy.EndGameExplode();
        }
    }
}



