using System.Collections;
using TMPro;
using UnityEngine;
using Util;

[RequireComponent(typeof(Health))]
public class NPC : InteractableEnvironmentItem
{

    [SerializeField] private Health health;
    [SerializeField] private bool canDie = false;
    [Header("Dialogue")]
    [SerializeField] private float defaultDialogueTimer = 5f;
    [SerializeField] protected string[] greetings = { "Hello there!", "Hi.", "What's up, skater?", "Oh, I didn't see you there." };
    [SerializeField] private TMP_Text floatingText;
    [Header("Names")]
    [SerializeField] protected string definiteName;
    [SerializeField] private string[] names = { "Fred", "Martha", "Mystery Man", "Mystery Woman", "Blargo", "Brunhilde", "Marg", "Bratti", "Surya", "Cletus" };
    [SerializeField] private GameObject[] forms;

    private void Start()
    {
        if (string.IsNullOrEmpty(definiteName))
        {
            name = names.GetRandomItem();
        } 
        else
        {
            name = $"{definiteName} (NPC)";
        }

        if (health == null)
        {
            health = GetComponent<Health>();
        }

        if (forms.Length > 2)
        {
            int foundIndex = Random.Range(0, forms.Length);
            for (int i = 0; i < forms.Length; i++)
            {
                if (i == foundIndex)
                {
                    forms[i].SetActive(true);
                } else
                {
                    forms[i].SetActive(false);
                }
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        health.OnTakeDamage += SayOuch;
        health.OnDeath += TryDie;
    }

    protected void TryDie()
    {
        if (canDie)
        {
            Destroy(gameObject);
        } 
        else
        {
            Say("I cannot die. Nice try.");
            health.HealToFull();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        health.OnTakeDamage -= SayOuch;
        health.OnDeath -= TryDie;
        StopAllCoroutines();
    }

    protected void SayOuch()
    {
        Say("Ouch!");
    }

    /// <summary>
    /// Display some dialogue over our NPC's head.
    /// </summary>
    public void Say(string dialogue)
    {
        StopAllCoroutines();
        floatingText.text = dialogue;
        StartCoroutine(nameof(HideDialogueTimed));
    }

    private IEnumerator HideDialogueTimed()
    {
        yield return new WaitForSeconds(defaultDialogueTimer);
        HideDialogue();
    }

    public void HideDialogue()
    {
        floatingText.text = "";
    }

    protected override void DoPlayerInteraction()
    {
        Say(greetings.GetRandomItem());
    }
}
