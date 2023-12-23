using InventorySystem;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private CharacterInventoryComponent inventoryComponent;
    [SerializeField] private ItemBase inventoryItem;
    [SerializeField] private int amount = 1;
    [SerializeField] private Vector2Int coord;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inventoryComponent.Service.Add(inventoryItem.ID, amount);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inventoryComponent.Service.Add(coord, inventoryItem.ID, amount);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inventoryComponent.Service.Remove(inventoryItem.ID, true, amount);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inventoryComponent.Service.Remove(coord, inventoryItem.ID, true, amount);
        }
    }
}
