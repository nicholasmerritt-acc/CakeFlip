using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CarDrive : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private List<Transform> destinations;
    private float destinationCheckInterval = 1.0f;
    [SerializeField] private int destinationIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (destinations == null || destinations.Count == 0)
        {
            Debug.LogWarning($"no destinations found for {name}");
        } 
        else
        {
            agent.destination = destinations[destinationIndex].position;
        }

        StartCoroutine(nameof(CheckForNewDestination));
    }

    private IEnumerator CheckForNewDestination()
    {
        while (true)
        {
            yield return new WaitForSeconds(destinationCheckInterval);
            if (destinations == null || destinations.Count == 0)
            {
                Debug.LogWarning($"no destinations found for {name}");
            }
            else if (agent.destination == null || !agent.hasPath || agent.remainingDistance < .1f)
            {
                destinationIndex = (destinationIndex + 1) % destinations.Count;
                agent.destination = destinations[destinationIndex].position;
                Debug.Log($"{name} switching to destination {destinationIndex}: {agent.destination}");
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
