using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryTextBoxes : MonoBehaviour
{
    [SerializeField] private TMP_Text MyInventoryText;
    [SerializeField] private TMP_Text MyDroppedText;
    [SerializeField] private float fadeDroppedDelay = 3.0f;
    private const string INVENTORY_EMPTY_STRING = "<inventory empty>";

    private void OnEnable()
    {
        InventoryManager.UpdateUIForCarriedItem += UpdateCarryingText;
        InventoryManager.ItemDropped += UpdateDroppedText;
    }

    private void OnDisable()
    {
        InventoryManager.UpdateUIForCarriedItem -= UpdateCarryingText;
        InventoryManager.ItemDropped -= UpdateDroppedText;
    }

    /// <summary>
    /// Update the UI text that tells us what is in our inventory.
    /// </summary>
    private void UpdateCarryingText(string newItemName)
    {
        string newText = "";
        if (string.IsNullOrEmpty(newItemName))
        {
            newText = INVENTORY_EMPTY_STRING;
        }
        else
        {
            newText = $"Carrying: {newItemName}";
        }
        MyInventoryText.text = newText;

    }

    /// <summary>
    /// Update the UI text that tells us what item we most recently dropped.
    /// </summary>
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

        //if we dropped something, inventory is now empty
        MyInventoryText.text = INVENTORY_EMPTY_STRING;
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
