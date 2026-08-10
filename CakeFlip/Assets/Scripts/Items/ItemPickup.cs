using UnityEngine;

namespace Pickup
{
    [System.Serializable]
    public enum PickupableItemType
    {
        Undefined,
        Egg,
        Donut,
        Key,
        Pizza,
        IceCream,
        ToyShip,
        Saturn
    }

    public class ItemPickup : MonoBehaviour
    {
        public PickupableItemType ItemType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.TheInventoryManager.PickupItem(this);
                gameObject.SetActive(false);
            }
        }
    }
}
