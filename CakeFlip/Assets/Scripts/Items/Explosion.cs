using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float selfDestructTimer = 5f;
    [SerializeField] private ParticleSystem explosion;

    void Start()
    {
        if (explosion == null)
        {
            explosion = GetComponent<ParticleSystem>();
        }
        explosion.Play();
        Destroy(gameObject, selfDestructTimer);
    }
}
