using System;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class InventorySlotData
    {
        [SerializeField] public ItemBase Item;
        [SerializeField] public int Amount;
        public virtual string ItemId
        {
            get
            {
                if (Item != null) return Item.ID;
                else return null;
            }
            set { if (value == null) Item = null; }
        }

        public virtual int SlotCapacity
        {
            get
            {
                if (Item != null) return Item.InventorySlotCapacity;
                else return int.MaxValue;
            }
            set { }
        }

        //public virtual string ItemId { get; set; }
        //public virtual int SlotCapacity { get; set; } 
    }
}
