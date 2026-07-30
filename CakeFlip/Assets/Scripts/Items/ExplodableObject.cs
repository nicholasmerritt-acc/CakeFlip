using UnityEngine;

public class ExplodableObject : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private bool bigExplosion = false;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float explosionForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //explode when we collide with skateboard
            if (collision.gameObject.GetComponent<PlayerController>().IsSkateboard) {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                if (bigExplosion)
                {
                    Vector3 explosionPosition = transform.position;
                    Collider[] hits = Physics.OverlapSphere(explosionPosition, explosionRadius);
                    foreach (Collider collider in hits)
                    {
                        if (collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
                        {
                            rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}
