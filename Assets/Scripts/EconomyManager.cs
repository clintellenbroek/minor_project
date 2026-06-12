using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Currencies")]
    public int mood = 100;
    public int energy = 100;
    public int waterLevel = 100;

    public void IncreaseMood(int amount)
    {
        mood += amount;
    }

    public void IncreaseEnergy(int amount)
    {
        energy += amount;
    }

    public void IncreaseWaterLevel(int amount)
    {
        waterLevel += amount;
    }
}
