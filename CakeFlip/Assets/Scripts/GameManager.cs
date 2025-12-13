using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton. there can be only one
    public static GameManager Instance { get; private set; }

    public string nextLevelName = "Level1";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// save which level we are on, so if we quit to main menu we can go back to where we left off
    /// </summary>
    /// <param name="nextLevelName">name of the scene unity is to load as the next level</param>
    public static void SetNextLevel(string nextLevelName)
    {
        if (Instance != null)
        {
            Instance.nextLevelName = nextLevelName;
        }
    }
}
