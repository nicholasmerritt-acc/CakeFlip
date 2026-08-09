using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroVoiceoverTextScrollUp : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private int confirmCount = 0;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        GameManager.Instance.TheAudioManager.PlayIntroVoiceoverClip();
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        inputActions.Player.DialogueNext.performed += GoMainMenu;
        inputActions.Player.Enable();
    }

    private void GoMainMenu(InputAction.CallbackContext context)
    {
        if (confirmCount > 1)
        {
            //we don't want to use the async loader / loading screen here. just go straight to the main menu for this transition only
            SceneManager.LoadScene("MainMenu");
        } 
        else
        {
            confirmCount++;
        }
    }

    private void OnDisable()
    {
        inputActions.Player.DialogueNext.performed -= GoMainMenu;
        inputActions.Player.Disable();
        StopAllCoroutines();
    }
}
