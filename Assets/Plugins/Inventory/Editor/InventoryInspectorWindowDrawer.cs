using InventorySystem;
using UnityEditor;

namespace Inventory
{
    internal class InventoryInspectorWindowDrawer : EditorWindow
    {
        public static void Open(CharacterInventoryComponent target)
        {
            InventoryInspectorWindowDrawer window = GetWindow<InventoryInspectorWindowDrawer>($"Inventory of {target.gameObject.name}");
        }
    }
}
