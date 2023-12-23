namespace InventorySystem
{
    [System.Serializable]
    public class InventoryDroppedEventArgs
    {
        public string ItemId { get; }
        public int Amount { get; }
        public InventoryDroppedEventArgs(string itemId, int amount)
        {
            ItemId = itemId;
            Amount = amount;
        }
    }
}
