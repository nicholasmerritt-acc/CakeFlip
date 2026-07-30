using UnityEngine;

public class ExplodableObject : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //explode when we collide with skateboard
            if (collision.gameObject.GetComponent<PlayerController>().IsSkateboard) {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
