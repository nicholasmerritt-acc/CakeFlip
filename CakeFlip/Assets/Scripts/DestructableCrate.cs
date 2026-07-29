using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("boom");
        Destroy(gameObject);
    }
}
