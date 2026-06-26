using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;

public class GainSlider : MonoBehaviour
{
    public CinemachineInputAxisController inputAxisController;
    public Slider slider;

    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        var controllers = inputAxisController.Controllers;

        for (int i = 0; i < controllers.Count; i++)
        {
            var c = controllers[i];
            if (c.Name == "Look Orbit X")
            {
                c.Input.Gain = value;
                controllers[i] = c; // struct, dus terugschrijven
            }
        }
    }

    void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderChanged);
    }
}