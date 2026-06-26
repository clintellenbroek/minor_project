using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider sensitivitySlider;

    public static float sensitivity = 1f;

    private void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);

        sensitivitySlider.value = sensitivity;

        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;

        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }
}