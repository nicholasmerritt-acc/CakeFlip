using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 50;
    private bool jumping = false;
    private float jumpForce = 30;

    public Transform respawnPosition;
    private int yBoundary = -30;

    private Rigidbody myRigidbody;
    private Animator myAnimator;

    private GameObject currentScoreText;
    private GameObject incrementalScoreText;

    private string[] adjectiveList =
    {
        "Sick",
        "Wicked",
        "Gnarly",
        "Awesome",
        "Amazing",
        "Nice"
    };

    //number of points the player has for the current level
    public int points = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();

        currentScoreText = GameObject.FindGameObjectWithTag("CurrentScoreText");
        incrementalScoreText = GameObject.FindGameObjectWithTag("IncrementalScoreText");
        currentScoreText.SetActive(false);
        incrementalScoreText.SetActive(false);
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
        } else
        {
            //can only do tricks in midair!
            if (Input.GetKeyDown(KeyCode.Z))
            {
                myAnimator.SetTrigger("frontflipTrigger");
                AddPoints(200);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                myAnimator.SetTrigger("sideflipTrigger");
                AddPoints(100);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                myAnimator.SetTrigger("backflipTrigger");
                AddPoints(300);
            }
        }

            //zero out the z since this is a "2D" game. might change this in a bonus level or something?
            //portal sequence where we go through an abandoned warehouse in search of the real cake idk
            transform.SetPositionAndRotation(new(transform.position.x, transform.position.y, 0), transform.rotation);

    }

    public void AddPoints(int amount)
    {
        points += amount;
        currentScoreText.SetActive(true);
        currentScoreText.GetComponent<TMP_Text>().SetText($"{points}");
        GameObject newScoreText = Instantiate(incrementalScoreText, incrementalScoreText.transform.parent);
        newScoreText.SetActive(true);
        //TODO change position randomly since text is overlapping
        newScoreText.GetComponent<TMP_Text>().SetText($"+{amount}! {GetRandomAdjective()}!");

        IEnumerator FadeNewScore()
        {
            yield return new WaitForSeconds(1);
            newScoreText.SetActive(false);
        }

        StartCoroutine(FadeNewScore());
    }

    public string GetRandomAdjective()
    {
        return adjectiveList[Random.Range(0, adjectiveList.Length)];
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
