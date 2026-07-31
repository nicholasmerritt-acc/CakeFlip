using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public Dictionary<PickupableItemType, string> ItemToLevelName;
    public GameObject CurrentItem;
    private float itemDropOffset = -3f;
    public static event Action<string> ItemCarried;
    public static event Action<string> ItemDropped;

    [Header("References")]
    public PlayerController Player;

    private void Awake()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //these are initialized here because other Start() methods depend on them
        InitializeItemDictionary();
        InitializeTrickDictionaries();
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
        //TODO constants
        ItemToLevelName = new Dictionary<PickupableItemType, string>
        {
            { PickupableItemType.Undefined, "CityStreet" },
            { PickupableItemType.Egg, "CityStreet" },
            { PickupableItemType.Donut, "ScientistLab" },
            { PickupableItemType.Key, "ScientistLab" },
            { PickupableItemType.Pizza, "CrateIsland" },
            { PickupableItemType.IceCream, "CrateIsland" }
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
        if (CurrentItem != null)
        {
            //make new position a certain amount behind us
            Vector3 newPosition = Player.transform.position + Player.transform.forward * itemDropOffset;
            //newPosition.y = CurrentItem.transform.position.y;
            DropCurrentItem(newPosition);
        }

        //set current item to the item we just picked up
        CurrentItem = pickup.gameObject;
        ItemCarried?.Invoke(pickup.name);
        Debug.Log($"picked up {pickup.name}");
    }

    public GameObject DropCurrentItem(Vector3 newPosition)
    {
        if (CurrentItem == null)
        {
            Debug.Log("Trying to drop a null item!");
            return null;
        }

        //"drop" the current item
        CurrentItem.SetActive(true);

        //reparent the transform, so it now doesn't gain permanent dontdestroyonload powers
        CurrentItem.transform.parent = Player.transform.parent;
        CurrentItem.transform.position = newPosition;

        Debug.Log($"Dropped {CurrentItem} at new position {newPosition}");
        ItemDropped?.Invoke(CurrentItem.name);

        GameObject dropMe = CurrentItem;
        CurrentItem = null;
        return dropMe;
    }

    /// <summary>
    /// make sure our inventory persists between scenes
    /// </summary>
    public void SaveInventory()
    {
        if (CurrentItem != null)
        {
            CurrentItem.transform.parent = transform;
        }
    }

}
