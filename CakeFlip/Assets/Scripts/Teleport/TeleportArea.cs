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

    /// <summary>
    /// teleport player after delay. load a new scene and keep track of what we are holding
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoTeleport()
    {
        yield return new WaitForSeconds(teleportDelay);
        SceneManager.LoadScene(SceneNameToTeleportTo);
    }

    //TODO teleport loading bar
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
