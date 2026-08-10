using UnityEngine;

public class ExplodableObject : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int explosionDamage = 20;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float upwardsModifier = 5f;
    [SerializeField] private bool bigExplosion = false;
    [SerializeField] private bool spawnOnDeath = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //explode when we collide with skateboard
            if (collision.gameObject.GetComponent<PlayerShapeshift>().IsSkateboard)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                if (spawnOnDeath)
                {
                    if (TryGetComponent<ItemSpawner>(out ItemSpawner spawner))
                    {
                        spawner.SpawnOnce();
                    }
                }

                //add explosion force to all colliders around this.
                //we do this after spawning so we have a chance of launching the spawned item into the air
                if (bigExplosion)
                {
                    Vector3 explosionPosition = transform.position;
                    Collider[] hits = Physics.OverlapSphere(explosionPosition, explosionRadius);
                    foreach (Collider collider in hits)
                    {
                        if (collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
                        {
                            rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier);
                        }
                        if (collider.TryGetComponent<Health>(out Health health))
                        {
                            //if big explosion, player takes damage twice. be careful!
                            health.TakeDamage(explosionDamage);
                        }
                    }
                }

                collision.gameObject.GetComponent<Health>().TakeDamage(explosionDamage);
                Destroy(gameObject);
            }
        }
    }
}
