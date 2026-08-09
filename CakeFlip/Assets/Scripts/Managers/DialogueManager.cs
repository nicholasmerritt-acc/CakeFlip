using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    private const string CONTINUE_DIALOGUE = "Press any key to continue...";
    [SerializeField] private bool blocking = false;
    [SerializeField] private float dialogueFadeDelay = 4f;
    [SerializeField] private TMP_Text dialogueText;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void Start()
    {
        if (GameManager.Instance.ThePauseGameHandler.isMainMenu)
        {
            return;
        }
        dialogueText = GameManager.Instance.HUD.DialogueText;
    }

    private void OnEnable()
    {
        inputActions.Player.DialogueNext.performed += DialogueNextPressed;
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.DialogueNext.performed -= DialogueNextPressed;
        inputActions.Player.Disable();
    }

    /// <summary>
    /// Say some dialogue that stays on the screen until the user dismisses it
    /// </summary>
    /// <param name="dialogue"></param>
    /// <param name="clip"></param>
    public void SayBlockingDialogue(string dialogue, AudioClip clip = null)
    {
        dialogueText.text = $"\"{dialogue}\"\n{CONTINUE_DIALOGUE}";
        blocking = true;
        

        if (clip != null)
        {
            GameManager.Instance.TheAudioManager.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Show something on the UI that will fade in a bit.
    /// </summary>
    public void SayNonBlockingDialogue(string dialogue, AudioClip clip = null)
    {
        Debug.Log(dialogue);
        dialogueText.text = dialogue;
        if (clip != null)
        {
            GameManager.Instance.TheAudioManager.PlayOneShot(clip);
        }
        StartCoroutine(nameof(HideTextAfterDelay));
    }

    /// <summary>
    /// Any key was pressed. Hide blocking dialogue
    /// </summary>
    /// <param name="context"></param>
    private void DialogueNextPressed(InputAction.CallbackContext context)
    {
        if (blocking)
        {
            HideDialogue();
        }
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(dialogueFadeDelay);
        HideDialogue();
    }

    public void HideDialogue()
    {
        dialogueText.text = "";
    }
}