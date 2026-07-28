using UnityEngine;

public class ItemBobUpAndDown : MonoBehaviour
{
    [SerializeField] private float maxY = 1.5f;
    [SerializeField] private float minY = 1f;
    [SerializeField] private float moveSpeed = .3f;

    private void Start()
    {
        Vector3 startPosition = transform.position;
        startPosition.y = (maxY + minY) / 2f;
        transform.position = startPosition;
    }

    void Update()
    {
        if (transform.position.y > maxY || transform.position.y < minY)
        {
            moveSpeed = -moveSpeed;
        }
        transform.position = transform.position + Vector3.up * (moveSpeed * Time.deltaTime);
    }
}
