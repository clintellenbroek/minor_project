using UnityEngine;
using TMPro;

public class DayOverUI : MonoBehaviour
{
    public static DayOverUI Instance;

    [Header("UI Elements")]
    public GameObject dayOverPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI choicesTitleText;
    public TextMeshProUGUI choicesContentText;
    public TextMeshProUGUI totalWaterUsedTitle;
    public TextMeshProUGUI totalWaterUsedContentText;
    public PlayerController playerController;

    void Awake()
    {
        Instance = this;

        if (dayOverPanel == null) Debug.LogError("dayOverPanel is not assigned!");
        if (titleText == null) Debug.LogError("titleText is not assigned!");
        if (choicesTitleText == null) Debug.LogError("choicesTitleText is not assigned!");
        if (choicesContentText == null) Debug.LogError("choicesContentText is not assigned!");
        if (totalWaterUsedTitle == null) Debug.LogError("totalWaterUsedTitle is not assigned!");
        if (totalWaterUsedContentText == null) Debug.LogError("totalWaterUsedContentText is not assigned!");

        dayOverPanel.SetActive(false);
    }

    // isGameOver true = energy reached 0, false = normal day overview
    public void ShowPanel(bool isGameOver)
    {
        if (playerController != null)
            playerController.enabled = false;
        else
            Debug.LogWarning("[DayOverUI] playerController is not assigned!");

        dayOverPanel.SetActive(true);

        if (isGameOver)
        {
            // Game over — only show title and total water used, no choices
            titleText.text = "Game Over";
            choicesTitleText.gameObject.SetActive(false);
            choicesContentText.gameObject.SetActive(false);
        }
        else
        {
            // Day overview — show everything including choices
            titleText.text = "Day Overview";
            choicesTitleText.gameObject.SetActive(true);
            choicesContentText.gameObject.SetActive(true);
            choicesTitleText.text = "Your choices:";

            // Build the choices overview from savedChoices
            if (EconomyManager.Instance.savedChoices.Count == 0)
            {
                choicesContentText.text = "No choices made.";
            }
            else
            {
                string overview = "";
                foreach (var entry in EconomyManager.Instance.savedChoices)
                {
                    overview += $"• {entry.Key}\n  → {entry.Value.choiceText} ({entry.Value.waterCost} water)\n\n";
                }
                choicesContentText.text = overview;
            }
        }

        totalWaterUsedTitle.text = "Total water used:";
        totalWaterUsedContentText.text = EconomyManager.Instance.totalWaterUsed + "L";

        Debug.Log($"[DayOverUI] Panel opened — isGameOver: {isGameOver}");
    }

    public void ClosePanel()
    {
        dayOverPanel.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        Debug.Log("[DayOverUI] Panel closed.");
    }


    // normale dag afsluiten
    // DayOverUI.Instance.ShowPanel(isGameOver: false); // Day overview with choices
}