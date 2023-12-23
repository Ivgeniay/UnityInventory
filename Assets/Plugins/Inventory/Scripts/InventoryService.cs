using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using PlasticGui.PreferencesWindow;
using UnityEditor.Graphs;

namespace InventorySystem
{
    [System.Serializable]
    public class InventoryService
    {
        public SafeAction<InventoryChangeEventArgs> OnItemsAddedEvent = new();
        public SafeAction<InventoryChangeEventArgs> OnItemsRemovedEvent = new();
        public SafeAction<InventoryDroppedEventArgs> OnItemsDroppedEvent = new();

        private InventoryConfig inventoryConfig;
        private InventoryData inventoryData;

        public InventoryService(InventoryData inventoryData, InventoryConfig inventoryConfig) {
            this.inventoryData = inventoryData;
            this.inventoryConfig = inventoryConfig;
            
        }

        public void Add(string itemId, int amount = 1)
        {
            int remainingAmount = amount;
            AddToSlotsWithSameItem(itemId, remainingAmount, out remainingAmount);
            if (remainingAmount <= 0) return;
            
            AddToFirstAvailableSlot(itemId, remainingAmount, out remainingAmount);
            if (remainingAmount > 0) OnItemsDroppedEvent?.Invoke(new InventoryDroppedEventArgs(itemId, remainingAmount));
        }
        public void Add(Vector2Int slotCoordinate, string itemId, int amount = 1)
        {
            int rowLength = inventoryConfig.InventorySize.x;
            int slotIndex = slotCoordinate.x + rowLength * slotCoordinate.y;
            InventorySlotData slot = inventoryData.Slots[slotIndex];
            int newValue = slot.Amount + amount;

            if (slot.IsEmpty())
            {
                bool isExist = ItemsDataBase.Instance.TryGetByID(itemId, out ItemBase value);
                if (isExist)
                {
                    slot.Item = value;
                }
                else throw new Exception();
            }
            if (newValue > slot.SlotCapacity)                           
            {
                int remainingItems = newValue - slot.SlotCapacity;      
                int itemToAddAmount = slot.SlotCapacity - slot.Amount;  
                slot.Amount = slot.SlotCapacity;                        

                OnItemsAddedEvent?.Invoke(
                    new InventoryChangeEventArgs(
                        itemId, 
                        itemToAddAmount, 
                        slotCoordinate
                        )
                    );
                Add(itemId, remainingItems);
            }
            else
            {
                slot.Amount = newValue;
                OnItemsAddedEvent?.Invoke(
                    new InventoryChangeEventArgs(
                        itemId,
                        amount,
                        slotCoordinate
                        )
                    );
            }
        }
        public bool Remove(string itemId, bool isDrop = true, int amount = 1)
        {
            if (!Has(itemId, amount)) return false;

            var amountToRemove = amount;
            Vector2Int size = inventoryConfig.InventorySize;
            int rowLength = size.x;
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    Vector2Int slotCoordinates = new Vector2Int(i, j);
                    InventorySlotData slot = inventoryData.Slots[slotCoordinates.x + rowLength * slotCoordinates.y];
                    
                    if (slot.ItemId != itemId) continue;
                    if (amountToRemove > slot.Amount)
                    {
                        amountToRemove -= slot.Amount;
                        Remove(slotCoordinates, itemId, isDrop, slot.Amount);
                    }
                    else
                    {
                        Remove(slotCoordinates, itemId, isDrop, amountToRemove);
                        return true;
                    }
                }
            }

