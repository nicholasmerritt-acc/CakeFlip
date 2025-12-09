using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public string nextLevelName;
    public float loadLevelDelay = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("you win yay! advancing to " + nextLevelName);
            GameManager.SetNextLevel(nextLevelName);
            Invoke(nameof(LoadNextScene), loadLevelDelay);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
