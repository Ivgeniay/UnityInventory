using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class InventoryData
    {
        [SerializeField] public List<InventorySlotData> Slots;

        public void InitializeEmpty(int capacity = 1)
        {
            Slots = new List<InventorySlotData>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                Slots.Add(new InventorySlotData() { });
            }
        }
    }
}
