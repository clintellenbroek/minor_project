using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider volumeSlider;
    public string exposedParameter;

    private void Start()
    {
        mixer.GetFloat(exposedParameter, out float value);
        volumeSlider.value = Mathf.Pow(10, value / 20);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        // Slider gaat van 0.0001 tot 1
        mixer.SetFloat(exposedParameter, Mathf.Log10(value) * 20);
    }
}