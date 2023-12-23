
namespace InventorySystem
{
    [System.Serializable]
    public class InventorySlotUpdate
    {
        public InventorySlotData Sender;
        public ItemBase Item;
        public int Amount;

        public InventorySlotUpdate(InventorySlotData sender, ItemBase item, int amount)
        {
            this.Sender = sender;
            this.Item = item;   
            this.Amount = amount;
        }
    }
}
