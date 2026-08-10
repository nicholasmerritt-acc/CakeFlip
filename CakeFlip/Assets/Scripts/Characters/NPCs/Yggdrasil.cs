using Pickup;
using UnityEngine;

/// <summary>
/// The ancient, famously hungry, deity??? is he a deity? is he a he? idk that much about mythology actually. sorry Yggdrasil
/// </summary>
public class Yggdrasil : InteractableEnvironmentItem
{
    [SerializeField] private AudioClip questCompleteClip;
    [SerializeField] private AudioClip questFailClip;
    [SerializeField] private AudioClip questStartClip;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
    }

    protected override string CloseEnoughToInteractMessage
    {
        get => "YGGDRASIL HUNGERS. BRING ME PIZZA.";
        set => base.CloseEnoughToInteractMessage = value;
    }

    private void Start()
    {
        if (gameManager.Unlocks.Contains(Trick.TrickType.DoubleJump))
        {
            PostQuestComplete();
        }
    }

    /// <summary>
    /// Change interaction based on whether we have complete the quest or not. This should become a virtual method if we add more NPCs like this in the future
    /// </summary>
    private void PostQuestComplete()
    {
        playClipOnEncounter = false;
        CloseEnoughToInteractMessage = "YGGDRASIL IS CONTENTEDLY EATING HIS PIZZA. YUMMM.";
    }

    protected override void DoPlayerInteraction()
    {
        //if player has a pizza, remove it and unlock doublejump
        if (gameManager.TheInventoryManager.InventoryContains(PickupableItemType.Pizza))
        {
            gameManager.TheInventoryManager.RemoveCurrentItem();
            gameManager.Unlock(Trick.TrickType.DoubleJump);
            gameManager.TheDialogueManager.SayNonBlockingDialogue("EXCELLENT. I HAVE HUNGERED FOR A THOUSAND YEARS FOR THIS PIZZA.", questCompleteClip);
            PostQuestComplete();
        }
        //if player tries to talk with no item in hand, tell them about the quest
        else if (gameManager.TheInventoryManager.InventoryContains(PickupableItemType.Undefined))
        {
            gameManager.TheDialogueManager.SayNonBlockingDialogue("I AM YGGDRASIL. BRING ME PIZZA.", questStartClip);
        }
        //if the player brings an item, tell them they have incorrectly chosen and should continue on their noble pizza quest
        else
        {
            gameManager.TheDialogueManager.SayNonBlockingDialogue("NO, NOT THIS. BRING ME PIZZA.", questFailClip);
        }
    }
}
