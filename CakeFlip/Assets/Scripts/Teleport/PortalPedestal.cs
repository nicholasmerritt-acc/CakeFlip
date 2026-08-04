using UnityEngine;
using UnityEngine.InputSystem;
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

    private InputSystem_Actions inputActions;

    public override string InteractMessage { get => interactMessage; set => interactMessage = value; }

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void Start()
    {
        if (teleportArea == null)
        {
            Debug.LogWarning("teleport area not setup correctly");
            teleportArea = FindAnyObjectByType<TeleportArea>();
        }
        defaultScene = GameManager.Instance.ItemToLevelName[ItemPickup.PickupableItemType.Undefined];
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += OnPlayerInteraction;
    }

    private void OnPlayerInteraction(InputAction.CallbackContext context)
    {
        if (CanInteract)
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
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnPlayerInteraction;
        inputActions.Player.Disable();
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

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        //TODO update pedestal specific?
        //TODO hide UI thing
    }

    public override void PlayerNearby()
    {
        Debug.Log("pedestal is nearby the player! standing by");
        Debug.Log(interactMessage);
        Debug.Log(InteractMessage);
    }
}
