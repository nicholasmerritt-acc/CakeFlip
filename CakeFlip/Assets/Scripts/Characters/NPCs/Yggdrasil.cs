using UnityEngine;

public class Yggdrasil : InteractableEnvironmentItem
{
    [SerializeField] private AudioClip questCompleteClip;
    [SerializeField] private AudioClip questFailClip;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
    }

    protected override string CloseEnoughToInteractMessage
    {
        get => "YGGDRADISL HUNGERS. BRING ME A PIZZA.";
        set => base.CloseEnoughToInteractMessage = value;
    }

    private void Start()
    {
        if (gameManager.Unlocks.Contains(Trick.TrickType.DoubleJump))
        {
            PostQuestComplete();
        }
    }

    private void PostQuestComplete()
    {
        playClipOnEncounter = false;
        CloseEnoughToInteractMessage = "YGGDRASIL IS CONTENTEDLY EATING HIS PIZZA. YUMMM.";
    }

    protected override void DoPlayerInteraction()
    {
        //if player has a pizza, remove it and unlock doublejump
        if (gameManager.TheInventoryManager.InventoryContains(ItemPickup.PickupableItemType.Pizza))
        {
            gameManager.TheInventoryManager.RemoveCurrentItem();
            gameManager.Unlock(Trick.TrickType.DoubleJump);
            gameManager.SayDialogue("EXCELLENT. I HAVE HUNGERED FOR A THOUSAND YEARS FOR THIS PIZZA.");
            gameManager.TheAudioManager.PlayOneShot(questCompleteClip);
            PostQuestComplete();
        }
        else
        {
            gameManager.SayDialogue("NO, NOT THIS. BRING ME A PIZZA.");
            gameManager.TheAudioManager.PlayOneShot(questFailClip);
        }
    }
}
