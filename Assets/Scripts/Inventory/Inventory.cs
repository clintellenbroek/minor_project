using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Inventory : MonoBehaviour
{

    // Store items per slot
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
    public EconomyManager economyManager;

    public InputAction useItemAction;


    public void Start()
    {
        for (int i = 0; i < slots; i++)
        {
            GameObject obj = Instantiate(InventorySlotPrefab, inventoryHolder.transform);
            inventorySlots[i] = obj.GetComponent<InventorySlotHandler>();
        }

        inventorySlots[0].Select();

        AddItem(starterItem, 1);
        useItemAction.Enable();

        useItemAction.performed += ctx =>
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.IsSelected() && slot.selectedItem != null)
                {
                    slot.Use(economyManager);
                    break;
                }
            }
        };
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

    public bool HasItem(InventoryItem item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.selectedItem == item)
                return true;
        }
        return false;
    }


    public void AddItem(InventoryItem item, int amount)
    {
        if (HasItem(item))
        {
            for (int i = 0; i < slots; i++)
            {
                if (inventorySlots[i].selectedItem == item)
                {
                    inventorySlots[i].SetItemCount(inventorySlots[i].itemCount + amount);
                    break;
                }
            }
            return;
        }

        for (int i = 0; i < slots; i++)
        {
            if (inventorySlots[i].selectedItem == null)
            {
                inventorySlots[i].SetItem(item, amount);
                break;
            }
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        for (int i = 0; i < slots; i++)
        {
            if (inventorySlots[i].selectedItem == item)
            {
                inventorySlots[i].UnsetItem();
                break;
            }
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
        if (!HasItem(item))
            return false;

        for (int i = 0; i < slots; i++)
        {
            if (inventorySlots[i].selectedItem == item)
            {
                if (inventorySlots[i].itemCount >= amount)
                {
                    inventorySlots[i].SetItemCount(inventorySlots[i].itemCount - amount);
                    if (inventorySlots[i].itemCount == 0)
                    {
                        RemoveItem(item);
                    }
                    return true;
                }
            }
        }
        return false;
    }
}