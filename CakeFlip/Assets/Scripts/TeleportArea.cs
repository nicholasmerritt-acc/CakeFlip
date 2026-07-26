using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportArea : MonoBehaviour
{
    public string TeleportTo;
    //todo teleport event? that vfx can subscribe to? or just trigger here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //teleport after delay?
            if (string.IsNullOrEmpty(TeleportTo))
            {
                Debug.Log("Nowhere to teleport to! Staying here...");
            } 
            else
            {
                SceneManager.LoadScene(TeleportTo);
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        //add to bar

    //        //teleport

    //        //if string empty, go to lab? or dont teleport
    //        //need item as activator!
    //    }
    //}
}
