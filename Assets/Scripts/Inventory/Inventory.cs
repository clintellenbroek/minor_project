using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Inventory : MonoBehaviour
{

    // Store items per slot
    Dictionary<InventoryItem, int> inventory = new();
    InventorySlotHandler[] inventorySlots = new InventorySlotHandler[5];

    // Max slots for the inventory
    [Header("Inventory Settings")]
    public int slots = 5;

    [Header("UI")]
    public GameObject inventoryHolder;
    public GameObject InventorySlotPrefab;

    // Keep track of the inventory 
    private Keyboard keyboard = Keyboard.current;

    public InventoryItem starterItem;


    public void Start()
    {
        for (int i = 0; i < slots; i++)
        {
            GameObject obj = Instantiate(InventorySlotPrefab, inventoryHolder.transform);
            inventorySlots[i] = obj.GetComponent<InventorySlotHandler>();
        }

        inventorySlots[0].Select();

        AddItem(starterItem, 1);
    }

    public void Update()
    {
        if (keyboard.digit1Key.wasPressedThisFrame) SelectSlot(0);
        if (keyboard.digit2Key.wasPressedThisFrame) SelectSlot(1);
        if (keyboard.digit3Key.wasPressedThisFrame) SelectSlot(2);
        if (keyboard.digit4Key.wasPressedThisFrame) SelectSlot(3);
        if (keyboard.digit5Key.wasPressedThisFrame) SelectSlot(4);
    }

    public void SelectSlot(int index)
    {
       if (index < 0 || index >= slots)
            return;
        for (int i = 0; i < slots; i++)
        {
            if (i == index)
                inventorySlots[i].Select();
            else
                inventorySlots[i].Deselect();
        }
    }


    public void AddItem(InventoryItem item, int amount)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] = amount + inventory[item];
            return;
        }

        if (inventory.Keys.Count < slots)
        {
            inventory.Add(item, amount);
            for (int i = 0; i < slots; i++)
            {
                if (inventorySlots[i].selectedItem == null)
                {
                    inventorySlots[i].SetItem(item);
                    break;
                }
            }
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        if (inventory.ContainsKey(item))
        {
            for (int i = 0; i < slots; i++)
            {
                if (inventorySlots[i].selectedItem == item)
                {
                    inventorySlots[i].UnsetItem();
                    break;
                }
            }
            inventory.Remove(item);
        }
    }

    /**
     * Take items from the inventory
     * @param item The item to take from the inventory
     * @param amount The amount of items to take from the inventory
     * @returns true if the item was taken, false if not enough items were present
     */
    public bool TakeItem(InventoryItem item, int amount)
    {
        if (inventory.ContainsKey(item))
        {
            if (inventory[item] >= amount)
            {
                inventory[item] -= amount;
                if (inventory[item] == 0)
                {
                    RemoveItem(item);
                }
                return true;
            }
        }
        return false;
    }

    public int HasItem(InventoryItem item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }
}