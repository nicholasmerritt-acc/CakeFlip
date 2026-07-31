using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private float initialSpawnDelay;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject[] spawnPrefabs;
    [SerializeField] private int[] dropPercents;
    private int maxDropPercent = 100;

    void Start()
    {
        StartCoroutine(nameof(SpawnItem));
    }

    /// <summary>
    /// Instantiate a prefab, after some initial delay, every spawnInterval seconds at our designated location
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnItem()
    {
        if (spawnPrefabs == null || spawnPrefabs.Length == 0 || dropPercents == null || dropPercents.Length == 0 || spawnPrefabs.Length != dropPercents.Length)
        {
            Debug.LogWarning("spawnPrefabs and/or dropPercents are not setup correctly. Ensure they are the same nonzero length.");
            yield break;
        }

        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            float currentDropChance = Random.Range(0, maxDropPercent);

            GameObject prefabToSpawn = spawnPrefabs[0];
            int sumTotal = 0;
            for (int i = 0; i < spawnPrefabs.Length; i++ )
            {
                int dropRate = dropPercents[i];
                sumTotal += dropRate;
                if (currentDropChance <= sumTotal)
                {
                    prefabToSpawn = spawnPrefabs[i];
                    Debug.Log($"Spawned {prefabToSpawn} with a {sumTotal} % drop rate");
                    break;
                }
            }
            if (prefabToSpawn == null)
            {
                Debug.LogWarning($"dropPercents math is off. should sum to {maxDropPercent}");
                prefabToSpawn = spawnPrefabs[0];
            }
            Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        }
    }
}
