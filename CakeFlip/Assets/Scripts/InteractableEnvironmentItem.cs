using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableEnvironmentItem : MonoBehaviour
{
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
        inputActions.Player.Interact.performed += OnPlayerInteraction;
    }
    protected virtual void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnPlayerInteraction;
        inputActions.Player.Disable();
    }


    protected virtual void OnPlayerInteraction(InputAction.CallbackContext context)
    {
        Debug.Log($"Player interacted with {name}. Wow.");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteract = true;
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
