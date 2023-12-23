namespace InventorySystem
{
    internal static class InventorySlotDataExtensions
    {
        internal static bool IsEmpty(this InventorySlotData source) =>
            source.Amount <= 0 || string.IsNullOrEmpty(source.ItemId);

        internal static void Clean(this InventorySlotData source)
        {
            source.Item = null;
            //source.Amount = 0;
            //source.ItemId = null;
        }
    }
}
