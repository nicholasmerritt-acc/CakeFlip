using TMPro;
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

        DisplayPointsNeededString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player.points >= pointsNeeded)
            {
                Debug.Log("you win yay! advancing to " + nextLevelName);
                levelCompleteText.SetActive(true);
                pointsNeededText.SetActive(false);
                GameManager.SetNextLevel(nextLevelName);
                Invoke(nameof(LoadNextScene), loadLevelDelay);
            }
            else
            {
                DisplayPointsNeededString();
            }
        }
    }

    void DisplayPointsNeededString()
    {
        string scoreString = $"score {pointsNeeded} points to pass this level!";
        Debug.Log(scoreString);

        if (pointsNeeded <= 0)
        {
            return;
        }

        pointsNeededText.SetActive(true);
        pointsNeededText.GetComponent<TMP_Text>().SetText(scoreString);
        Invoke(nameof(HidePointsNeeded), 3.0f);
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
