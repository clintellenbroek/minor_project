using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class SituationTrigger : MonoBehaviour
{
    public Situation situation;
    public GameObject interactPrompt;

    public AudioSource soundEffect;
    public ParticleSystem particleEffect;
    public float effectDuration = 3f;

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
        SituationUI.Instance.ShowSituation(situation, this);
    }

    public void PlayEffects()
    {
        if (soundEffect != null)
            soundEffect.Play();
            StartCoroutine(StopAudioAfterDelay(effectDuration));

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