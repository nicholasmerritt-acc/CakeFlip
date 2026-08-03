using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScientistDialogue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject portal;
    private InputSystem_Actions inputActions;

    [Header("Dialogue")]
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private bool waiting = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.DialogueNext.performed += DialogueNextPressed;
    }

    private void DialogueNextPressed(InputAction.CallbackContext context)
    {
        if (waiting)
        {
            dialogueText.text = "Looks like the sedative has worn off. You should be able to move around now. Please, run along and leave me to my science. I've opened a portal for you.";
            cameraController.LookAroundEnabled = true;
            portal.SetActive(true);
        }
    }

    private void OnDisable()
    {
        inputActions.Player.DialogueNext.performed -= DialogueNextPressed;
        inputActions.Player.Disable();
    }

    public void InitialWakeupDialogue()
    {
        //when player wakes up, spotlight will fade in and then call this.
        dialogueText.gameObject.SetActive(true);
        //we need to now pause and wait until any key is pressed
        waiting = true;
    }

    void Start()
    {
        cameraController.LookAroundEnabled = false;
    }

    //TODO progress from dialogue to dialogue, and unlock different player abilities as we go.
    //TODO scientist tutorial help guy etc etc. for now he can just tell player to scram.
}
