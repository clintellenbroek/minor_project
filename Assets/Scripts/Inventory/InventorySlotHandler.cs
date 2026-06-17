using UnityEngine;
using UnityEngine.UI;

public class InventorySlotHandler : MonoBehaviour
{
    public GameObject selectedGameObject;
    public InventoryItem selectedItem;

    public Image icon;


    public void Select()
    {
        selectedGameObject.SetActive(true);
    }

    public void SetItem(InventoryItem item)
    {
        selectedItem = item;
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
    }

    public void UnsetItem()
    {
        selectedItem = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
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
