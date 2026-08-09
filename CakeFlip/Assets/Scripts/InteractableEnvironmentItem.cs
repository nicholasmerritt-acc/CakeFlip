using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableEnvironmentItem : MonoBehaviour
{
    [SerializeField] private AudioClip encounterClip;
    [SerializeField] protected bool playClipOnEncounter = true;

    /// <summary>
    /// The player is near enough to press the interact button on this object. Show them this message.
    /// </summary>
    protected string closeEnoughToInteractMessage = "Press F to interact";

    /// <summary>
    /// The player is near enough to press the interact button on this object. Show them this message.
    /// </summary>
    protected virtual string CloseEnoughToInteractMessage
    {
        get => closeEnoughToInteractMessage;
        set => closeEnoughToInteractMessage = value;
    }
    public bool CanInteract = false;

    private InputSystem_Actions inputActions;


    protected virtual void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    protected virtual void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += AttemptPlayerInteraction;
    }
    protected virtual void OnDisable()
    {
        inputActions.Player.Interact.performed -= AttemptPlayerInteraction;
        inputActions.Player.Disable();
    }


    private void AttemptPlayerInteraction(InputAction.CallbackContext context)
    {
        if (CanInteract)
        {
            DoPlayerInteraction();
        }
    }

    protected virtual void DoPlayerInteraction()
    {
        Debug.Log($"You attempted to talk to {name}. Nothing happened.");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteract = true;
            if (encounterClip != null && playClipOnEncounter)
            {
                GameManager.Instance.TheAudioManager.PlayOneShot(encounterClip);
            }
            Debug.Log(CloseEnoughToInteractMessage);
            //TODO show UI thing
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteract = false;
        }
        //TODO hide UI thing
    }

}
