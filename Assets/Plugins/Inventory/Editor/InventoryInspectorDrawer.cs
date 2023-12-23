#if UNITY_EDITOR 
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using InventorySystem;
using UnityEditor;
using UnityEngine;
using Inventory;
using UnityEditor.Callbacks;
using static UnityEngine.GraphicsBuffer;

public class AssetHandler
{
    [OnOpenAsset]
    public static bool OpenEditor(int instanceId, int line)
    {
        CharacterInventoryComponent inventoryComponent = EditorUtility.InstanceIDToObject(instanceId) as CharacterInventoryComponent;
        if (inventoryComponent != null)
        {
            InventoryInspectorWindowDrawer.Open(inventoryComponent);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(CharacterInventoryComponent))]
internal class InventoryInspectorDrawer : Editor
{
    private const string TREE_PATH = "Assets/Plugins/Inventory/UI/InventoryUI.uxml";
    private const string CELL_TREE_PATH = "Assets/Plugins/Inventory/UI/InventoryCell/Cell.uxml";
    private const string STYLE_PATH = "Assets/Plugins/Inventory/UI/InventoryUI.uss";

    [SerializeField] private VisualTreeAsset tree;
    [SerializeField] private StyleSheet styles;
    [SerializeField] private VisualTreeAsset cellTree;

    private List<VisualElement> inventoryRow = new();
    private Dictionary<int, VisualElement> slotCell = new();
    private VisualElement cachedVisualTree;
    private VisualElement inventory;

    private SerializedObject serializedObjectt;
    private SerializedProperty rowsProperty;
    private SerializedProperty columnsProperty;

    CharacterInventoryComponent inventoryComponent;

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();

    //    if (GUILayout.Button("Open Editor"))
    //    {
    //        InventoryInspectorWindowDrawer.Open(target as CharacterInventoryComponent);
    //    }
    //}

    private void OnDisable()
    {
        inventoryRow.Clear();
        slotCell.Clear();
    }

    private void OnEnable()
    {
        serializedObjectt = new SerializedObject(target);
        rowsProperty = serializedObject.FindProperty("Rows");
        columnsProperty = serializedObject.FindProperty("Columns");

        inventoryRow = new();
        slotCell = new();
    }

    public override VisualElement CreateInspectorGUI()
    {
        inventoryComponent = (CharacterInventoryComponent)target;
        if (inventoryComponent == null) return null;

        inventoryComponent.InitializeFromEditor();
        serializedObject.ApplyModifiedProperties();

        if (tree is null) tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TREE_PATH);
        if (styles is null) styles = AssetDatabase.LoadAssetAtPath<StyleSheet>(STYLE_PATH);

        VisualElement root = new();
        tree.CloneTree(root);
        root.styleSheets.Add(styles);

        AddSizeFields(inventoryComponent, root);

        inventory = root.Q<VisualElement>("inventory"); 
        DrawInventory(inventory);

        cachedVisualTree = root;
        return root;
    }

    private void DrawInventory(VisualElement inventory)
    {
        foreach (VisualElement item in slotCell.Values) {
            VisualElement parent = item.parent;
            parent.Remove(item);
        }
        foreach (var item in inventoryRow)
        {
            VisualElement parent = item.parent;
            parent.Remove(item);
        }
        inventoryRow.Clear();
        slotCell.Clear();

        for (int i = 0; i < inventoryComponent.Rows; i++)
        {
            VisualElement row = CreateInventoryRow(inventory);
            inventoryRow.Add(row);
        }

        int counter = 0;
        for (int i = 0; i < inventoryComponent.Rows; i++)
        {
            for (int j = 0; j < inventoryComponent.Columns; j++)
            {
                VisualElement cell = CreateCell(inventoryComponent, counter, inventoryRow[i]);
                slotCell.Add(counter, cell);
                counter++;
            }
        }
    }

