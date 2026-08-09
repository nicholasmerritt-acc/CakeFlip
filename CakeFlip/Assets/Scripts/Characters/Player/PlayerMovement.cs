using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private InputSystem_Actions inputActions;
    private InputSystem_Actions.PlayerActions playerActions;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Vector3 moveMe;
    [SerializeField] private bool stopping;
    [SerializeField] private float speedAnimationAdjustment = 2f;

    [Header("Jumping")]
    private const float groundedCheckDistance = .5f;
    private bool hasJumpedFirstJump = false;
    private bool hasDoubleJumped = false;
    private bool hasTouchedGroundSinceDoubleJumping = true;
    [SerializeField] private bool grounded;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundedCheckMask;

    [Header("Audio")]
    [SerializeField] private float skateboardRollClipTimeout = 4f;
    [SerializeField] private float timeOfLastskateboardRollClip = 0f;
    [SerializeField] private AudioClip skateboardRollClip;
    [SerializeField] private AudioClip skateboardJumpClip;

    [Header("References")]
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerShapeshift shapeshifter;

    private void Start()
    {
        if (myRigidbody == null)
        {
            myRigidbody = GetComponent<Rigidbody>();
        }
        cameraTransform = Camera.main.transform;
    }

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerActions = inputActions.Player;
    }

    private void OnEnable()
    {
        playerActions.Enable();
        playerActions.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        playerActions.Jump.performed -= Jump;
        playerActions.Disable();
    }


    private void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        if (stopping)
        {
            hasJumpedFirstJump = false;
            myRigidbody.linearVelocity = Vector3.up * myRigidbody.linearVelocity.y; //super hard stop. zero out everything but the y
            myRigidbody.angularVelocity = Vector3.zero;
            stopping = false;
        }
        else if (shapeshifter.IsSkateboard)
        {
            myRigidbody.AddForce(moveMe, ForceMode.Force);
            TryPlaySkateboardRollClip();

            if (hasJumpedFirstJump)
            {
                DoPhysicsJumpWithSound();
                hasJumpedFirstJump = false;
            }
            //only double jump if we're absolutely sure we can
            if (hasDoubleJumped && hasTouchedGroundSinceDoubleJumping)
            {
                DoPhysicsJumpWithSound();
                hasDoubleJumped = false;
                hasTouchedGroundSinceDoubleJumping = false;
            }
        }
        else
        {
            hasJumpedFirstJump = false;
            hasDoubleJumped = false;
            shapeshifter.SetAnimationFloat("Speed_f", moveMe.sqrMagnitude * speedAnimationAdjustment);

            Vector3 newMove = new Vector3(moveMe.x, myRigidbody.linearVelocity.y, moveMe.z);
            myRigidbody.linearVelocity = newMove;
        }

        //only do grounded check if we are wanting to reset after double jumping. don't want to do it every frame.
        if (!hasTouchedGroundSinceDoubleJumping)
        {
            hasTouchedGroundSinceDoubleJumping = Grounded();
        }
    }

    private void DoPhysicsJumpWithSound()
    {
        myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        GameManager.Instance.TheAudioManager.PlayOneShot(skateboardJumpClip);
    }

    private void TryPlaySkateboardRollClip()
    {
        if (Time.time - timeOfLastskateboardRollClip > skateboardRollClipTimeout)
        {
            GameManager.Instance.TheAudioManager.PlayOneShot(skateboardJumpClip);
            timeOfLastskateboardRollClip = Time.time;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        //we can only jump in skateboard form.
        //also, two jumps maximum. if we've hit that, don't do anything more.
        if (!shapeshifter.IsSkateboard)
        {
            hasJumpedFirstJump = false;
            hasDoubleJumped = false;
            return;
        }
        if (Grounded())
        {
            hasJumpedFirstJump = true;
        }
        else
        {
            //if we are in the air, can only jump if double jump enabled
            //so either:
            //1. we are grounded. then we can always jump
            //2. not grounded. then we can only jump if double jump enabled and we haven't already double jumped
            if (DoubleJumpUnlocked())
            {
                hasDoubleJumped = true;
            }
        }
    }

    private bool DoubleJumpUnlocked()
    {
        return GameManager.Instance.Unlocks.Contains(Trick.TrickType.DoubleJump);
    }


    public bool Grounded()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundedCheckDistance, groundedCheckMask);
        return grounded;
    }


    private void HandleMovement()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        float moveX = moveInput.x;
        float moveZ = moveInput.y;

        if (moveZ < 0 && shapeshifter.IsSkateboard)
        {
            stopping = true;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 cameraRelativeMoveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

        if (cameraRelativeMoveDirection.sqrMagnitude > .001f && !stopping)
        {
            //get target rotation
            Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeMoveDirection);
            Quaternion slerpTarget = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            //slerp
            transform.rotation = slerpTarget;
        }

        moveMe = cameraRelativeMoveDirection * moveSpeed;
    }
}
