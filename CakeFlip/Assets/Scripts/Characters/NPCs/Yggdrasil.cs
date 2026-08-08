using UnityEngine;
using UnityEngine.InputSystem;

public class Yggdrasil : InteractableEnvironmentItem
{
    protected override string CloseEnoughToInteractMessage
    {
        get => "YGGDRADISL HUNGERS. BRING ME A PIZZA.";
        set => base.CloseEnoughToInteractMessage = value;
    }

    private void Start()
    {
        if (GameManager.Instance.Unlocks.Contains(Trick.TrickType.DoubleJump))
        {
            ChangeMessageAfterQuestComplete();
        }
    }

    private void ChangeMessageAfterQuestComplete()
    {
        CloseEnoughToInteractMessage = "YGGDRASIL IS CONTENTEDLY EATING HIS PIZZA. YUMMM.";
    }

    protected override void OnPlayerInteraction(InputAction.CallbackContext context)
    {
        //if player has a pizza, remove it and unlock doublejump
        if (GameManager.Instance.InventoryContains(ItemPickup.PickupableItemType.Pizza))
        {
            GameManager.Instance.RemoveCurrentItem();
            GameManager.Instance.Unlock(Trick.TrickType.DoubleJump);
            GameManager.Instance.SayDialogue("EXCELLENT. I HAVE HUNGERED FOR A THOUSAND YEARS FOR THIS PIZZA.");
            ChangeMessageAfterQuestComplete();
        }
        else
        {
            GameManager.Instance.SayDialogue("NO, BRING ME A PIZZA.");
        }



    }
}
