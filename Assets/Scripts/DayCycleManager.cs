using UnityEngine;
using TMPro;

public class DayCycleManager : MonoBehaviour
{
    [Header("Dag instellingen")]
    public float dayDurationSeconds = 120f;
    public Light directionalLight;

    [Header("UI")]
    public GameObject dayEndScreen;
    public TextMeshProUGUI dayText;

    private float currentTime = 0f;
    private int currentDay = 1;
    private bool dayEnded = false;

    void Start()
    {
        currentTime = 0.25f * dayDurationSeconds;
        dayEndScreen.SetActive(false);
    }

    void Update()
    {
        if (dayEnded) return;

        currentTime += Time.deltaTime;

        float normalizedTime = currentTime / dayDurationSeconds;
        //float sunAngle = (normalizedTime * 360f) - 90f;
        float sunAngle = Mathf.Lerp(15f, 170f, normalizedTime);
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0f);

        //float intensity = Mathf.Clamp01(Mathf.Sin(normalizedTime * Mathf.PI * 2f) + 0.1f);
        //directionalLight.intensity = intensity * 2f;

        float intensity = Mathf.Lerp(2f, 0.2f, normalizedTime);
        directionalLight.intensity = intensity;

        if (currentTime >= dayDurationSeconds)
        {
            EndDay();
        }
    }

    void EndDay()
    {
        dayEnded = true;
        dayEndScreen.SetActive(true);
        dayText.text = "Dag " + currentDay + " voorbij!";
    }

    public void StartNextDay()
    {
        currentDay++;
        currentTime = 0.25f * dayDurationSeconds;
        dayEnded = false;
        dayEndScreen.SetActive(false);
    }
}