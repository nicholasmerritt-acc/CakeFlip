using System;
using TMPro;
using UnityEngine;

[Serializable]
public class HUD : MonoBehaviour
{
    public TMP_Text DialogueText;
    public TMP_Text InteractPromptText;
    public TMP_Text DroppedText;
    public TMP_Text InventoryText;

    public void Initialize()
    {
        DialogueText.text = "";
    }
}
