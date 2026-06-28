using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SituationUI : MonoBehaviour
{
    public static SituationUI Instance;

    [Header("UI Elements")]
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;

    private SituationTrigger currentTrigger;

    private System.Action onClose;

    void Awake()
    {
        Instance = this;

        if (panel == null) Debug.LogError("panel is niet ingevuld!");
        if (titleText == null) Debug.LogError("titleText is niet ingevuld!");
        if (choicesContainer == null) Debug.LogError("choicesContainer is niet ingevuld!");
        if (choiceButtonPrefab == null) Debug.LogError("choiceButtonPrefab is niet ingevuld!");

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