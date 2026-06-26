using UnityEngine;
using UnityEngine.InputSystem;

public class SituationTrigger : MonoBehaviour
{
    public Situation situation;
    public GameObject interactPrompt;

    // private bool hasTriggered = false;
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() == null) return;
        playerInRange = true;
        interactPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() == null) return;
        playerInRange = false;
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        // if (playerInRange && !hasTriggered && Keyboard.current.eKey.wasPressedThisFrame)
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // hasTriggered = true;
            interactPrompt.SetActive(false);
            TriggerSituation();
        }
    }

    void TriggerSituation()
    {
        SituationUI.Instance.ShowSituation(situation);
    }
}