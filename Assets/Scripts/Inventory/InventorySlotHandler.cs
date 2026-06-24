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
}
