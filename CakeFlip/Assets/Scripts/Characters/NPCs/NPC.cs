using System.Collections;
using TMPro;
using UnityEngine;
using Util;

[RequireComponent(typeof(Health))]
public class NPC : InteractableEnvironmentItem
{
    [SerializeField] private TMP_Text floatingText;
    [SerializeField] private Health health;
    [SerializeField] private bool canDie = false;
    [SerializeField] private float defaultDialogueTimer = 5f;
    [SerializeField] protected string[] greetings = { "Hello there!", "Hi.", "What's up, skater?", "Oh, I didn't see you there." };
    [SerializeField] protected string definiteName;
    [SerializeField] private string[] names = { "Fred", "Martha", "Mystery Man", "Mystery Woman", "Blargo", "Brunhilde", "Marg", "Bratti", "Surya", "Cletus" };

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
            SayForever("I cannot die. Nice try.");
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

    protected void SayForever(string dialogue)
    {
        Say(dialogue, Mathf.Infinity);
    }

    public void Say(string dialogue, float timeout = 0)
    {
        StopAllCoroutines();
        if (timeout <= 0) {
            timeout = defaultDialogueTimer;
        }
        floatingText.text = dialogue;
        if (timeout != Mathf.Infinity)
        {
            IEnumerator HideDialogueTimed()
            {
                yield return new WaitForSeconds(timeout);
                HideDialogue();
            }
            StartCoroutine(nameof(HideDialogueTimed));
        }
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
