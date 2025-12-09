using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 40;
    private bool jumping = false;
    private float jumpForce = 20;

    public Transform respawnPosition;
    private int yBoundary = -30;

    private Rigidbody myRigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (Input.GetKeyDown(KeyCode.R) || transform.position.y < yBoundary)
        {
            Respawn();
            return;
        }
        else if (Grounded())
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                jumping = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                myRigidbody.linearVelocity = Vector3.zero; //super hard stop
                //myRigidbody.AddForce(-myRigidbody.linearVelocity); 
            }

            float horizontalInput = Input.GetAxis("Horizontal");
            //transform.Translate(Time.deltaTime * moveSpeed * horizontalInput * Vector3.right);
            myRigidbody.AddForce(horizontalInput * moveSpeed * Vector3.right);
        }

        //zero out the z since this is a "2D" game. might change this in a bonus level or something?
        //portal sequence where we go through an abandoned warehouse in search of the real cake idk
        transform.SetPositionAndRotation(new(transform.position.x, transform.position.y, 0), transform.rotation);

    }

    private void FixedUpdate()
    {
        if (jumping)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumping = false;
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
