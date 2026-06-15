using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Inventory : MonoBehaviour
{

    // Store items per slot
    Dictionary<InventoryItem, int> inventory = new();
    
    // Max slots for the inventory
    public int slots = 5;


    public void AddItem(InventoryItem item, int amount)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] = amount + inventory[item];
            return;
        }

        if (inventory.Keys.Count < slots)
            inventory.Add(item, amount);
    }

    public void RemoveItem(InventoryItem item)
    {
        if (inventory.ContainsKey(item)) 
            inventory.Remove(item);
    }
}