using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public string nextLevelName;
    private float loadLevelDelay = 2f;
    public int pointsNeeded = 0;

    private GameObject levelCompleteText;
    private GameObject pointsNeededText;

    void Start()
    {
        levelCompleteText = GameObject.FindGameObjectWithTag("LevelCompleteText");
        pointsNeededText = GameObject.FindGameObjectWithTag("PointsNeededText");
        levelCompleteText.SetActive(false);
        pointsNeededText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            if (GameManager.Instance.points >= pointsNeeded)
            {
                Debug.Log("you win yay! advancing to " + nextLevelName);
                levelCompleteText.SetActive(true);
                pointsNeededText.SetActive(false);
                GameManager.SetNextLevel(nextLevelName);
                Invoke(nameof(LoadNextScene), loadLevelDelay);
            }
            else
            {
                Debug.Log($"score {pointsNeeded} points to pass this level!");
                pointsNeededText.SetActive(true);
                Invoke(nameof(HidePointsNeeded), 5.0f);
            }
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    private void HidePointsNeeded()
    {
        pointsNeededText.SetActive(false);
    }
}
