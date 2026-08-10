using UnityEngine;

public class MainMenuSkateboard : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDistance = .5f;

    [Header("Items")]
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform itemJail;
    [SerializeField] private Transform itemHoldingArea;
    [SerializeField] private int itemIndex;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        direction = (endPoint.position - transform.position).normalized;
        direction.y = 0f;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, endPoint.position) < stoppingDistance)
        {
            transform.position = startPoint.position;
            HoldNextItem();
        }
        transform.Translate(direction * (moveSpeed * Time.deltaTime), Space.World);
    }

    /// <summary>
    /// Replace the item currently held (if there is one) with an item from the pool.
    /// Instead of destroying and instantiating, we just move them to a holding zone offscreen.
    /// This could probably be an abstract ItemPool class or something in the future.
    /// </summary>
    private void HoldNextItem()
    {
        int nextIndex = (itemIndex + 1) % items.Length;
        items[itemIndex].transform.parent = itemJail;
        items[itemIndex].transform.localPosition = Vector3.zero;
        items[nextIndex].transform.parent = itemHoldingArea;
        items[nextIndex].transform.localPosition = Vector3.zero;
        itemIndex = nextIndex;
    }
}