            return true;
        }
        public bool Remove(Vector2Int slotCoordinate, string itemId, bool isDrop = true, int amount = 1)
        {
            Vector2Int size = inventoryConfig.InventorySize;
            int rowLength = size.x;
            InventorySlotData slot = inventoryData.Slots[slotCoordinate.x + rowLength * slotCoordinate.y];

            if (slot.IsEmpty() || slot.ItemId != itemId || slot.Amount < amount) return false;

            slot.Amount -= amount;
            if (slot.Amount == 0) slot.Clean();
                
            OnItemsRemovedEvent?.Invoke(
                new InventoryChangeEventArgs(
                    itemId, 
                    amount, 
                    slotCoordinate)
                );
        
            if (isDrop)
            {
                OnItemsDroppedEvent?.Invoke(
                    new InventoryDroppedEventArgs(
                        itemId,
                        amount
                        )
                    );
            }

            return true;
        }
        public bool Has(string itemId, int amount = 1)
        {
            IEnumerable<InventorySlotData> allSlotsWithItem = inventoryData.Slots.Where(e => e.ItemId == itemId);
            int sumExist = 0;
            foreach (InventorySlotData slot in allSlotsWithItem)
                sumExist += slot.Amount;

            return sumExist >= amount;
        }

        private void AddToSlotsWithSameItem(string itemId, int amount, out int remainingAmount)
        {
            Vector2Int size = inventoryConfig.InventorySize;
            int rowLength = size.x;
            remainingAmount = amount;

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    Vector2Int coordinates = new Vector2Int(i, j);
                    InventorySlotData slot = inventoryData.Slots[coordinates.x + rowLength * coordinates.y];
                    
                    if (slot.IsEmpty()) continue;
                    if (slot.Amount >= slot.SlotCapacity) continue;             
                    if (slot.ItemId != itemId) continue;

                    int newValue = slot.Amount + remainingAmount;
                    int itemsToAddAmount = 0;
                    if (newValue > slot.SlotCapacity)                           
                    {
                        remainingAmount = newValue - slot.SlotCapacity;         
                        itemsToAddAmount = slot.SlotCapacity - slot.Amount;     
                        slot.Amount = slot.SlotCapacity;                        

                        OnItemsAddedEvent?.Invoke(new InventoryChangeEventArgs(
                            itemId,
                            itemsToAddAmount,
                            coordinates
                            ));
                    }
                    else
                    {
                        slot.Amount = newValue;
                        itemsToAddAmount = remainingAmount;
                        remainingAmount = 0;

                        OnItemsAddedEvent?.Invoke(new InventoryChangeEventArgs(
                            itemId,
                            itemsToAddAmount,
                            coordinates
                            ));
                        return;
                    }
                }
            }
        }
        private void AddToFirstAvailableSlot(string itemId, int amount, out int remainingAmount)
        {
            bool isExist = ItemsDataBase.Instance.TryGetByID(itemId, out ItemBase value);
            if (!isExist) throw new Exception();

            Vector2Int size = inventoryConfig.InventorySize;
            int rowLength = size.x;
            remainingAmount = amount;

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    Vector2Int coordinates = new Vector2Int(i, j);
                    InventorySlotData slot = inventoryData.Slots[coordinates.x + rowLength * coordinates.y];

                    if (!slot.IsEmpty()) continue;

                    slot.Item = value;
                    //slot.ItemId = itemId;

                    int newValue = remainingAmount;
                    int itemsToAddAmount = 0;

                    if (newValue > slot.SlotCapacity)                                           
                    {
                        remainingAmount = newValue - slot.SlotCapacity;                         
                        itemsToAddAmount = slot.SlotCapacity;                                   
                        slot.Amount = slot.SlotCapacity;                                        

                        OnItemsAddedEvent?.Invoke(new InventoryChangeEventArgs(
                            itemId,
                            itemsToAddAmount,
                            coordinates
                            ));
                    }
                    else
                    {
                        slot.Amount = newValue;
                        itemsToAddAmount = remainingAmount;
                        remainingAmount = 0;

                        OnItemsAddedEvent?.Invoke(new InventoryChangeEventArgs(
                            itemId,
                            itemsToAddAmount,
                            coordinates
                            ));
                        return;
                    }
                }
            }
        } 
    }
}
