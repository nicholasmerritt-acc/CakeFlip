using UnityEngine;
using UnityEngine.InputSystem;
using Trick;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private InputSystem_Actions.PlayerActions playerActions;

    [Header("Tricks")]
    [SerializeField] private bool trickInProgress = false;
    [SerializeField] private int failedTrickDamage = 1;

    [Header("Interacting")]
    [SerializeField] private InteractableEnvironmentItem nearbyItem;

    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private PlayerShapeshift shapeshifter;

    [Header("Positioning")]
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private int yBoundary = -30;
    [SerializeField] private float boundaryCheckInterval = 1f;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerActions = inputActions.Player;
    }

    void Start()
    {
        SetupReferences();
        SetPlayer();
        StartCoroutine(nameof(CheckBoundary));
    }

    private IEnumerator CheckBoundary()
    {
        while (true)
        {
            yield return new WaitForSeconds(boundaryCheckInterval);
            if (transform.position.y < yBoundary)
            {
                Respawn();
            }
        }
    }

    private void SetPlayer()
    {
        GameManager.Instance.SetPlayer(this);
    }

    /// <summary>
    /// Get references only if not setup in inspector.
    /// </summary>
    private void SetupReferences()
    {
        respawnPosition = transform.position;

        if (myRigidbody == null)
        {
            myRigidbody = GetComponent<Rigidbody>();
        }
        if (health == null)
        {
            health = GetComponent<Health>();
        }
        if (movement == null)
        {
            movement = GetComponent<PlayerMovement>();
        }
        if (shapeshifter == null)
        {
            shapeshifter = GetComponent<PlayerShapeshift>();
        }
    }

    private void OnEnable()
    {
        playerActions.Enable();
        playerActions.Reset.performed += ResetPlayer;

        playerActions.Trick1.performed += DoTrick1;
        playerActions.Trick2.performed += DoTrick2;
        playerActions.Trick3.performed += DoTrick3;

        Skateboard.TrickCompleted += TrickCompleted;
    }

    private void OnDisable()
    {
        playerActions.Reset.performed -= ResetPlayer;

        playerActions.Trick1.performed -= DoTrick1;
        playerActions.Trick2.performed -= DoTrick2;
        playerActions.Trick3.performed -= DoTrick3;
        playerActions.Disable();

        Skateboard.TrickCompleted -= TrickCompleted;

        StopAllCoroutines();
    }

    private void DoTrick1(InputAction.CallbackContext context)
    {
        DoTrick(TrickType.Frontflip);
    }
    private void DoTrick2(InputAction.CallbackContext context)
    {
        DoTrick(TrickType.Backflip);
    }
    private void DoTrick3(InputAction.CallbackContext context)
    {
        DoTrick(TrickType.Sideflip);
    }

    /// <summary>
    /// Perform a skateboard trick, which sets an animation and can earn the player points
    /// </summary>
    /// <param name="whichFlip"></param>
    private void DoTrick(TrickType whichFlip)
    {
        if (trickInProgress)
        {
            return;
        }

        bool unlocked = GameManager.Instance.Unlocks.Contains(whichFlip);

        //only attempt trick if we are an airborne skateboard, and if we've unlocked it.
        bool grounded = movement.Grounded();
        if (!shapeshifter.IsSkateboard || grounded)
        {
            return;
        }
        if (!unlocked)
        {
            GameManager.Instance.TheDialogueManager.SayNonBlockingDialogue("You haven't learned that trick yet!");
            return;
        }

        trickInProgress = true;

        SkateboardTrick whichTrick = GameManager.Instance.SkateboardTrickDictionary[whichFlip];
        shapeshifter.SetAnimationTrigger(whichTrick.AnimationTrigger);
    }

    /// <summary>
    /// Set the player back at the level start point, because they asked to or they need some help
    /// </summary>
    private void ResetPlayer(InputAction.CallbackContext value)
    {
        Respawn();
    }

    /// <summary>
    /// Set player back to initial position and stop all movement
    /// </summary>
    private void Respawn()
    {
        transform.SetPositionAndRotation(respawnPosition, Quaternion.identity);
        myRigidbody.linearVelocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
    }

    public void AddPoints(int points)
    {
        GameManager.Instance.TheDialogueManager.SayNonBlockingDialogue($"{points} points! nice!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (trickInProgress && shapeshifter.IsSkateboard)
        {
            //if we hit an IJumpable, fail the trick
            if (collision.gameObject.TryGetComponent<IJumpable>(out IJumpable jumpable))
            {
                trickInProgress = false;
                shapeshifter.SetAnimationTrigger("trickCanceled");
                health.TakeDamage(failedTrickDamage);
                GameManager.Instance.TheDialogueManager.SayNonBlockingDialogue($"Trick failed! Oof!");
            }
        }
    }

    /// <summary>
    /// announce that we have compeleted a skateboard trick
    /// </summary>
    /// <param name="whichTrick"></param>
    private void TrickCompleted(TrickType whichTrick)
    {
        trickInProgress = false;
        AddPoints(GameManager.Instance.SkateboardTrickDictionary[whichTrick].Points);
        GameManager.Instance.TheAudioManager.PlayAudienceClip();
    }
}
