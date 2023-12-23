using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class InventoryData
    {
        public event Action<InventorySlotUpdate> OnUpdate;

        [SerializeField] public List<InventorySlotData> Slots;

        public void InitializeEmpty(int capacity = 1)
        {
            Slots = new List<InventorySlotData>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                var inventorySlot = new InventorySlotData();
                inventorySlot.OnUpdate += OnUpdateHandler;
                Slots.Add(inventorySlot);
            }
        }

        private void OnUpdateHandler(InventorySlotUpdate data) =>
            OnUpdate?.Invoke(data);
    }
}
