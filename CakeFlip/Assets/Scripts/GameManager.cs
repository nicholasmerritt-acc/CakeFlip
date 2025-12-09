using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public string nextLevelName = "Level1";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetNextLevel(string nextLevelName)
    {
        if (instance != null)
        {
            instance.nextLevelName = nextLevelName;
        }
    }
}
