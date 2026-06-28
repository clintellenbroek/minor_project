using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    public static bool isPaused = false;

    [Header("Currencies")]
    public int mood = 100;
    public int energy = 100;
    public int waterLevel = 100;
    public int totalWaterUsed = 0;

    public Dictionary<string, (string choiceText, float waterCost)> savedChoices 
            = new Dictionary<string, (string, float)>();

    public int energyDecreaseAmount = 1;
    public float energyDecreaseTime = 5f;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(DrainEnergy());
    }

    IEnumerator DrainEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(energyDecreaseTime);
            
            // Skip drain if game is paused
            if (!isPaused)
                DecreaseEnergy(energyDecreaseAmount);
        }
    }

    public void IncreaseMood(int amount)
    {
        mood = Mathf.Clamp(mood + amount, 0, 100);
        //mood += amount;

    }

    public void IncreaseEnergy(int amount)
    {
        energy = Mathf.Clamp(energy + amount, 0, 100);
    }

    public void DecreaseEnergy(int amount)
    {
        energy = Mathf.Clamp(energy - amount, 0, 100);

        if (energy <= 0)
        {
            // Get current day from DayCycleManager to show on game over screen
            int daysSurvived = FindObjectOfType<DayCycleManager>().currentDay;

            if (DayOverUI.Instance != null)
                DayOverUI.Instance.ShowPanel(isGameOver: true, daysSurvived: daysSurvived);
            else
                Debug.LogError("DayOverUI.Instance is null!");
        }
    }

    public void IncreaseWaterLevel(int amount)
    {
        waterLevel = Mathf.Clamp(waterLevel + amount, 0, 100);
        //waterLevel += amount;
    }
    public void IncreaseTotalWaterUsed(int amount)
    {
        totalWaterUsed += amount;
    }

    public void SaveChoice(string situationTitle, string choiceText, double waterCost)
    {
        savedChoices[situationTitle] = (choiceText, (float) waterCost);
    }

    public void ResetDay()
    {
        mood = 100;
        energy = 100;
        waterLevel = 100;
        totalWaterUsed = 0;
        savedChoices.Clear();
    }
}
