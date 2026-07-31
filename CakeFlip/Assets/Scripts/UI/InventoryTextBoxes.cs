using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryTextBoxes : MonoBehaviour
{
    [SerializeField] private TMP_Text MyInventoryText;
    [SerializeField] private TMP_Text MyDroppedText;
    [SerializeField] private float fadeDroppedDelay = 3.0f;

    private void OnEnable()
    {
        GameManager.ItemCarried += UpdateCarryingText;
        GameManager.ItemDropped += UpdateDroppedText;
    }

    private void OnDisable()
    {
        GameManager.ItemCarried -= UpdateCarryingText;
        GameManager.ItemDropped -= UpdateDroppedText;
    }

    private void UpdateCarryingText(string newItemName)
    {
        string newText = "";
        if (string.IsNullOrEmpty(newItemName))
        {
            newText = "<inventory empty>";
        }
        else
        {
            newText = $"Carrying: {newItemName}";
        }
        MyInventoryText.text = newText;

    }

    private void UpdateDroppedText(string droppedText) {
        //ignore previous text fade
        StopAllCoroutines();

        //now set dropped text
        if (string.IsNullOrEmpty(droppedText))
        {
            MyDroppedText.text = "";
        }
        else
        {
            MyDroppedText.text = $"Dropped: {droppedText}";
            StartCoroutine(nameof(FadeDroppedText));
        }
    }

    private IEnumerator FadeDroppedText()
    {
        yield return new WaitForSeconds(fadeDroppedDelay);
        MyDroppedText.text = "";
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
