using UnityEngine;

public class PatrollerSpawner : ItemSpawner
{
    [SerializeField] private Transform[] destinations;

    /// <summary>
    /// Spawn a moving item from our list of moving items.
    /// </summary>
    public override GameObject SpawnOnce()
    {
        GameObject spawnMe = base.SpawnOnce();
        if (spawnMe.TryGetComponent<Patroller>(out Patroller drive))
        {
            drive.SetDestinations(destinations);
            return spawnMe;
        }
        else
        {
            Debug.LogError("You set up your patroller spawner wrong. Make sure it has patrollers in it.");
            return null;
        }
    }
}
