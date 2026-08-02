using UnityEngine;

public class ItemBobUpAndDown : MonoBehaviour
{
    [SerializeField] private float maxY = .5f;
    private float maxYTotal;
    [SerializeField] private float minY = .5f;
    private float minYTotal;
    [SerializeField] private float moveSpeed = .3f;

    //we want to do this setting in Awake, because sometimes items are disabled until we want them to appear in the scene
    //e.g. when they are "in inventory" or being swapped onto a pedestal
    private void Awake()
    {
        maxYTotal = transform.position.y + maxY;
        minYTotal = transform.position.y - minY;
    }

    void Update()
    {
        if ((moveSpeed > 0 && transform.position.y > maxYTotal) || (moveSpeed < 0 && transform.position.y < minYTotal))
        {
            moveSpeed = -moveSpeed;
        }
        transform.position = transform.position + Vector3.up * (moveSpeed * Time.deltaTime);
    }
}
