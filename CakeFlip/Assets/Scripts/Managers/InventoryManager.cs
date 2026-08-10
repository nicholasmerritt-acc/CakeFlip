using System;
using Pickup;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager: MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private PickupableItemType currentItem;
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

    /// <summary>
    /// Get player reference and make sure UI updates 
    /// </summary>
    private void ResetOnSceneChange(Scene scene, LoadSceneMode mode)
    {
        player = FindAnyObjectByType<PlayerController>();

        if (currentItem != PickupableItemType.Undefined)
        {
            UpdateUIForCarriedItem?.Invoke(currentItem.ToString());
        }
    }

    /// <summary>
    /// are we currently holding this item?
    /// </summary>
    public bool InventoryContains(PickupableItemType item)
    {
        return currentItem == item;
    }

    /// <summary>
    /// get rid of our current item without "dropping" aka instantiating it
    /// </summary>
    public void RemoveCurrentItem()
    {
        if (currentItem == PickupableItemType.Undefined)
        {
            return;
        }

        ItemDropped?.Invoke(currentItem.ToString());
    }

    /// <summary>
    /// Since we do not store the gameObject, but rather an enum value of its type, we need to instantiate it
    /// </summary>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    public GameObject DropCurrentItem(Vector3 newPosition)
    {
        if (currentItem == PickupableItemType.Undefined)
        {
            return null;
        }
        GameObject prefab = GameManager.Instance.ItemTypeToPrefabTable[currentItem].gameObject;
        GameObject dropped = Instantiate(prefab, newPosition, prefab.transform.rotation);
        ItemDropped?.Invoke(currentItem.ToString());

        currentItem = PickupableItemType.Undefined;
        return dropped;
    }
    /// <summary>
    /// we can only hold one item at a time, so if we pick something up, we must drop something
    /// </summary>
    /// <param name="pickup"></param>
    public void PickupItem(ItemPickup pickup)
    {
        if (currentItem != PickupableItemType.Undefined)
        {
            //make new position a certain amount behind us
            Vector3 newPosition = player.transform.position + player.transform.forward * itemDropOffset;
            DropCurrentItem(newPosition);
        }

        //set current item to the item we just picked up
        currentItem = pickup.ItemType;
        UpdateUIForCarriedItem?.Invoke(currentItem.ToString());
    }

    /// <summary>
    /// See what item we are holding without modifying it
    /// </summary>
    public PickupableItemType PeekCurrentItem()
    {
        return currentItem;
    }
}