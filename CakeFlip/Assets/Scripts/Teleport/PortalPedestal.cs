using UnityEngine;
using Util;

public class PortalPedestal : InteractableEnvironmentItem
{
    [SerializeField] private ItemPickup itemOnPedestal;
    [SerializeField] private GameObject itemOnPortal;
    [SerializeField] private Transform portalHoverPoint;
    [SerializeField] private Transform pedestalHoverPoint;
    [SerializeField] private TeleportArea teleportArea;
    [SerializeField] private string defaultScene;
    [SerializeField] private string interactMessage = "Put an Item here to change the teleporter's destination.";

    private void Start()
    {
        if (teleportArea == null)
        {
            Debug.LogWarning("teleport area not setup correctly");
            teleportArea = FindAnyObjectByType<TeleportArea>();
        }
        defaultScene = GameManager.Instance.ItemToLevelName[ItemPickup.PickupableItemType.Undefined];
        closeEnoughToInteractMessage = interactMessage;
    }

    protected override void DoPlayerInteraction()
    {
        if (itemOnPedestal == null)
        {
            Debug.Log("putting item on pedestal");
            //take current item out of inventory (drop it) and put on pedestal
            GameObject dropped = GameManager.Instance.DropCurrentItem(pedestalHoverPoint.position);
            if (dropped == null)
            {
                Debug.Log("nothing in inventory! so nothing to put on pedestal!");
                return;
            }
            dropped.SetActive(true);
            dropped.TrySetEnabledCollider(false);
            //TODO extension method for enable/disable collider

            itemOnPedestal = dropped.GetComponent<ItemPickup>();

            //update portal item and which dimension we're traveling to
            itemOnPortal = Instantiate(dropped, portalHoverPoint.position, dropped.transform.rotation);
            //lookup which dimension we should travel to aka the name of the scene to load
            teleportArea.SceneNameToTeleportTo = GameManager.Instance.ItemToLevelName[itemOnPedestal.ItemType];
        } 
        else
        {
            Debug.Log("swapping item on pedestal");
            //take item off pedestal.
            GameManager.Instance.PickupItem(itemOnPedestal);
            itemOnPedestal = null;

            //should be removed from pedestal. now remove from portal.
            Destroy(itemOnPortal);
            itemOnPortal = null;

            //set portal target back to default
            teleportArea.SceneNameToTeleportTo = defaultScene;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (itemOnPedestal == null)
        {
            //TODO show UI thing that says "press F to place item on pedestal"
        } else
        {
            //TODO show UI thing that says "press F to take item from pedestal"
        }
    }
}
