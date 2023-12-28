using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using PlasticGui.PreferencesWindow;
using UnityEditor.Graphs;
using Codice.Client.BaseCommands.BranchExplorer;

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
        public void Add(ItemBase item, int amount = 1)
        {
            ItemsDataBase.Instance.AddItem(item);
            Add(item.gameObject.GetInstanceID().ToString(), amount);
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
        public void Add(Vector2Int slotCoordinate, ItemBase item, int amount = 1)
        {
            ItemsDataBase.Instance.AddItem(item);
            Add(slotCoordinate, item.gameObject.GetInstanceID().ToString(), amount);
        }


        public bool RemoveFirst(string itemId, bool isDrop = true, int amount = 1)
        {
            if (!Has(itemId, amount)) return false;

            var amountToRemove = amount;
            Vector2Int size = inventoryConfig.InventorySize;
            int rows = size.y;
            int cols = size.x;
            int rowLength = size.x;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Vector2Int slotCoordinates = new Vector2Int(j, i); 
                    int index = slotCoordinates.x + rowLength * slotCoordinates.y;
                    InventorySlotData slot = inventoryData.Slots[index];
                    
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
        public bool RemoveFirst(ItemBase item, bool isDrop = true, int amount = 1)
        {
            ItemsDataBase.Instance.AddItem(item);
            return RemoveFirst(item.gameObject.GetInstanceID().ToString(), isDrop, amount);
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
        public bool Remove(Vector2Int slotCoordinate, ItemBase item, bool isDrop = true, int amount = 1)
        {
            ItemsDataBase.Instance.AddItem(item);
            return Remove(slotCoordinate, item.gameObject.GetInstanceID().ToString(), isDrop, amount);
        }

        public bool RemoveLast(string itemId, bool isDrop = true, int amount = 1)
        {
            if (!Has(itemId, amount)) return false;

            var amountToRemove = amount;
            Vector2Int size = inventoryConfig.InventorySize;
            int rows = size.y;
            int cols = size.x;
            int rowLength = size.x;
            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = cols - 1; j >= 0; j--)
                {
                    Vector2Int slotCoordinates = new Vector2Int(j, i);
                    int index = slotCoordinates.x + rowLength * slotCoordinates.y;
                    InventorySlotData slot = inventoryData.Slots[index];

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
        public bool RemoveLast(ItemBase item, bool isDrop = true, int amount = 1)
        {
            ItemsDataBase.Instance.AddItem(item);
            return RemoveLast(item.gameObject.GetInstanceID().ToString(), isDrop, amount);
        }

        public bool Has(string itemId, int amount = 1)
        {
            IEnumerable<InventorySlotData> allSlotsWithItem = inventoryData.Slots.Where(e => e.ItemId == itemId);
            int sumExist = 0;
            foreach (InventorySlotData slot in allSlotsWithItem)
                sumExist += slot.Amount;

            return sumExist >= amount;
        }
        public bool Has(ItemBase item, int amount = 1)
        {
            ItemsDataBase.Instance.AddItem(item); 
            return Has(item.gameObject.GetInstanceID().ToString(), amount); 
        }

        private void AddToSlotsWithSameItem(string itemId, int amount, out int remainingAmount)
        {
            Vector2Int size = inventoryConfig.InventorySize;
            int rows = size.y;
            int cols = size.x;
            int rowLength = size.x;
            remainingAmount = amount;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Vector2Int coordinates = new Vector2Int(j, i);
                    int index = coordinates.x + rowLength * coordinates.y;
                    InventorySlotData slot = inventoryData.Slots[index];
                    
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
            int rows = size.y;
            int cols = size.x;
            int rowLength = size.x;
            remainingAmount = amount;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Vector2Int coordinates = new Vector2Int(j, i);
                    int index = coordinates.x + rowLength * coordinates.y;
                    InventorySlotData slot = inventoryData.Slots[index];

                    if (!slot.IsEmpty()) continue;

                    slot.Item = value;

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
