using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string name;
    public Image icon;
}
