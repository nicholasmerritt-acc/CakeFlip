using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerOpen : MonoBehaviour
{
    [Header("Tweakables")]
    private bool hasJumped = false;
    [SerializeField] private float jumpForce = 27f;
    [SerializeField] private float yaw = 0f;
    [SerializeField] private float pitch = 15f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private int yBoundary = -30;

    [Header("Quaternion Debugging")]
    [SerializeField] private Vector3 current;
    [SerializeField] private Vector3 bigTarget;
    [SerializeField] private Vector3 tinyTarget;

    [Header("References")]
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController cc;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
        respawnPosition = transform.position;
        cameraTransform = Camera.main.transform;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene("MainMenu");
        //}
        
        if (Input.GetKeyDown(KeyCode.R) || transform.position.y < yBoundary)
        {
            Respawn();
            return;
        }
        else if (Input.GetButtonDown("Stop"))
        {
            myRigidbody.linearVelocity = Vector3.zero; //super hard stop
            myRigidbody.angularVelocity = Vector3.zero;
                                                       //myRigidbody.AddForce(-myRigidbody.linearVelocity); 
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

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 cameraRelativeMoveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;
        //cameraRelativeMoveDirection.Normalize();
        if (cameraRelativeMoveDirection.sqrMagnitude > .001f)
        {
            //get target rotation
            Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeMoveDirection);
            current = transform.rotation.eulerAngles;
            bigTarget = targetRotation.eulerAngles;
            Quaternion slerpTarget = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            tinyTarget = slerpTarget.eulerAngles;
            //slerp
            transform.rotation = slerpTarget;
        }

        //yaw += Input.GetAxisRaw("Mouse X") * 5f;
        //pitch -= Input.GetAxisRaw("Mouse Y") * 5f;
        //Quaternion newRotation = Quaternion.Euler(pitch, yaw, 0f);
        //transform.rotation = newRotation;

        //Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 moveMe = cameraRelativeMoveDirection * moveSpeed * Time.deltaTime;


        //transform.Translate(moveMe);
        cc.Move(moveMe);
    }

    private void FixedUpdate()
    {
        if (hasJumped)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasJumped = false;
        }
    }

    private bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    private void Respawn()
    {
        transform.SetPositionAndRotation(respawnPosition, Quaternion.identity);
        myRigidbody.linearVelocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
    }
}
