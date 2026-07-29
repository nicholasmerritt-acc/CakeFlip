using UnityEngine;

public class ItemBobUpAndDown : MonoBehaviour
{
    [SerializeField] private float maxY = .5f;
    private float maxYTotal;
    [SerializeField] private float minY = .5f;
    private float minYTotal;
    [SerializeField] private float moveSpeed = .3f;

    private void Start()
    {
        maxYTotal = transform.position.y + maxY;
        minYTotal = transform.position.y - minY;
    }

    void Update()
    {
        if (transform.position.y > maxYTotal || transform.position.y < minYTotal)
        {
            moveSpeed = -moveSpeed;
        }
        transform.position = transform.position + Vector3.up * (moveSpeed * Time.deltaTime);
    }
}