    private VisualElement CreateInventoryRow(VisualElement root = null)
    {
        VisualElement visualElement = new VisualElement();
        visualElement.style.flexDirection = new StyleEnum<FlexDirection>()
        {
            value = FlexDirection.Row,
        };
        if (root != null) root.Add(visualElement);

        return visualElement;
    }
    private VisualElement CreateCell(CharacterInventoryComponent inventoryComponent, int slotIndex, VisualElement parentVisualElement)
    {
        VisualElement cell = new();
        if (cellTree is null) cellTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(CELL_TREE_PATH);
        cellTree.CloneTree(cell);

        VisualElement inputField = cell.Q<VisualElement>(null, "unity-base-field__input");
        if (inputField != null)
            inputField.style.backgroundColor = new StyleColor(Color.clear);

        InventorySlotData slotData = inventoryComponent.InventoryData.Slots[slotIndex];

        ObjectField obField = cell.GetElement<ObjectField>();
        VisualElement cellBackground = cell.Q<VisualElement>("cellBackground");
        if (slotData.Item != null)
            cellBackground.style.backgroundImage = slotData.Item.Icon;
        
        Label counter = cell.Q<Label>("counter");
        slotData.OnUpdate += (e) => 
        {
            obField.value = e.Item;
            UpdateCounter(counter, e.Amount, e.Sender.SlotCapacity); 
        };
        obField.RegisterValueChangedCallback(ObjectFieldCallback(slotData, cellBackground, counter));

        counter.text = string.Concat(slotData.Amount, " / ", slotData.SlotCapacity);
        Button incrementButton = cell.Q<Button>("increment");
        Button decrementButton = cell.Q<Button>("decrement");

        incrementButton.clicked += () => {
            if (slotData.Item != null && slotData.Amount < slotData.SlotCapacity)
            {
                slotData.Amount += 1;
                OnApply();
            } 
        };

        decrementButton.clicked += () => {
            if (slotData.Item != null)
            {
                if (slotData.Amount > 1)
                {
                    slotData.Amount -= 1;
                    OnApply();
                }
                else
                {
                    slotData.Clean();
                    OnApply();
                }
            }
        };

        if (slotData != null)
            obField.value = slotData.Item;

        parentVisualElement.Add(cell);
        return cell;
    }

    private void UpdateCounter(Label counter, int amount, int capacity) =>
        counter.text = string.Concat(amount, " / ", capacity);
    

    private EventCallback<ChangeEvent<Object>> ObjectFieldCallback(InventorySlotData slotData, VisualElement cellBackground, Label counter)
    {
        return e =>
        {
            ItemBase value = null;
            if (e.newValue != null)
                value = e.newValue as ItemBase;

            slotData.Item = value;
            if (slotData != null && slotData.Item != null && slotData.Item.Icon != null)
            {
                cellBackground.style.backgroundImage = slotData.Item.Icon;
                if (slotData.Amount == 0) slotData.Amount += 1; 
                ItemsDataBase.Instance.AddItem(value);
            }
            else
                cellBackground.style.backgroundImage = null;

            UpdateCounter(counter, slotData.Amount, slotData.SlotCapacity);
            OnApply();
        };
    }

    private void AddSizeFields(CharacterInventoryComponent inventoryComponent, VisualElement root)
    {
        IntegerField rowField = root.Q<IntegerField>("rowField");
        IntegerField colField = root.Q<IntegerField>("colField");

        rowField.value = rowsProperty.intValue;
        colField.value = columnsProperty.intValue;

        rowField.RegisterValueChangedCallback(OnRowChange);
        colField.RegisterValueChangedCallback(OnColumnChange);
    } 
    private void OnColumnChange(ChangeEvent<int> e)
    {
        int result = 1;
        if (e.newValue > 0) result = e.newValue;
        else
        {
            IntegerField source = e.target as IntegerField;
            if (source != null)
                source.value = result; 
        }
        
        columnsProperty.intValue = result;
        inventoryComponent.Columns = result;
        ((CharacterInventoryComponent)target).ResizeInventory();
        DrawInventory(inventory);
        OnApply();
    } 
    private void OnRowChange(ChangeEvent<int> e)
    {
        int result = 1;
        if (e.newValue > 0) result = e.newValue;
        else
        {
            IntegerField source = e.target as IntegerField;
            if (source != null)
                source.value = result;
        }

        rowsProperty.intValue = result;
        inventoryComponent.Rows = result;
        ((CharacterInventoryComponent)target).ResizeInventory();
        DrawInventory(inventory);
        OnApply();
    } 
    private void LogCurrentMethod(int pass = 1)
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        System.Diagnostics.StackFrame currectFrame = stackTrace.GetFrame(1);
        System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(pass);

        string currectMethodName = currectFrame.GetMethod().Name;
        string currectClassName = currectFrame.GetMethod().DeclaringType.Name;

        string callMethodName = stackFrame.GetMethod().Name;
        string callClassName = stackFrame.GetMethod().DeclaringType.Name;

        Debug.Log("Current Method: " + currectClassName + "." + currectMethodName + " Call:" + callClassName + "." + callMethodName);
    }

    private void OnApply()
    {
        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif