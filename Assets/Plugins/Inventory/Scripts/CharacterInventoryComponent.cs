﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class CharacterInventoryComponent : MonoBehaviour
    {
        [SerializeField] public int Rows = 1;
        [SerializeField] public int Columns = 1;
        [HideInInspector] public InventoryConfig InventoryConfig;
        [HideInInspector] public InventoryData InventoryData;
        [HideInInspector] public InventoryService Service;

        private void Awake()
        {
            if (InventoryConfig == null) InventoryConfig = new InventoryConfig()
            {
                InventorySize = new Vector2Int(Rows, Columns),
            };
            if (InventoryData == null)
            {
                InventoryData = new InventoryData();
                InventoryData.InitializeEmpty(InventoryConfig.InventorySize.x * InventoryConfig.InventorySize.y);
            }
            Service = new(InventoryData, InventoryConfig);
        }

#if UNITY_EDITOR 

        internal void InitializeFromEditor()
        {
            if (InventoryData == null || 
                InventoryData.Slots == null || 
                InventoryData.Slots.Count != (Rows * Columns) || 
                InventoryConfig.InventorySize.x != Rows || 
                InventoryConfig.InventorySize.y != Columns)
            {
                ResizeInventory();
            }
        }

        internal void ResizeInventory()
        {
            InventoryConfig = new InventoryConfig() { InventorySize = new Vector2Int(Rows, Columns), };
            int inventorySize = InventoryConfig.InventorySize.x * InventoryConfig.InventorySize.y;
            InventoryData InventoryData = new InventoryData();
            InventoryData.InitializeEmpty(inventorySize);
            if (this.InventoryData != null)
            {
                for (int i = 0; i < InventoryData.Slots.Count; i++)
                {
                    if (this.InventoryData.Slots != null && i < this.InventoryData.Slots.Count)
                    {
                        if (this.InventoryData.Slots[i] != null)
                            InventoryData.Slots[i] = this.InventoryData.Slots[i];
                    }
                    else break;
                }
            }
            this.InventoryData = InventoryData;
        }
#endif

    }

}
