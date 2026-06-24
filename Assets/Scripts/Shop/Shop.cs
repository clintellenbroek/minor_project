using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Shop Settings")]
    public GameObject contentContainer;
    public GameObject shopItemPrefab;
    public Inventory inventory;
    public EconomyManager economyManager;

    [Header("Shop Items")]
    public List<Shopitem> shopItems = new();


    private void Start()
    { 
        foreach(var shopItem in shopItems)
        {
            GameObject obj = Instantiate(shopItemPrefab, contentContainer.transform);
            ShopItemHandler handler = obj.GetComponent<ShopItemHandler>();
            handler.SetShopitem(shopItem);
            handler.inventory = inventory;
            handler.eco = economyManager;
        }
    }
}

