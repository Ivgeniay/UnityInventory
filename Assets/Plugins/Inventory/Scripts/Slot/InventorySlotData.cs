using System;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class InventorySlotData
    {
        [SerializeField] public ItemBase Item;
        [SerializeField] private int amount;
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
                else return 0;
            }
            set { }
        }

        public int Amount
        {
            get
            {
                if (Item != null)
                {
                    if (amount == 0) return 1;
                    return amount;
                }
                else return 0;
            }
            set
            {
                if (Item != null) amount = value;
                else amount = 0;
            }
        }

        //public virtual string ItemId { get; set; }
        //public virtual int SlotCapacity { get; set; } 
    }
}
