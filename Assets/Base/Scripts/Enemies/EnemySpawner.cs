using UnityEngine;
using System.Collections;

namespace heyjoelang
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private GameObject objectToSpawn;
        private float spawnInterval = 3f;
        private float spawnRadius = 10f;
        private bool isSpawning = true;
        private void Start()
        {
            StartCoroutine(SpawnObjects());
        }

        private IEnumerator SpawnObjects()
        {
            while (isSpawning)
            {
                float randomAngle = Random.Range(0f, 2.0f * Mathf.PI);
                Vector3 spawnLocation = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * spawnRadius;
                GameObject spawn = Instantiate(objectToSpawn,
                    spawnLocation,
                    transform.rotation, transform);
                yield return new WaitForSeconds(spawnInterval);
                spawnInterval = Mathf.Max(1f, spawnInterval * 0.90f);
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
}



