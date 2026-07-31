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
        if (spawnPrefabs == null || spawnPrefabs.Length == 0 || weights == null || weights.Length == 0 || spawnPrefabs.Length != weights.Length)
        {
            Debug.LogWarning("spawnPrefabs and/or weights are not setup correctly. Ensure they are the same nonzero length.");
            yield break;
        }

        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Instantiate(spawnPrefabs.GetWeightedItem(weights), spawnPosition.position, Quaternion.identity);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
