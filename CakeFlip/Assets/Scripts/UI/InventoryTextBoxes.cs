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
        GameManager.InventoryChanged += UpdateInventoryAndDroppedText;
    }

    private void OnDisable()
    {
        GameManager.InventoryChanged -= UpdateInventoryAndDroppedText;
    }

    private void UpdateInventoryAndDroppedText(string newItemName, string droppedItemName)
    {
        //ignore previous text updates
        StopAllCoroutines();

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

        //now set dropped text
        if (string.IsNullOrEmpty(droppedItemName))
        {
            newText = "";
        }
        else
        {
            newText = $"Dropped: {droppedItemName}";
        }
        MyDroppedText.text = newText;
        StartCoroutine(nameof(FadeDroppedText));
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
