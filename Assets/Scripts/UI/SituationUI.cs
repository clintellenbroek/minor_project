using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SituationUI : MonoBehaviour
{
    public static SituationUI Instance;

    [Header("UI Elements")]
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;
    public TextMeshProUGUI notEnoughWaterText;

    private SituationTrigger currentTrigger;
    private Coroutine blinkCoroutine;
    private System.Action onClose;

    void Awake()
    {
        Instance = this;

        if (panel == null) Debug.LogError("panel is niet ingevuld!");
        if (titleText == null) Debug.LogError("titleText is niet ingevuld!");
        if (choicesContainer == null) Debug.LogError("choicesContainer is niet ingevuld!");
        if (choiceButtonPrefab == null) Debug.LogError("choiceButtonPrefab is niet ingevuld!");
        if (notEnoughWaterText != null) notEnoughWaterText.gameObject.SetActive(false);

        panel.SetActive(false);
    }

    public void ShowSituation(Situation situation, SituationTrigger trigger, System.Action onClose = null)
    {
        currentTrigger = trigger;
        string situationTitle = situation.title;
        this.onClose = onClose;
        panel.SetActive(true);
        titleText.text = situation.title;

        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(choicesContainer.GetChild(i).gameObject);
        }

        for (int i = 0; i < situation.choices.Length; i++)
        {
            var choice = situation.choices[i];
            GameObject btn = Instantiate(choiceButtonPrefab, choicesContainer);
            string moodText = choice.moodCost.ToString("+#;-#;0");
            btn.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{choice.text}\nWater: {choice.waterCost} | Mood: {moodText}";
            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (EconomyManager.Instance == null)
                {
                    Debug.LogError("EconomyManager.Instance is null!");
                    return;
                }
 
            if (EconomyManager.Instance.waterLevel < (int)choice.waterCost)
            {
                Debug.Log($"[SituationUI] Not enough water! Need {choice.waterCost}, have {EconomyManager.Instance.waterLevel}");
                notEnoughWaterText.text = $"Not enough water! You need {choice.waterCost}L but only have {EconomyManager.Instance.waterLevel}L left.";

                // Stop previous blink if still running
                if (blinkCoroutine != null)
                    StopCoroutine(blinkCoroutine);

                blinkCoroutine = StartCoroutine(BlinkText(duration: 2f, blinkSpeed: 0.3f));
                return;
            }

                EconomyManager.Instance.IncreaseWaterLevel(-(int)choice.waterCost);
                EconomyManager.Instance.IncreaseTotalWaterUsed((int)choice.waterCost);
                EconomyManager.Instance.IncreaseMood((int)choice.moodCost);
                EconomyManager.Instance.SaveChoice(situation.title, choice.text, choice.waterCost);
                OnChoiceSelected(index, choice);
            });
        }
    }

    void OnChoiceSelected(int index, Choice choice)
    {
        panel.SetActive(false);

        if (currentTrigger != null)
            currentTrigger.PlayEffects();

        onClose?.Invoke();
    }

    //void OnChoiceSelected(int index, Choice choice)
    //{
    //    panel.SetActive(false);
    //    onClose?.Invoke();
    //}

    IEnumerator BlinkText(float duration, float blinkSpeed = 0.3f)
    {
        float elapsed = 0f;
        notEnoughWaterText.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            notEnoughWaterText.enabled = !notEnoughWaterText.enabled; // Toggle visibility
            yield return new WaitForSeconds(blinkSpeed);
            elapsed += blinkSpeed;
        }

        // Make sure text is hidden after blinking
        notEnoughWaterText.enabled = true;
        notEnoughWaterText.gameObject.SetActive(false);
        blinkCoroutine = null;
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        titleText.text = "";
        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(choicesContainer.GetChild(i).gameObject);
        }
        onClose?.Invoke();
    }
}