using System.Collections;
using UnityEngine;
using Util;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private float initialSpawnDelay;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject[] spawnPrefabs;
    [SerializeField] private int[] weights;
    [SerializeField] private bool continuousSpawning = true;

    void Start()
    {
        if (spawnPosition == null)
        {
            spawnPosition = transform;
        }

        if (continuousSpawning)
        {
            StartCoroutine(nameof(SpawnLoop));
        }
    }

    /// <summary>
    /// Instantiate a prefab, after some initial delay, every spawnInterval seconds at our designated location
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnLoop()
    {
        if (spawnPrefabs == null || spawnPrefabs.Length == 0 || weights == null || weights.Length == 0 || spawnPrefabs.Length != weights.Length)
        {
            Debug.LogWarning("spawnPrefabs and/or weights are not setup correctly. Ensure they are the same nonzero length.");
            yield break;
        }

        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnOnce();
        }
    }

    /// <summary>
    /// Spawn an item from our list of items
    /// </summary>
    public GameObject SpawnOnce()
    {
        GameObject spawnMe = spawnPrefabs.GetWeightedItem(weights);
        if (spawnMe != null)
        {
            spawnMe = Instantiate(spawnMe, spawnPosition.position, spawnMe.transform.rotation);
        }
        return spawnMe;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
