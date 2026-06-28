using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SituationTrigger : MonoBehaviour
{
    public Situation situation;
    public GameObject interactPrompt;

    public AudioSource soundEffect;
    public ParticleSystem particleEffect;
    public float effectDuration = 3f;

    private bool playerInRange = false;
    private bool hasTriggeredToday = false; // Tracks if already triggered this day

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() == null) return;
        playerInRange = true;

        // Only show prompt if not yet triggered today
        if (!hasTriggeredToday)
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
        if (playerInRange && !hasTriggeredToday && Keyboard.current.eKey.wasPressedThisFrame)
        {
            hasTriggeredToday = true;
            interactPrompt.SetActive(false);
            TriggerSituation();
        }
    }

    void TriggerSituation()
    {
        SituationUI.Instance.ShowSituation(situation, this);
    }

    // Called by DayCycleManager at the start of each new day
    public void ResetForNewDay()
    {
        hasTriggeredToday = false;
    }

    public void PlayEffects()
    {
        if (soundEffect != null)
        {
            soundEffect.Play();
            StartCoroutine(StopAudioAfterDelay(effectDuration));
        }

        if (particleEffect != null)
        {
            particleEffect.Play();
            StartCoroutine(StopParticlesAfterDelay(effectDuration));
        }
    }

    IEnumerator StopParticlesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (particleEffect != null)
            particleEffect.Stop();
    }

    IEnumerator StopAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (soundEffect != null)
            soundEffect.Stop();
    }
}