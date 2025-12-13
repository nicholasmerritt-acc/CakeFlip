using UnityEngine;

public class EggPickup : MonoBehaviour
{
    public AudioClip cluck;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            //TODO show the egg count in the UI / do something useful with it
            player.AddPoints(10000);
            GetComponent<AudioSource>().PlayOneShot(cluck);
            Destroy(gameObject);
        }
    }
}
