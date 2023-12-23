using UnityEngine;
using System; 

namespace InventorySystem
{
    [System.Serializable]
    public class ItemBase : MonoBehaviour
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public int InventorySlotCapacity { get; private set; } = 1;
        [field: SerializeField] public Texture2D Icon { get; private set; } = null;

        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(ID)) GenerateNewID();
        }

        public void GenerateNewID() => ID = Guid.NewGuid().ToString();
    }
}
