using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Header("Buttons")]
    public Button nextDayButton;
    public Button returnToMenuButton;

    [Header("References")]
    public PlayerController playerController;


    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    void Awake()
    {
        Instance = this;

        if (dayOverPanel == null) Debug.LogError("dayOverPanel is not assigned!");
        if (titleText == null) Debug.LogError("titleText is not assigned!");
        if (choicesTitleText == null) Debug.LogError("choicesTitleText is not assigned!");
        if (choicesContentText == null) Debug.LogError("choicesContentText is not assigned!");
        if (totalWaterUsedTitle == null) Debug.LogError("totalWaterUsedTitle is not assigned!");
        if (totalWaterUsedContentText == null) Debug.LogError("totalWaterUsedContentText is not assigned!");
        if (nextDayButton == null) Debug.LogError("nextDayButton is not assigned!");
        if (returnToMenuButton == null) Debug.LogError("returnToMenuButton is not assigned!");

        dayOverPanel.SetActive(false);
    }

    void Start()
    {
        if (playerController != null)
        {
            playerStartPosition = playerController.transform.position;
            playerStartRotation = playerController.transform.rotation;
            Debug.Log($"[DayOverUI] Player start position saved: {playerStartPosition}");
        }
    }

    public void ShowPanel(bool isGameOver, int daysSurvived = 0)
    {
        EconomyManager.isPaused = true;

        if (playerController != null)
            playerController.enabled = false;
        else
            Debug.LogWarning("[DayOverUI] playerController is not assigned!");

        dayOverPanel.SetActive(true);

        if (isGameOver)
        {
            titleText.text = "Game Over";
            choicesTitleText.text = "Days Survived: " + daysSurvived;
            choicesContentText.gameObject.SetActive(false);
            nextDayButton.gameObject.SetActive(false);
            returnToMenuButton.gameObject.SetActive(true);
        }
        else
        {
            titleText.text = "Day Overview";
            choicesTitleText.gameObject.SetActive(true);
            choicesTitleText.text = $"Your choices — Day {daysSurvived}:"; // Day number included
            choicesContentText.gameObject.SetActive(true);

            if (EconomyManager.Instance.savedChoices.Count == 0)
            {
                choicesContentText.text = "No choices made.";
            }
            else
            {
                string overview = "";
                foreach (var entry in EconomyManager.Instance.savedChoices)
                {
                    overview += $"• {entry.Key} → {entry.Value.choiceText} ({entry.Value.waterCost} water)\n";
                }
                choicesContentText.text = overview;
            }

            nextDayButton.gameObject.SetActive(true);
            returnToMenuButton.gameObject.SetActive(false);
        }

        totalWaterUsedTitle.text = "Total water used:";
        totalWaterUsedContentText.text = EconomyManager.Instance.totalWaterUsed + "L";

        Debug.Log($"[DayOverUI] Panel opened — isGameOver: {isGameOver}");
    }

    public void ClosePanel()
    {
        EconomyManager.isPaused = false;

        if (playerController != null)
            playerController.enabled = true;

        dayOverPanel.SetActive(false);
        Debug.Log("[DayOverUI] Panel closed.");
    }

    private void ResetPlayer()
    {
        if (playerController == null) return;

        CharacterController cc = playerController.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        playerController.transform.position = playerStartPosition;
        playerController.transform.rotation = playerStartRotation;

        if (cc != null) cc.enabled = true;

        Debug.Log("[DayOverUI] Player reset to original start position.");
    }

    public void OnNextDayButtonClicked()
    {
        EconomyManager.Instance.ResetDay();
        ResetPlayer();
        
        // Tell DayCycleManager to start the next day
        DayCycleManager dayCycle = FindObjectOfType<DayCycleManager>();
        if (dayCycle != null)
            dayCycle.StartNextDay();
        else
            Debug.LogError("[DayOverUI] DayCycleManager not found!");

        ClosePanel();
    }

    public void OnReturnToMenuButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Debug.Log("[DayOverUI] Returning to main menu.");
    }

}