using System;
using System.Collections.Generic;
using TMPro;
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
    public static Dictionary<PickupableItemType, string> ItemToLevelName;
    public GameObject CurrentItem;
    private float dropoffOffset = -3f;
    public static event Action<string, string> InventoryChanged;

    [Header("References")]
    public PlayerControllerOpen Player;

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

        Player = FindAnyObjectByType<PlayerControllerOpen>();
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
        ItemToLevelName = new Dictionary<PickupableItemType, string>
        {
            { PickupableItemType.Undefined, "CityStreet" }
        };
    }

    public void UnlockTrick(Trick.TrickType trickType)
    {
        UnlockedTricks.Add(trickType);
        //TODO add trick to playerprefs
    }

    /// <summary>
    /// we can only hold one item at a time, so if we pick something up, we must drop something
    /// </summary>
    /// <param name="pickup"></param>
    public void PickupItem(ItemPickup pickup)
    {
        string droppedString = "";

        if (CurrentItem != null)
        {
            //"drop" the current item
            CurrentItem.SetActive(true);

            //reparent the transform, so it now doesn't gain permanent dontdestroyonload powers
            CurrentItem.transform.parent = Player.transform.parent;

            //make new position a certain amount behind us, but keep the height of the item before we picked it up
            Vector3 newPosition = Player.transform.position + Player.transform.forward * dropoffOffset;
            newPosition.y = CurrentItem.transform.position.y;
            CurrentItem.transform.position = newPosition;

            Debug.Log($"Dropped {CurrentItem} behind player");
            droppedString = CurrentItem.name;
        }

        //set current item to the item we just picked up
        CurrentItem = pickup.gameObject;

        InventoryChanged?.Invoke(pickup.name, droppedString);
        Debug.Log($"picked up {pickup.name}");
    }

    /// <summary>
    /// make sure our inventory persists between scenes
    /// </summary>
    public void SaveInventory()
    {
        CurrentItem.transform.parent = transform;
    }

}
