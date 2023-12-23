using System;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class InventoryConfig
    {
        [SerializeField] public Vector2Int InventorySize { get; set; }
    }
}
