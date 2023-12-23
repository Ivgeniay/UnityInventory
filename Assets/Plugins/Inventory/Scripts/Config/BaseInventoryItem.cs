using UnityEngine;
using System; 

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "EOTW/Inventory/Item")]
    [System.Serializable]
    public class BaseInventoryItem : ScriptableObject
    {
        [SerializeField] private Texture icon;
        [SerializeField] private ItemBase item;
        [SerializeField] private int count = 0;
        [SerializeField] private int stack = 10;

        public Texture Icon { get => icon; set => icon = value; }
        public ItemBase Item { get => item; set => item = value; }
        public int Count { get => count; set => count = value; }
        public int Stack { get => stack; set => stack = value; }

        public void Increment()
        {
            if (Count < Stack)
                Count++; 
        }

        public void Decrement()
        {
            if (Count > 1)
                Count--;
        }

    }
}
