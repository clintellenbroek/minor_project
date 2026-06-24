using UnityEngine;

[CreateAssetMenu(fileName = "New Situation", menuName = "WaterWise/Situation")]
public class Situation : ScriptableObject
{
    public string title;
    [TextArea]
    public string description;

    public Choice[] choices = new Choice[4];
}