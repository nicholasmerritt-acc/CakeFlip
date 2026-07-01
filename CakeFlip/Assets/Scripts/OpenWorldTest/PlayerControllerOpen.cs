using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerOpen : MonoBehaviour
{
    bool hasJumped = false;
    private float jumpForce = 27f;

    public Transform respawnPosition;
    private int yBoundary = -30;

    private Rigidbody myRigidbody;
    private Animator myAnimator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
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

        if (Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
        }
        else if (Input.GetButtonDown("Stop"))
        {
            myRigidbody.linearVelocity = Vector3.zero; //super hard stop
            myRigidbody.angularVelocity = Vector3.zero;
                                                       //myRigidbody.AddForce(-myRigidbody.linearVelocity); 
        }

        if (!Grounded())
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
        float moveY = Input.GetAxisRaw("Vertical");


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
        transform.SetPositionAndRotation(respawnPosition.position, Quaternion.identity);
        myRigidbody.linearVelocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
    }
}
