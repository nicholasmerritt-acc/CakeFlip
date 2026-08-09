using UnityEngine;

public class LinkedLadderArea : InteractableEnvironmentItem
{
    [SerializeField] private Transform otherLadderArea;

    protected override string CloseEnoughToInteractMessage { get => "Press F to use the Ladder"; set => base.CloseEnoughToInteractMessage = value; }

    protected override void DoPlayerInteraction()
    {
        //if we have a partner, teleport there
        if (otherLadderArea != null)
        {
            if (player == null)
            {
                Debug.LogError("failed to set player in the interactable item trigger");
            } 
            else
            {
                player.transform.position = otherLadderArea.position;
            }
        }
    }
}
