using System.Collections;
using UnityEngine;

public class SpawnCrate : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject[] cratePrefabs;
    [SerializeField] private int[] dropPercents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(nameof(CrateSpawn));
    }

    private IEnumerator CrateSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            float dropLuck = Random.Range(0, 100);
            GameObject prefabToSpawn;
            switch (dropLuck)
            {
                case < 80:
                    prefabToSpawn = cratePrefabs[0];
                    break;
                default:
                    prefabToSpawn = cratePrefabs[1];
                    break;
            }
            Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        }
    }
}
