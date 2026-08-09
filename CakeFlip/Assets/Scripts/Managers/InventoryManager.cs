using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using static ItemPickup;

public class InventoryManager: MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject currentItem;
    private float itemDropOffset = -3f;
    
    public static event Action<string> UpdateUIForCarriedItem;
    public static event Action<string> ItemDropped;


    private void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetOnSceneChange;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetOnSceneChange;
    }


    private void ResetOnSceneChange(Scene scene, LoadSceneMode mode)
    {
        player = FindAnyObjectByType<PlayerController>();

        if (currentItem != null)
        {
            UpdateUIForCarriedItem?.Invoke(currentItem.GetComponent<ItemPickup>().name);
        }
    }

    public bool InventoryContains(PickupableItemType item)
    {
        if (currentItem != null)
        {
            if (currentItem.TryGetComponent<ItemPickup>(out ItemPickup itemPickup))
            {
                return itemPickup.ItemType == item;
            }
        }
        return false;
    }


    public void RemoveCurrentItem()
    {
        if (currentItem == null)
        {
            Debug.Log("Nothing in inventory to remove!");
            return;
        }

        ItemDropped?.Invoke(currentItem.name);
        Destroy(currentItem);
    }

    public GameObject DropCurrentItem(Vector3 newPosition)
    {
        if (currentItem == null)
        {
            Debug.Log("Trying to drop a null item!");
            return null;
        }

        //"drop" the current item. aka return it to the way it was. enable gameObject and collider
        ToggleCurrentItem(true);
        currentItem.transform.position = newPosition;
        ItemDropped?.Invoke(currentItem.name);

        GameObject itemToReturn = currentItem;
        currentItem = null;
        return itemToReturn;
    }
    /// <summary>
    /// we can only hold one item at a time, so if we pick something up, we must drop something
    /// </summary>
    /// <param name="pickup"></param>
    public void PickupItem(ItemPickup pickup)
    {
        if (currentItem != null)
        {
            //make new position a certain amount behind us
            Vector3 newPosition = player.transform.position + player.transform.forward * itemDropOffset;
            //newPosition.y = CurrentItem.transform.position.y;
            DropCurrentItem(newPosition);
        }

        //set current item to the item we just picked up
        currentItem = pickup.gameObject;

        //"pick up" item, aka parent it to the GameManager, and disable it
        ToggleCurrentItem(false);

        UpdateUIForCarriedItem?.Invoke(pickup.name);
    }

    /// <summary>
    /// Change the transfrom from an inactive child of the game manager to a real active item in the world, and vice versa
    /// </summary>
    /// <param name="dropping"></param>
    private void ToggleCurrentItem(bool dropping)
    {
        currentItem.TrySetEnabledCollider(dropping);
        currentItem.SetActive(dropping);

        if (dropping)
        {
            //reparent the transform
            currentItem.transform.SetParent(player.transform.parent);

            //TODO remove permanent dontdestroyonload powers... somehow...
        }
        else
        {
            currentItem.transform.SetParent(transform);
        }
    }

    public PickupableItemType PeekCurrentItem()
    {
        if (currentItem == null)
        {
            return PickupableItemType.Undefined;
        }
        else
        {
            return currentItem.GetComponent<ItemPickup>().ItemType;
        }
    }
}