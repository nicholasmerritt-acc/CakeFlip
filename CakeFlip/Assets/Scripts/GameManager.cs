using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum ItemType
    {
        Undefined,
        Egg,
        Donut,
        Key
    }

    //singleton. there can be only one
    public static GameManager Instance { get; private set; }

    public Dictionary<Trick.TrickType, Trick.SkateboardTrick> SkateboardTrickDictionary;
    public Dictionary<ItemType, GameObject> SpawnableItemTable; //TODO items etc

    public string nextLevelName = "Level1";

    private void Awake()
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

    private void Start()
    {
        InitializeTrickDictionary();
    }

    private void InitializeTrickDictionary()
    {
        SkateboardTrickDictionary = new()
        {
            {
                Trick.TrickType.Backflip,
                new Trick.SkateboardTrick
                {
                    WhichTrick = Trick.TrickType.Backflip,
                    Points = 10,
                    AnimationTrigger = "backflipTrigger",
                    Unlocked = false
                }
            },
            {
                Trick.TrickType.Sideflip,
                new Trick.SkateboardTrick
                {
                    WhichTrick = Trick.TrickType.Sideflip,
                    Points = 10,
                    AnimationTrigger = "sideflipTrigger",
                    Unlocked = false
                }
            },
            {
                Trick.TrickType.Treflip,
                new Trick.SkateboardTrick
                {
                    WhichTrick = Trick.TrickType.Treflip,
                    Points = 10,
                    AnimationTrigger = "treflipTrigger",
                    Unlocked = false
                }
            },
            {
                Trick.TrickType.Frontflip,
                new Trick.SkateboardTrick
                {
                    WhichTrick = Trick.TrickType.Frontflip,
                    Points = 10,
                    AnimationTrigger = "frontflipTrigger",
                    Unlocked = false
                }
            }
        };
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
