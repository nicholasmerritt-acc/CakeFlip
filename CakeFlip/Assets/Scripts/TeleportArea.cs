using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportArea : MonoBehaviour
{
    public string SceneNameToTeleportTo;
    [SerializeField] private float teleportDelay;
    //todo teleport event? that vfx can subscribe to? or just trigger here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //teleport after delay?
            if (string.IsNullOrEmpty(SceneNameToTeleportTo))
            {
                Debug.Log("Nowhere to teleport to! Staying here...");
            } 
            else
            {
                Debug.Log("Begin Teleport... hold on to your hat...");
                StartCoroutine(nameof(DoTeleport));
            }
        }
    }

    private IEnumerator DoTeleport()
    {
        GameManager.Instance.SaveInventory();
        yield return new WaitForSeconds(teleportDelay);
        SceneManager.LoadScene(SceneNameToTeleportTo);
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
