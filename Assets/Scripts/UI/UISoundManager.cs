using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance;

    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    void Awake()
    {
        Instance = this;
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}
