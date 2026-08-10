using Util;
using Pickup;

public class FarmerJenkins : NPC
{
    private GameManager gameManager;
    private bool questComplete = false;

    protected override string CloseEnoughToInteractMessage
    {
        get => "Farmer Jenkins is tending to his chickens.";
        set => base.CloseEnoughToInteractMessage = value;
    }

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
    }

    private void PostQuestComplete()
    {
        CloseEnoughToInteractMessage = "Farmer Jenkins thanks you for the ice cream.";
    }

    protected override void DoPlayerInteraction()
    {
        //if player has an ice cream, remove it and unlock treflip
        if (gameManager.TheInventoryManager.InventoryContains(PickupableItemType.IceCream))
        {
            gameManager.TheInventoryManager.RemoveCurrentItem();
            gameManager.Unlock(Trick.TrickType.Treflip);
            gameManager.TheDialogueManager.SayNonBlockingDialogue("Delicious! Thank you.");
            PostQuestComplete();
            questComplete = true;
        }
        else if (questComplete)
        {
            Say(CloseEnoughToInteractMessage);
        }
        else
        {
            Say(greetings.GetRandomItem());
        }
    }
}
