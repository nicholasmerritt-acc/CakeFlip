using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerOpen : MonoBehaviour
{
    //[Header("Camera")]
    //[SerializeField] private float yaw = 0f;
    //[SerializeField] private float pitch = 15f;

    private InputSystem_Actions inputActions;

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
    [SerializeField] private LayerMask mask;

    [Header("Debugging")]
    [SerializeField] private Vector3 current;
    [SerializeField] private Vector3 bigTarget;
    [SerializeField] private Vector3 tinyTarget;

    [Header("References")]
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Transform cameraTransform;

    [Header("Positioning")]
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private int yBoundary = -30;

    [Header("Shapeshifting")]
    [SerializeField] private bool isSkateboard = false;
    [SerializeField] private GameObject Guy;
    [SerializeField] private Animator guyAnimator;
    [SerializeField] private GameObject Skateboard;
    [SerializeField] private Animator skateboardAnimator;
    public static event Action<bool> OnShapeshift;


    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        respawnPosition = transform.position;
        cameraTransform = Camera.main.transform;
        
        if (Guy == null)
        {
            //TODO find him
        }

        if (Skateboard == null)
        {
            //TODO find it. can't leave home without me trusty skateboard
        }

        if (guyAnimator == null)
        {
            guyAnimator = Guy.GetComponent<Animator>();
        }
        if (skateboardAnimator == null)
        {
            skateboardAnimator = Skateboard.GetComponent<Animator>();
        }
        BecomeMan();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Reset.performed += ResetPlayer;
        inputActions.Player.Jump.performed += Jump;
        inputActions.Player.Shapeshift.performed += ShapeshiftOnPress;

        inputActions.Player.Trick1.performed += DoTrick1;
        inputActions.Player.Trick2.performed += DoTrick2;
        inputActions.Player.Trick3.performed += DoTrick3;
    }

    private void OnDisable()
    {
        inputActions.Player.Reset.performed -= ResetPlayer;
        inputActions.Player.Jump.performed -= Jump;
        inputActions.Player.Shapeshift.performed -= ShapeshiftOnPress;

        inputActions.Player.Trick1.performed -= DoTrick1;
        inputActions.Player.Trick2.performed -= DoTrick2;
        inputActions.Player.Trick3.performed -= DoTrick3;
        inputActions.Player.Disable();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        float moveX = moveInput.x;
        float moveZ = moveInput.y;

        if (moveZ < 0 && isSkateboard)
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

    private void DoTrick1(InputAction.CallbackContext value)
    {
        DoTrick(Trick.TrickType.Frontflip);
    }
    private void DoTrick2(InputAction.CallbackContext value)
    {
        DoTrick(Trick.TrickType.Backflip);
    }
    private void DoTrick3(InputAction.CallbackContext value)
    {
        DoTrick(Trick.TrickType.Sideflip);
    }

    private void DoTrick(Trick.TrickType whichFlip)
    {
        //only attempt trick if we are an airborne skateboard
        if (!isSkateboard || Grounded())
        {
            return;
        }

        Trick.SkateboardTrick value = GameManager.Instance.SkateboardTrickDictionary[whichFlip];
        skateboardAnimator.SetTrigger(value.AnimationTrigger);
    }

    private void Jump(InputAction.CallbackContext value)
    {
        if (isSkateboard && Grounded())
        {
            hasJumped = true;
        }
    }

    private void ResetPlayer(InputAction.CallbackContext value)
    {
        Respawn();
    }


    // Update is called once per frame
    void Update()
    {
        //TODO menus

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene("MainMenu");
        //}

        if (transform.position.y < yBoundary)
        {
            Respawn();
            return;
        }
        HandleMovement();
    }

    private void ShapeshiftOnPress(InputAction.CallbackContext value)
    {
        Shapeshift(!isSkateboard);
    }

    public void Shapeshift(bool toSkateboard)
    {
        isSkateboard = toSkateboard;

        Skateboard.SetActive(toSkateboard);
        Guy.SetActive(!toSkateboard);

        //we just shapeshifted, so tell our listeners
        OnShapeshift?.Invoke(toSkateboard);
    }

    public void BecomeSkateboard()
    {
        Shapeshift(true);
    }

    public void BecomeMan()
    {
        Shapeshift(false);
    }


    private void FixedUpdate()
    {
        if (stopping)
        {
            hasJumped = false;
            myRigidbody.linearVelocity = Vector3.up * myRigidbody.linearVelocity.y; //super hard stop. zero out everything but the y
            myRigidbody.angularVelocity = Vector3.zero;
            //myRigidbody.AddForce(-myRigidbody.linearVelocity); ???
            stopping = false;
        }
        else if (isSkateboard)
        {
            Debug.Log("applying force");
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
            guyAnimator.SetFloat("Speed_f", moveMe.sqrMagnitude * speedAnimationAdjustment);

            Vector3 newMove = new Vector3(moveMe.x, myRigidbody.linearVelocity.y, moveMe.z);
            myRigidbody.linearVelocity = newMove;
        }
    }

    private bool Grounded()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, .5f, mask);

        if (grounded && !hit.collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("hit not ground: " + hit.collider.gameObject.name);
            grounded = false;
        }
        //grounded = transform.position.y < 1f;
        return grounded;
    }

    private void Respawn()
    {
        transform.SetPositionAndRotation(respawnPosition, Quaternion.identity);
        myRigidbody.linearVelocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
    }

    public void AddPoints(int points)
    {
        Debug.Log("got {points}");
    }
}
