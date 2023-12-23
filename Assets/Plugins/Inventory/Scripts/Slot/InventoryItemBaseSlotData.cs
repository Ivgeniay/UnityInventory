using System;

namespace InventorySystem
{
    [System.Serializable]
    internal class InventoryItemBaseSlotData : InventorySlotData
    { 
        public override string ItemId
        {
            get
            {
                if (Item != null) return Item.ID;
                else return null;
            }
            set { if (value == null) Item = null; }
        }

        public override int SlotCapacity
        {
            get
            {
                if (Item != null) return Item.InventorySlotCapacity;
                else return int.MaxValue;
            }
            set { }
        }
    }
}
