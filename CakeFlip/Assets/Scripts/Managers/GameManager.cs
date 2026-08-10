using System;
using System.Collections.Generic;
using Pickup;
using Trick;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton. there can be only one
    public static GameManager Instance { get; private set; }

    [Header("Tricks")]
    public Dictionary<TrickType, SkateboardTrick> SkateboardTrickDictionary;
    public HashSet<TrickType> Unlocks;

    [Header("HUD")]
    [SerializeField] private HUD hudPrefab;
    private HUD hud;
    public HUD HUD
    {
        get
        {
            if (hud == null)
            {
                hud = Instantiate(hudPrefab, Canvas.transform);
            }
            return hud;
        }

        set => hud = value;
    }

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

    /// <summary>
    ///  Prefab library, for spawning pickupable items
    /// </summary>
    public Dictionary<PickupableItemType, ItemPickup> ItemTypeToPrefabTable;

    /// <summary>
    /// Since we can't serialize a dictionary, use this array to initialize. Indices should correspond to the PickupableitemType enum.
    /// </summary>
    [SerializeField] ItemPickup[] itemPickupPrefabsForDictionary;

    /// <summary>
    /// Matches item name to the level it will teleport you to
    /// </summary>
    public Dictionary<PickupableItemType, string> ItemToLevelNameTable;
    
    private void Awake()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        //these are initialized here because other Start() methods depend on them
        InitializeItemDictionaries();
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
        //Reset Canvas, since every scene should have its own
        canvas = null;
        ThePauseGameHandler.UnpauseGame();

        if (ThePauseGameHandler.IsMainMenu() || ThePauseGameHandler.IsIntroStarWarsScrollScene())
        {
            return;
        } 
        else
        {
            //save current scene and inventory to player prefs, so we can save our progress
            PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
            HUD.Initialize();
        }

    }

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (ThePauseGameHandler.IsMainMenu() || ThePauseGameHandler.IsIntroStarWarsScrollScene())
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
        if (ThePauseGameHandler.IsIntroScene())
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
        LoadScene("GameOver");
    }

    /// <summary>
    /// Reset all unlocks and start a brand new game.
    /// </summary>
    public void StartNewGame()
    {
        PlayerPrefs.SetString("CurrentScene", "Science");
        foreach (TrickType trick in Enum.GetValues(typeof(TrickType)))
        {
            PlayerPrefs.SetInt($"TrickUnlocked{trick}", 0);
            Unlocks.Remove(trick);
        }
        PlayerPrefs.SetInt("CurrentItem", 0);
        Instance.LoadScene("MainMenu");
    }


    /// <summary>
    /// Setup dictionaries of tricks and unlocks for future reference
    /// </summary>
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
                    Points = 100,
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
    }

    /// <summary>
    /// Initialize our very useful dictionaries. Note that we have to reconstruct the ItemTypeToPrefabTable very carefully based on index.
    /// </summary>
    private void InitializeItemDictionaries()
    {
        ItemToLevelNameTable = new Dictionary<PickupableItemType, string>
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

        ItemTypeToPrefabTable = new();
        for (int i = 0; i < itemPickupPrefabsForDictionary.Length; i++)
        {
            ItemTypeToPrefabTable[(PickupableItemType)i] = itemPickupPrefabsForDictionary[i];
        }
    }

    /// <summary>
    /// We can now perform this trick while in the air in skateboard form. Hooray!
    /// </summary>
    /// <param name="trickType"></param>
    public void Unlock(TrickType trickType)
    {
        Unlocks.Add(trickType);
        PlayerPrefs.SetInt($"TrickUnlocked{trickType}", 1);
    }

    /// <summary>
    /// Get a reference to the player
    /// </summary>
    /// <param name="playerController"></param>
    public void SetPlayer(PlayerController playerController)
    {
        player = playerController;
    }

    /// <summary>
    /// Bring up the loading screen and load the next level asynchronously
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        TheAsyncLoader.LoadLevelAsync(sceneName);
    }

    /// <summary>
    /// For debug (Utsab) use only. Unlock all the tricks in the game.
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
            GameObject restoredInventoryItem = Instantiate(ItemTypeToPrefabTable[typeToRestore].gameObject);
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
