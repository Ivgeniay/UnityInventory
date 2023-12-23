using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    internal class ItemsDataBase : ScriptableObject
    {
        [SerializeField] private List<ItemBase> items = new();
        private const string ASSET_PATH = "Assets/Resources/";
        private static string assetName => nameof(ItemsDataBase);
        private static ItemsDataBase instance;
        public static ItemsDataBase Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = Resources.Load<ItemsDataBase>(assetName);
                if (instance != null) return Instance;
                instance = CreateInstance<ItemsDataBase>();
#if UNITY_EDITOR
                if (!Directory.Exists(ASSET_PATH))
                {
                    Directory.CreateDirectory(ASSET_PATH);
                }
                UnityEditor.AssetDatabase.CreateAsset(instance, string.Concat(ASSET_PATH, assetName, ".asset"));
                UnityEditor.AssetDatabase.Refresh();
#endif
                return instance;
            }
        }

        public void AddItem(ItemBase item)
        {
            if (!items.Contains(item)) items.Add(item);
            items.RemoveAll(e => e == null);
        }

        public void RemoveItem(ItemBase item)
        {
            if (items.Contains(item)) items.Remove(item);
            items.RemoveAll(e => e == null);
        }

        public bool TryGetByID(string id, out ItemBase item)
        {
            item = items.FirstOrDefault(e => e.ID == id);
            return item != null;
        }
    }

}
