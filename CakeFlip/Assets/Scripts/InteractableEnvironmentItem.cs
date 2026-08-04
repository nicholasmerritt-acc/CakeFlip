using UnityEngine;

public abstract class InteractableEnvironmentItem : MonoBehaviour
{
    /// <summary>
    /// The player is near enough to press the interact button on this object
    /// </summary>
    public abstract void PlayerNearby();

    /// <summary>
    /// Message shown to the player when they are near enough to interact
    /// </summary>
    public abstract string InteractMessage { get; set; }

    public bool CanInteract = false;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteract = true;
            Debug.Log(InteractMessage);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteract = false;
            Debug.Log($"{name} deactivated");
        }
        //TODO hide UI thing
    }
}
