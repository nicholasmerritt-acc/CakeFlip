using UnityEngine;
using UnityEngine.InputSystem;

public class PortalPedestal : MonoBehaviour
{
    [SerializeField] private bool pedestalEnabled = false;
    [SerializeField] private ItemPickup itemOnPedestal;
    [SerializeField] private GameObject itemOnPortal;
    [SerializeField] private Transform portalHoverPoint;
    [SerializeField] private Transform pedestalHoverPoint;
    [SerializeField] private TeleportArea teleportArea;
    [SerializeField] private string defaultScene;

    private InputSystem_Actions inputActions;
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

    private void OnPlayerInteraction(InputAction.CallbackContext value)
    {
        if (pedestalEnabled)
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
                if (dropped.TryGetComponent<Collider>(out Collider droppedCollider))
                {
                    droppedCollider.enabled = false;
                }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pedestalEnabled = true;
            Debug.Log("Pedestal activated");
        }
        if (itemOnPedestal == null)
        {
            //TODO show UI thing that says "press F to place item on pedestal"
        } else
        {
            //TODO show UI thing that says "press F to take item from pedestal"
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pedestalEnabled = false;
            Debug.Log("Pedestal deactivated");
        }
        //TODO hide UI thing
    }
}
