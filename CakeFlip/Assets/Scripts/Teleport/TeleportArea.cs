using System.Collections;
using Pickup;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportArea : MonoBehaviour
{
    public string SceneNameToTeleportTo;
    [SerializeField] private float teleportDelay;
    [SerializeField] private string defaultScene;

    private void Start()
    {
        defaultScene = GameManager.Instance.ItemToLevelNameTable[PickupableItemType.Undefined];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (string.IsNullOrEmpty(SceneNameToTeleportTo))
            {
                if (SceneManager.GetActiveScene().name == defaultScene)
                {
                    //don't teleport in tutorial / home base
                    return;
                } 
                else
                {
                    SceneNameToTeleportTo = defaultScene;
                }
            } 

            GameManager.Instance.TheDialogueManager.SayNonBlockingDialogue("TELEPORT INITIATED... Hold on to your hat...");
            StartCoroutine(nameof(DoTeleport));
        }
    }

    /// <summary>
    /// Teleport player after delay, aka load a new scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoTeleport()
    {
        yield return new WaitForSeconds(teleportDelay);
        GameManager.Instance.LoadScene(SceneNameToTeleportTo);
    }
}
