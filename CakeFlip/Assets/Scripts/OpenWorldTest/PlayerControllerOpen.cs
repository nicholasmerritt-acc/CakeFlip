using UnityEngine;

public class PlayerControllerOpen : MonoBehaviour
{
    [Header("Tweakables")]
    [SerializeField] private float yaw = 0f;
    [SerializeField] private float pitch = 15f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Vector3 moveMe;
    [SerializeField] private bool stopping;

    [Header("")]
    private bool hasJumped = false;
    [SerializeField] private bool grounded;
    [SerializeField] private float jumpForce = 27f;

    [Header("Debugging")]
    [SerializeField] private Vector3 current;
    [SerializeField] private Vector3 bigTarget;
    [SerializeField] private Vector3 tinyTarget;

    [Header("References")]
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Transform cameraTransform;

    [Header("")]
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private int yBoundary = -30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponentInChildren<Animator>();
        respawnPosition = transform.position;
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene("MainMenu");
        //}

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.R) || transform.position.y < yBoundary)
        {
            Respawn();
            return;
        }
        else if (moveZ < 0)
        {
            stopping = true;
        }

        if (Grounded())
        {
            if (Input.GetButtonDown("Jump"))
            {
                hasJumped = true;
            }
        }
        else
        {
            int pointsToAdd = 0;
            //can only do tricks in midair! keep doing tricks after victory screen but don't add the score
            if (Input.GetKeyDown(KeyCode.Z))
            {
                myAnimator.SetTrigger("frontflipTrigger");
                pointsToAdd = 200;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                myAnimator.SetTrigger("sideflipTrigger");
                pointsToAdd = 100;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                myAnimator.SetTrigger("backflipTrigger");
                pointsToAdd = 300;
            }
            if (pointsToAdd > 0)
            {
                //AddPoints(pointsToAdd);
                Debug.Log($"Scored {pointsToAdd} points!");
            }
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

        //yaw += Input.GetAxisRaw("Mouse X") * 5f;
        //pitch -= Input.GetAxisRaw("Mouse Y") * 5f;
        //Quaternion newRotation = Quaternion.Euler(pitch, yaw, 0f);
        //transform.rotation = newRotation;

        //Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        moveMe = cameraRelativeMoveDirection * moveSpeed;


        //transform.Translate(moveMe);
        //cc.Move(moveMe);
    }

    private void FixedUpdate()
    {
        if (hasJumped)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasJumped = false;
        }

        if (stopping)
        {
            myRigidbody.linearVelocity = Vector3.zero; //super hard stop
            myRigidbody.angularVelocity = Vector3.zero;
            //myRigidbody.AddForce(-myRigidbody.linearVelocity); ???
            stopping = false;
        }
        else
        {

            myRigidbody.AddForce(moveMe, ForceMode.Force);
        }

    }

    private bool Grounded()
    {
        grounded = transform.position.y < 1f;
        return grounded;

        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, LayerMask.NameToLayer("Player"));
        Debug.DrawRay(transform.position, Vector3.down);
        if (grounded && !hit.collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("hit " + hit.collider.gameObject.name);
            grounded = false;
        }
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
