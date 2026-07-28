using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [System.Serializable]
    public enum PickupableItemType
    {
        Undefined,
        Egg,
        Donut,
        Key,
        Pizza
    }

    public PickupableItemType ItemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PickupItem(ItemType);
            Destroy(gameObject);
        }
    }
}
