using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Util;

[RequireComponent(typeof(NavMeshAgent))]
public class Patroller : MonoBehaviour
{
    [SerializeField] private float minSpeed = .01f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<Transform> destinations;
    [SerializeField] private float destinationCheckInterval = 1.0f;
    [SerializeField] private float stoppingDistance = 1.0f;
    [SerializeField] private int destinationIndex;




    private void OnDrawGizmosSelected()
    {
        DrawDestinationPath();
    }

    private void DrawDestinationPath()
    {
        if (destinations.Count < 2)
        {
            return;
        } 
        else
        {
            Gizmos.color = Color.darkViolet;

            Vector3[] destinationPairs = new Vector3[destinations.Count * 2];
            for (int i = 0; i < destinations.Count; i++)
            {
                destinationPairs[i * 2] = destinations[i].transform.position;
                destinationPairs[(i * 2) + 1] = destinations[(i + 1) % destinations.Count].transform.position;
            }

            Gizmos.DrawLineList(destinationPairs);
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        SetNextDestination();

        StartCoroutine(nameof(CheckForNewDestination));
    }

    private IEnumerator CheckForNewDestination()
    {
        while (true)
        {
            yield return new WaitForSeconds(destinationCheckInterval);
            SetNextDestination();
        }
    }

    private void SetNextDestination()
    {
        if (destinations == null || destinations.Count == 0)
        {
            return;
        }
        //change destinations if:
        else if (agent.destination == null || //1. we don't have one
            (agent.remainingDistance <= agent.stoppingDistance && //2. OR we are close enough to the destination
            (!agent.hasPath || agent.velocity.sqrMagnitude < minSpeed))) //3. AND we have stopped
        {
            agent.TrySetDestination(destinations[destinationIndex].position);
            Debug.Log($"{name} set destination {destinationIndex}: {agent.destination}");
            destinationIndex = (destinationIndex + 1) % destinations.Count;
        }
    }

    public void SetDestinations(Transform[] newDestinations)
    {
        destinations = new();
        AddDestinations(newDestinations);
    }

    public void AddDestinations(Transform[] newDestinations)
    {
        destinations.AddRange(newDestinations);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
