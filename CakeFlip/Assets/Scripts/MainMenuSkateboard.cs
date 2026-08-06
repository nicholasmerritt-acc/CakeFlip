using UnityEngine;
using UnityEngine.AI;

public class MainMenuSkateboard : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float stoppingDistance = .1f;

    [Header("Items")]
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform itemJail;
    [SerializeField] private Transform itemHoldingArea;
    [SerializeField] private int itemIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.SetDestination(endPoint.position);
        agent.stoppingDistance = stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, endPoint.position) < stoppingDistance)
        {
            Debug.Log("resetting");
            transform.position = startPoint.position;
            GetNextItem();
        }
    }

    private void GetNextItem()
    {
        items[itemIndex].transform.position = itemJail.position;
        items[itemIndex + 1].transform.position = itemHoldingArea.position;
        itemIndex++;
    }
}
