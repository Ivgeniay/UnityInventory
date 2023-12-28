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
            inventoryComponent.Service.Add(inventoryItem, amount);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inventoryComponent.Service.Add(coord, inventoryItem, amount);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                inventoryComponent.Service.RemoveLast(inventoryItem, true, amount); 
            }
            else
            {
                inventoryComponent.Service.RemoveFirst(inventoryItem, true, amount);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inventoryComponent.Service.Remove(coord, inventoryItem, true, amount);
        }
    }
}
