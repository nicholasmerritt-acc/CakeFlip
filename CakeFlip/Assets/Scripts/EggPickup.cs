using UnityEngine;

public class EggPickup : MonoBehaviour
{
    public AudioClip cluck;
    //private PlayerControllerOpen player;

    private void Start()
    {
        //PlayerControllerOpen player = FindAnyObjectByType<PlayerControllerOpen>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<AudioSource>().PlayOneShot(cluck);
            Destroy(gameObject);
        }
    }
}
