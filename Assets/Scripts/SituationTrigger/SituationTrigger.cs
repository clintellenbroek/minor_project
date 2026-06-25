using UnityEngine;
using UnityEngine.InputSystem;

public class SituationTrigger : MonoBehaviour
{
    public Situation situation;
    public InputAction interactAction;
    public GameObject interactPrompt; // sleep InteractPrompt hier naartoe in Inspector

    private bool hasTriggered = false;
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
        interactPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        interactPrompt.SetActive(false);
    }

    void OnEnable()
    {
        interactAction.Enable();
        interactAction.performed += ctx =>
        {
            if (playerInRange && !hasTriggered)
            {
                hasTriggered = true;
                interactPrompt.SetActive(false);
                TriggerSituation();
            }
        };
    }

    void OnDisable()
    {
        interactAction.performed -= ctx => { };
        interactAction.Disable();
    }

    void TriggerSituation()
    {
        Debug.Log("Vraag: " + situation.title);
        Debug.Log("Beschrijving: " + situation.description);

        for (int i = 0; i < situation.choices.Length; i++)
        {
            Debug.Log(situation.choices[i].text + " kost " + situation.choices[i].waterCost + " water");
        }
    }
}