using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ItemPickup;
using Trick;
using Util;

public class GameManager : MonoBehaviour
{
    //singleton. there can be only one
    public static GameManager Instance { get; private set; }

    [Header("Tricks")]
    public Dictionary<TrickType, SkateboardTrick> SkateboardTrickDictionary;
    public HashSet<TrickType> UnlockedTricks;

    [Header("Inventory")]
    public Dictionary<PickupableItemType, GameObject> SpawnableItemTable; //prefab library, for spawning items
    public Dictionary<PickupableItemType, string> ItemToLevelName;
    public GameObject CurrentItem;
    private float itemDropOffset = -3f;
    public static event Action<string> ItemCarried;
    public static event Action<string> ItemDropped;

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Health playerHealth;
    public AudioManager TheAudioManager;
    public PauseGameHandler ThePauseGameHandler;
    public AsyncLoader TheAsyncLoader;
    private Canvas canvas;
    public Canvas Canvas
    {
        get
        {
            if (canvas == null)
            {
                canvas = FindAnyObjectByType<Canvas>();
            }
            return canvas;
        }

        set => canvas = value;
    }

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
        TheAudioManager = GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetCanvas;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetCanvas;
    }

    private void ResetCanvas(Scene scene, LoadSceneMode mode)
    {
        canvas = null;
    }

    private void Start()
    {
        FindPlayerReference();
    }

    private void FindPlayerReference()
    {
        if (ThePauseGameHandler.isMainMenu)
        {
            return;
        }

        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();
        }
        playerHealth = player.GetComponent<Health>();
        playerHealth.OnDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        GameManager.Instance.LoadScene("MainMenu");
    }

    private void InitializeTrickDictionaries()
    {
        SkateboardTrickDictionary = new()
        {
            {
                TrickType.Backflip,
                new SkateboardTrick
                {
                    WhichTrick = TrickType.Backflip,
                    Points = 10,
                    AnimationTrigger = "backflipTrigger",
                    Unlocked = false
                }
            },
            {
                TrickType.Sideflip,
                new SkateboardTrick
                {
                    WhichTrick = TrickType.Sideflip,
                    Points = 10,
                    AnimationTrigger = "sideflipTrigger",
                    Unlocked = false
                }
            },
            {
                TrickType.Treflip,
                new SkateboardTrick
                {
                    WhichTrick = TrickType.Treflip,
                    Points = 10,
                    AnimationTrigger = "treflipTrigger",
                    Unlocked = false
                }
            },
            {
                TrickType.Frontflip,
                new SkateboardTrick
                {
                    WhichTrick = TrickType.Frontflip,
                    Points = 10,
                    AnimationTrigger = "frontflipTrigger",
                    Unlocked = false
                }
            }
        };

        UnlockedTricks = new();
        UnlockTrick(TrickType.Backflip);
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
            { PickupableItemType.IceCream, "CrateIsland" },
            { PickupableItemType.ToyShip, "CrateIsland" },
            { PickupableItemType.Saturn, "CrateIsland" }
        };
    }

    public void UnlockTrick(TrickType trickType)
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
            Vector3 newPosition = player.transform.position + player.transform.forward * itemDropOffset;
            //newPosition.y = CurrentItem.transform.position.y;
            DropCurrentItem(newPosition);
        }

        //set current item to the item we just picked up
        CurrentItem = pickup.gameObject;

        //"pick up" item, aka parent it to the GameManager, and disable it
        ToggleCurrentItem(false);

        ItemCarried?.Invoke(pickup.name);
        Debug.Log($"picked up {pickup.name}");
    }

    /// <summary>
    /// Change the transfrom from an inactive child of the game manager to a real active item in the world, and vice versa
    /// </summary>
    /// <param name="dropping"></param>
    private void ToggleCurrentItem(bool dropping)
    {
        CurrentItem.TrySetEnabledCollider(dropping);
        CurrentItem.SetActive(dropping);

        if (dropping)
        {
            //reparent the transform, so it now doesn't gain permanent dontdestroyonload powers
            CurrentItem.transform.parent = player.transform.parent;
        } 
        else
        {
            CurrentItem.transform.parent = transform;
        }
    }

    public GameObject DropCurrentItem(Vector3 newPosition)
    {
        if (CurrentItem == null)
        {
            Debug.Log("Trying to drop a null item!");
            return null;
        }

        //"drop" the current item. aka return it to the way it was. enable gameObject and collider
        ToggleCurrentItem(true);

        CurrentItem.transform.position = newPosition;

        Debug.Log($"Dropped {CurrentItem} at new position {newPosition}");
        ItemDropped?.Invoke(CurrentItem.name);

        GameObject itemToReturn = CurrentItem;
        CurrentItem = null;
        return itemToReturn;
    }

    public void SetPlayer(PlayerController playerController)
    {
        player = playerController;
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("loading screen from gm");
        TheAsyncLoader.LoadLevelAsync(sceneName);
    }

    /// <summary>
    /// For debug use only. Unlock all the tricks in the game.
    /// </summary>
    public void DEBUG_UnlockAll()
    {
        foreach (TrickType trick in Enum.GetValues(typeof(TrickType)))
        {
            UnlockedTricks.Add(trick);
        }
    }
}
