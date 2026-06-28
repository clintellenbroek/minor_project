using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "InventoryItem", menuName = "Waterwise/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string name;
    public Sprite icon;
    public int energyIncrease;
    public int moodIncrease;
}
