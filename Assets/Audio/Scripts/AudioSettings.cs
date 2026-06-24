using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider volumeSlider;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        // Slider gaat van 0.0001 tot 1
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }
}