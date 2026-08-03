using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGameHandler : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    public static event Action GamePaused;
    public static event Action GameUnpaused;

    [Header("Game State")]
    public bool IsPaused = false;

    [Header("References")]
    [SerializeField] private GameObject pauseMenuPrefab;
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
    private GameObject pauseMenu;
    public GameObject PauseMenu
    {
        get
        {
            if (pauseMenu == null)
            {
                pauseMenu = Instantiate(pauseMenuPrefab, Canvas.transform);
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

    private void OnPause(InputAction.CallbackContext context)
    {
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
            //unpause
            IsPaused = false;
            Time.timeScale = 1.0f;
            GameUnpaused?.Invoke();
        }
        else
        {
            IsPaused = true;
            Time.timeScale = 0.0f;
            GamePaused?.Invoke();
        }

        PauseMenu.SetActive(IsPaused);
        return IsPaused;
    }
}
