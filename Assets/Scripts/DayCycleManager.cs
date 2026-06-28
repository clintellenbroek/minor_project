using UnityEngine;
using TMPro;

public class DayCycleManager : MonoBehaviour
{
    [Header("Day Settings")]
    public float dayDurationSeconds = 120f;
    public Light directionalLight;

    private float currentTime = 0f;
    public int currentDay = 1;
    private bool dayEnded = false;

    void Start()
    {
        currentTime = 0.25f * dayDurationSeconds;
    }

    void Update()
    {
        if (dayEnded) return;
        if (EconomyManager.isPaused) return;

        currentTime += Time.deltaTime;

        float normalizedTime = currentTime / dayDurationSeconds;
        float sunAngle = (normalizedTime * 360f) - 90f;
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0f);

        float intensity = Mathf.Clamp01(Mathf.Sin(normalizedTime * Mathf.PI * 2f) + 0.1f);
        directionalLight.intensity = intensity * 2f;

        if (currentTime >= dayDurationSeconds)
        {
            EndDay();
        }
    }

    void EndDay()
    {
        dayEnded = true;

        if (DayOverUI.Instance != null)
            DayOverUI.Instance.ShowPanel(isGameOver: false, daysSurvived: currentDay);
        else
            Debug.LogError("[DayCycleManager] DayOverUI.Instance is null!");

        Debug.Log($"[DayCycleManager] Day {currentDay} ended.");
    }

    public void StartNextDay()
    {
        currentDay++;
        currentTime = 0.25f * dayDurationSeconds;
        dayEnded = false;

        // Reset all situation triggers for the new day
        SituationTrigger[] allTriggers = FindObjectsOfType<SituationTrigger>();
        foreach (SituationTrigger trigger in allTriggers)
        {
            trigger.ResetForNewDay();
        }

        if (EconomyManager.Instance != null)
            EconomyManager.Instance.ResetDay();

        if (DayOverUI.Instance != null)
            DayOverUI.Instance.ClosePanel();

        Debug.Log($"[DayCycleManager] Day {currentDay} started — {allTriggers.Length} triggers reset.");
    }
}