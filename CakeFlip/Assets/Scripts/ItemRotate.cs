using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = .4f;

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed);
    }
}
