using UnityEngine;
using UnityEngine.InputSystem;
using Trick;

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

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerActions = inputActions.Player;
    }

    void Start()
    {
        SetupReferences();
        SetPlayer();
    }

    private void SetPlayer()
    {
        GameManager.Instance.SetPlayer(this);
    }

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

    private void DoTrick(TrickType whichFlip)
    {
        if (trickInProgress)
        {
            Debug.Log("already doing a trick!");
            return;
        }

        bool unlocked = GameManager.Instance.UnlockedTricks.Contains(whichFlip);

        //only attempt trick if we are an airborne skateboard, and if we've unlocked it.
        bool grounded = movement.Grounded();
        if (!shapeshifter.IsSkateboard || !unlocked || grounded)
        {
            if (!unlocked)
            {
                Debug.Log("you haven't learned that trick yet!");
            }
            return;
        }

        trickInProgress = true;

        SkateboardTrick whichTrick = GameManager.Instance.SkateboardTrickDictionary[whichFlip];
        shapeshifter.SetAnimationTrigger(whichTrick.AnimationTrigger);
    }

    private void ResetPlayer(InputAction.CallbackContext value)
    {
        Respawn();
    }

    void Update()
    {

        if (transform.position.y < yBoundary)
        {
            Respawn();
            return;
        }
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
        Debug.Log($"got {points} points");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (trickInProgress && shapeshifter.IsSkateboard && collision.gameObject.CompareTag("Ground"))
        {
            trickInProgress = false;
            shapeshifter.SetAnimationTrigger("trickCanceled");
            Debug.Log("trick failed! oops!");
            health.TakeDamage(failedTrickDamage);
        }

        ////TODO i don't think this does anything
        //if (collision.gameObject.TryGetComponent<InteractableEnvironmentItem>(out InteractableEnvironmentItem interactable))
        //{
        //    interactable.PlayerNearby();
        //}
    }

    /// <summary>
    /// announce that we have compelete a skateboard trick
    /// </summary>
    /// <param name="whichTrick"></param>
    private void TrickCompleted(TrickType whichTrick)
    {
        Debug.Log($"Finished a trick: {whichTrick}! nice!");
        trickInProgress = false;
        AddPoints(GameManager.Instance.SkateboardTrickDictionary[whichTrick].Points);
    }
}
