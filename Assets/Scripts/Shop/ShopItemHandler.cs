using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemHandler : MonoBehaviour
{
    [Header("Shop Item Settings")]
    public TMPro.TextMeshProUGUI itemNameText;
    public TMPro.TextMeshProUGUI itemDescriptionText;
    public TMPro.TextMeshProUGUI itemPriceText;
    public Image itemIcon;

    public Inventory inventory;
    public EconomyManager eco;

    private Shopitem shopItem;


    public void SetShopitem(Shopitem item)
    {
        shopItem = item;
        itemNameText.text = item.itemType.name;
        itemDescriptionText.text = item.description;
        itemPriceText.text = $"{item.price} {item.currency}";
        itemIcon.sprite = item.itemType.icon;
    }

    public void PurchaseItem()
    {
        Debug.Log(eco.energy);
        Debug.Log(eco.waterLevel);
        Debug.Log(eco.mood);
        Debug.Log(shopItem.price);
        if (shopItem.currency == "energy")
        {
            if (eco.energy <= shopItem.price)
            {
                StartCoroutine(TogglePriceTextColor());
                return;
            }

            eco.energy -= shopItem.price;
            inventory.AddItem(shopItem.itemType, shopItem.amount);
        }
        else if (shopItem.currency == "mood")
        {
            if (eco.mood <= shopItem.price)
            {
                StartCoroutine(TogglePriceTextColor()); 
                return;
            }


            eco.mood -= shopItem.price;
            inventory.AddItem(shopItem.itemType, shopItem.amount);
        }
        else if (shopItem.currency == "water")
        {
            if (eco.waterLevel <= shopItem.price)
            {
                StartCoroutine(TogglePriceTextColor()); 
                return;
            }


            eco.waterLevel -= shopItem.price;
            inventory.AddItem(shopItem.itemType, shopItem.amount);
        }
        else
        {
            StartCoroutine(TogglePriceTextColor());    
        }
    }


    IEnumerator TogglePriceTextColor()
    {
        itemPriceText.color = Color.red;
        yield return new WaitForSeconds(2f);
        itemPriceText.color = Color.black;
    }
}
