using System.Collections.Generic;
using UnityEngine;
using static ItemPickup;

public class GameManager : MonoBehaviour
{
    //singleton. there can be only one
    public static GameManager Instance { get; private set; }

    [Header("Tricks")]
    public Dictionary<Trick.TrickType, Trick.SkateboardTrick> SkateboardTrickDictionary;
    public HashSet<Trick.TrickType> UnlockedTricks;

    [Header("Inventory")]
    public Dictionary<PickupableItemType, GameObject> SpawnableItemTable; //prefab library, for spawning items
    public PickupableItemType CurrentItem;
    public static Dictionary<PickupableItemType, string> ItemToLevelName;

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
        InitializeTrickDictionaries();
        InitializeItemDictionary();
    }

    private void InitializeTrickDictionaries()
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

        UnlockedTricks = new();
        UnlockTrick(Trick.TrickType.Backflip);
        //TODO get unlocks from playerprefs
    }

    private void InitializeItemDictionary()
    {
        ItemToLevelName = new Dictionary<PickupableItemType, string>();
        //TODO match all items to level name strings for loading
    }



    public void UnlockTrick(Trick.TrickType trickType)
    {
        UnlockedTricks.Add(trickType);
        //TODO add trick to playerprefs
    }

    public void PickupItem(ItemPickup.PickupableItemType itemType)
    {
        //TODO drop old item

        CurrentItem = itemType;
        //TODO update UI
        Debug.Log($"picked up {itemType}");
    }
}
