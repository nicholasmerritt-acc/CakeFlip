using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseGameHandler : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    public static event Action GamePaused;
    public static event Action GameUnpaused;

    [Header("Game State")]
    public bool IsPaused = false;

    [Header("References")]
    [SerializeField] private GameObject pauseMenuPrefab;

    private GameObject pauseMenu;
    public GameObject PauseMenu
    {
        get
        {
            if (pauseMenu == null)
            {
                pauseMenu = Instantiate(pauseMenuPrefab, GameManager.Instance.Canvas.transform);
            }
            return pauseMenu;
        }

        set => pauseMenu = value;
    }
    private GameObject settingsPanel;
    public GameObject SettingsPanel
    {
        get
        {
            if (settingsPanel == null)
            {
                settingsPanel = PauseMenu.GetComponent<PauseMenu>().SettingsPanel;
            }
            return settingsPanel;
        }

        set => settingsPanel = value;
    }

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    /// <summary>
    /// There are certain tutorial scenes in which we want to skip setting up UI items
    /// </summary>
    /// <returns></returns>
    public bool IsIntroScene()
    {
        return IsMainMenu() || IsIntroStarWarsScrollScene();
    }

    public bool IsMainMenu()
    {
        return (SceneManager.GetActiveScene().buildIndex == 1) || SceneManager.GetActiveScene().name == "MainMenu";
    }

    public bool IsIntroStarWarsScrollScene()
    {
        return (SceneManager.GetActiveScene().buildIndex == 0) || SceneManager.GetActiveScene().name == "StarWarsScroll";
    }

    public bool IsScienceScene()
    {
        return (SceneManager.GetActiveScene().buildIndex == 2) || SceneManager.GetActiveScene().name == "Science";
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.PauseGame.performed += OnPause;
    }

    private void OnDisable()
    {
        inputActions.Player.PauseGame.performed -= OnPause;
        inputActions.Player.Disable();
    }

    /// <summary>
    /// Pause button has been triggered. Let's decide if we are toggling a pause, or just hiding a submenu
    /// </summary>
    /// <param name="context"></param>
    private void OnPause(InputAction.CallbackContext context)
    {
        if (IsIntroScene())
        {
            return;
        }
        if (PauseMenu == null)
        {
            Debug.LogError("PauseMenu prefab is not setup correctly.");
            return;
        }

        if (!SettingsPanel.activeInHierarchy)
        {
            TogglePause();
        }
        else
        {
            //if settings panel IS active, that means it is on top of an active pause menu.
            //so, turn off settings panel but keep pause menu intact
            SettingsPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Pause or unpause the game. Will instantiate PauseMenu if it is not already there.
    /// </summary>
    public bool TogglePause()
    {
        if (IsPaused)
        {
            return UnpauseGame();
        }
        else
        {
            return PauseGame();
        }
    }

    /// <summary>
    /// Pause the game.
    /// 
    /// PauseGame and UnpauseGame could be combined into one method, to save lines of code. 
    /// However, that is honestly kind of pointless, and it makes debugging so much harder. 
    /// This is much, much more readable. I learned the hard way.
    /// </summary>
    public bool PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0.0f;
        GamePaused?.Invoke();
        PauseMenu.SetActive(true);
        return true;
    }

    /// <summary>
    /// Unpause the game.
    /// </summary>
    public bool UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1.0f;
        GameUnpaused?.Invoke();
        PauseMenu.SetActive(false);
        return false;
    }
}
