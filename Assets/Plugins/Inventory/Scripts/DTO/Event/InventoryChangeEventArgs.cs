using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class InventoryChangeEventArgs
    {
        public string ItemId { get; }
        public int Amount { get; }
        public Vector2Int InventorySlotCoordinates { get; }
        public InventoryChangeEventArgs(string itemId, int amount, Vector2Int inventorySlotCoordinates)
        {
            ItemId = itemId;
            Amount = amount;
            InventorySlotCoordinates = inventorySlotCoordinates;
        }
    }
}
