using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableEnvironmentItem : MonoBehaviour
{
    [SerializeField] private AudioClip encounterClip;
    [SerializeField] protected bool playClipOnEncounter = true;
    [SerializeField] protected bool skateboardOnly = false;
    [SerializeField] protected bool humanOnly = false;
    protected PlayerController player;

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

    /// <summary>
    /// Test if we are actually close enough to the player to interact.
    /// </summary>
    /// <param name="context"></param>
    private void AttemptPlayerInteraction(InputAction.CallbackContext context)
    {
        if (CanInteract)
        {
            DoPlayerInteraction();
        }
    }

    /// <summary>
    /// Actually perform the interaction with the player, now that we know we are allowed to.
    /// </summary>
    protected virtual void DoPlayerInteraction()
    {
        GameManager.Instance.TheDialogueManager.SayNonBlockingDialogue($"You attempted to talk to {name}. Nothing happened.");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //setup player reference only when we need it
            if (player == null)
            {
                player = other.GetComponent<PlayerController>();
            }

            bool isSkateboard = other.GetComponent<PlayerShapeshift>().IsSkateboard;
            if (skateboardOnly)
            {
                CanInteract = isSkateboard;
            } 
            else if (humanOnly)
            {
                CanInteract = !isSkateboard;
            }
            else
            {
                CanInteract = true;
            }

            if (encounterClip != null && playClipOnEncounter)
            {
                GameManager.Instance.TheAudioManager.PlayOneShot(encounterClip);
            }
            GameManager.Instance.HUD.InteractPromptText.text = CloseEnoughToInteractMessage;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteract = false;
        }
        GameManager.Instance.HUD.InteractPromptText.text = "";
    }

}
