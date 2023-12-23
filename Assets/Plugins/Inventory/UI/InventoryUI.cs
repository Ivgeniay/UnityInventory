using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/InventoryUI")]
    public static void ShowExample()
    {
        InventoryUI wnd = GetWindow<InventoryUI>();
        wnd.titleContent = new GUIContent("InventoryUI");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
