using System;
using System.Collections.Generic;
using Trick;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ItemPickup;

public class GameManager : MonoBehaviour
{
    //singleton. there can be only one
    public static GameManager Instance { get; private set; }

    [Header("Tricks")]
    public Dictionary<TrickType, SkateboardTrick> SkateboardTrickDictionary;
    public HashSet<TrickType> Unlocks;

    [Header("HUD")]
    [SerializeField] private HUD hudPrefab;
    public HUD HUD;

    [Header("Player")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Health playerHealth;

    [Header("References")]
    public AudioManager TheAudioManager;
    public InventoryManager TheInventoryManager;
    public DialogueManager TheDialogueManager;
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

    public Dictionary<PickupableItemType, ItemPickup> SpawnableItemTable; //prefab library, for spawning items
    public Dictionary<PickupableItemType, string> ItemToLevelName;
    
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
        SceneManager.sceneLoaded += ResetForNextScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetForNextScene;
    }

    /// <summary>
    /// Reset references and object states when the scene changes
    /// </summary>
    private void ResetForNextScene(Scene scene, LoadSceneMode mode)
    {
        //Reset HUD and Canvas so we don't try to reference them in the next scene
        canvas = null;
        HUD = null;
        //if we have a custom hud in the scene, use that. else, use our prefab
        HUD = FindAnyObjectByType<HUD>();
        if (HUD == null && !ThePauseGameHandler.isMainMenu)
        {
            HUD = Instantiate(hudPrefab, Canvas.transform);
        }
        ThePauseGameHandler.UnpauseGame();

        if (!ThePauseGameHandler.isMainMenu)
        {
            //save current scene and inventory to player prefs, so we can save our progress
            PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
        }
    }


    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (ThePauseGameHandler.isMainMenu || sceneName == "MainMenu")
        {
            return;
        }
        
        FindPlayerReference();
        TheDialogueManager.SayNonBlockingDialogue($"NOW ENTERING: The {sceneName} dimension");
    }

    /// <summary>
    /// Get a reference to the Player and its components
    /// </summary>
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

    /// <summary>
    /// For now, the power of science can infinitely respawn you. You do lose your item, though.
    /// </summary>
    private void OnPlayerDeath()
    {
        TheInventoryManager.DropCurrentItem(transform.position);
        playerHealth.HealToFull();
        LoadScene("Science");
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

        Unlocks = new();
        Unlock(TrickType.Backflip);
        //TODO get unlocks from playerprefs
    }

    private void InitializeItemDictionary()
    {
        ItemToLevelName = new Dictionary<PickupableItemType, string>
        {
            { PickupableItemType.Undefined, "Science" },
            { PickupableItemType.Egg, "Farm" },
            { PickupableItemType.Donut, "Dessert" },
            { PickupableItemType.Key, "Dungeon" },
            { PickupableItemType.Pizza, "Pizza" },
            { PickupableItemType.IceCream, "Dessert" },
            { PickupableItemType.ToyShip, "Pirate" },
            { PickupableItemType.Saturn, "Science" }
        };
    }

    public void Unlock(TrickType trickType)
    {
        Unlocks.Add(trickType);
        PlayerPrefs.SetInt($"TrickUnlocked{trickType}", 1);
    }

    public void SetPlayer(PlayerController playerController)
    {
        player = playerController;
    }

    public void LoadScene(string sceneName)
    {
        TheAsyncLoader.LoadLevelAsync(sceneName);
    }

    /// <summary>
    /// For debug use only. Unlock all the tricks in the game.
    /// </summary>
    public void DEBUG_UnlockAll()
    {
        foreach (TrickType trick in Enum.GetValues(typeof(TrickType)))
        {
            Unlocks.Add(trick);
        }
    }

    /// <summary>
    /// Get our saved progress from the PlayerPrefs.
    /// </summary>
    public void RestoreProgress()
    {
        foreach (TrickType trick in Enum.GetValues(typeof(TrickType)))
        {
            if (PlayerPrefs.GetInt($"TrickUnlocked{trick}", 0) == 1)
            {
                Unlocks.Add(trick);
            }
        }
        PickupableItemType typeToRestore = (PickupableItemType)PlayerPrefs.GetInt("CurrentItem", 0);
        if (typeToRestore != PickupableItemType.Undefined)
        {
            GameObject restoredInventoryItem = Instantiate(SpawnableItemTable[typeToRestore].gameObject);
            TheInventoryManager.PickupItem(restoredInventoryItem.GetComponent<ItemPickup>());
        }
    }

    /// <summary>
    /// Save our progress to playerprefs when we quit the application, so we can resume later.
    /// </summary>
    public void SaveProgress()
    {
        //we don't need to save unlocks, because those are saved when we unlock them.
        //however, we do need to save inventory
        PlayerPrefs.SetInt("CurrentItem", (int)TheInventoryManager.PeekCurrentItem());
    }
}
