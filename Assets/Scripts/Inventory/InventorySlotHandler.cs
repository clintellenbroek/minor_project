using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotHandler : MonoBehaviour
{
    public GameObject selectedGameObject;
    public InventoryItem selectedItem;
    public TMP_Text itemCountText;

    public int itemCount = 0;
    public Image icon;


    public void Select()
    {
        selectedGameObject.SetActive(true);
    }

    public void SetItem(InventoryItem item, int amount)
    {
        selectedItem = item;
        itemCountText.text = amount.ToString();
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        itemCount = amount;
    }

    public void UnsetItem()
    {
        selectedItem = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        itemCount = 0;
        itemCountText.text = "";
    }

    public void SetItemCount(int count)
    {
        itemCount = count;
        itemCountText.text = count.ToString();
    }

    public void Toggle()
    {
        selectedGameObject.SetActive(!selectedGameObject.activeSelf);
    }

    public void Deselect()
    {
        selectedGameObject.SetActive(false);
    }

    public bool IsSelected()
    {
        return selectedGameObject.activeSelf;
    }

    public void Use(EconomyManager eco)
    {
        eco.mood = Mathf.Clamp(eco.mood + selectedItem.mood, 0, 100);
        eco.energy = Mathf.Clamp(eco.energy + selectedItem.energy, 0, 100);
        SetItemCount(itemCount - 1);
        if (itemCount <= 0)
        {
            UnsetItem();
        }
    }
}
