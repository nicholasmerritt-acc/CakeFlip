using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject SettingsPanel;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameManager.Instance.TheAudioManager;
    }

    public void ResetLevelOnClick()
    {
        audioManager.PlayButtonPressClip();
        GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Open the settings panel, for setting music and sfx volumes
    /// </summary>
    public void SettingsButtonOnClick()
    {
        audioManager.PlayButtonPressClip();
        SettingsPanel.SetActive(true);
    }

    /// <summary>
    /// Return to the main menu
    /// </summary>
    public void MainMenuOnClick()
    {
        audioManager.PlayButtonPressClip();
        GameManager.Instance.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quit the game. TODO Need to add alternate version if we ever want to build the game, since this will only work in the Editor
    /// </summary>
    public void QuitGameOnClick()
    {
        audioManager.PlayButtonPressClip();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    /// <summary>
    /// For debug use only. Unlock all the tricks in the game.
    /// </summary>
    public void DEBUG_UnlockAll()
    {
        GameManager.Instance.DEBUG_UnlockAll();
    }
}
