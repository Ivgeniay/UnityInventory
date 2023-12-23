#if UNITY_EDITOR

using InventorySystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemBase), true)]
[CanEditMultipleObjects]
internal class ItemBaseDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (targets.Length > 1)
        {
            if (GUILayout.Button("GenerateNewIDs"))
            {
                for (int i = 0; i < targets.Length; i ++)
                {
                    ItemBase scr = (ItemBase)targets[i];
                    scr.GenerateNewID();
                }
            }
        }
        else
        {
            ItemBase scr = (ItemBase)target;
            if (string.IsNullOrEmpty(scr.ID))
            {
                if (GUILayout.Button("GenerateNewID"))
                {
                    scr.GenerateNewID();
                }
            }
        }
    }
}

#endif
