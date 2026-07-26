using System.Collections;
using TMPro;
using UnityEngine;

public class ScientistDialogue : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject portal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraController.LookAroundEnabled = false;
        StartCoroutine(nameof(PlayerIsFree));
    }

    private IEnumerator PlayerIsFree()
    {
        yield return new WaitForSeconds(5f);
        cameraController.LookAroundEnabled = true;
        portal.SetActive(true);
    }


    //TODO progress from dialogue to dialogue, and unlock different player abilities as we go.
}
