using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Currencies")]
    public int mood = 100;
    public int energy = 100;
    public int waterLevel = 100;

    float timeLeft = 0.3f;

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


    private void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            mood -= 5;
            energy -= 5;
            timeLeft = .3f;
        }
    }
}
