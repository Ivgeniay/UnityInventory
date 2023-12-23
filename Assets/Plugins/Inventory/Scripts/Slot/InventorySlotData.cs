using System;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class InventorySlotData
    {
        public event Action<InventorySlotUpdate> OnUpdate;

        [SerializeField] public ItemBase item;
        [SerializeField] private int amount;

        public ItemBase Item
        {
            get { return item; }
            set { 
                if (value == null)
                    amount = 0;
                
                item = value;
                OnUpdate?.Invoke(new InventorySlotUpdate(this, item, amount));
            }
        }
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
                    return amount;
                }
                else return 0;
            }
            set
            {
                if (Item != null) amount = value;
                else amount = 0;
                OnUpdate?.Invoke(new InventorySlotUpdate(this, item, amount));
            }
        }
    }
}
