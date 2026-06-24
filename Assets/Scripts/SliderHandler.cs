using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [Header("Slider Object")]
    public Slider slider;

    public EconomyManager economyManager;
    public String propertyName;


    private FieldInfo field;

    private void Awake()
    {
        field = economyManager.GetType().GetField(propertyName);
    }

    public void Update()
    {
        slider.value = (int)field.GetValue(economyManager);
    }
}
