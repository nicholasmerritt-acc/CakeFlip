using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Button that allows you to escape out of a menu: a pause menu, a settings menu, etc
/// </summary>
public class PanelBackButton : MonoBehaviour
{
    public GameObject PanelToDeactivate;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.PauseGame.performed += OnHideCancel;
    }

    private void OnDisable()
    {
        inputActions.Player.PauseGame.performed -= OnHideCancel;
        inputActions.Player.Disable();
    }

    public void BackButtonOnClick()
    {
        GameManager.Instance.TheAudioManager.PlayButtonPressClip();
        DeactivatePanel();
    }

    private void OnHideCancel(InputAction.CallbackContext context)
    {
        DeactivatePanel();
    }

    public virtual void DeactivatePanel()
    {
        PanelToDeactivate.SetActive(false);
    }
}
