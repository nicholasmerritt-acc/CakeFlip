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
    private bool hasJumped = false;
    [SerializeField] private bool grounded;
    [SerializeField] private float jumpForce = 27f;
    [SerializeField] private LayerMask groundedCheckMask;

    [Header("Debugging")]
    [SerializeField] private Vector3 current;
    [SerializeField] private Vector3 bigTarget;
    [SerializeField] private Vector3 tinyTarget;

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
            hasJumped = false;
            myRigidbody.linearVelocity = Vector3.up * myRigidbody.linearVelocity.y; //super hard stop. zero out everything but the y
            myRigidbody.angularVelocity = Vector3.zero;
            stopping = false;
        }
        else if (shapeshifter.IsSkateboard)
        {
            myRigidbody.AddForce(moveMe, ForceMode.Force);

            if (hasJumped)
            {
                myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                hasJumped = false;
            }
        }
        else
        {
            hasJumped = false;
            shapeshifter.SetAnimationFloat("Speed_f", moveMe.sqrMagnitude * speedAnimationAdjustment);

            Vector3 newMove = new Vector3(moveMe.x, myRigidbody.linearVelocity.y, moveMe.z);
            myRigidbody.linearVelocity = newMove;
        }
    }

    private void Jump(InputAction.CallbackContext value)
    {
        if (shapeshifter.IsSkateboard && Grounded())
        {
            hasJumped = true;
        }
    }


    public bool Grounded()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, .5f, groundedCheckMask);

        if (grounded && !hit.collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("hit not ground: " + hit.collider.gameObject.name);
            grounded = false;
        }
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

            //debug
            current = transform.rotation.eulerAngles;
            bigTarget = targetRotation.eulerAngles;
            tinyTarget = slerpTarget.eulerAngles;
        }

        moveMe = cameraRelativeMoveDirection * moveSpeed;
    }
}
