using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("UI Togglers")]
    public List<UITogglers> uiTogglers;
    // Update is called once per frame

    private void Start()
    { 
        foreach (var inputAction in uiTogglers)
        {
            inputAction.inputAction.Enable();
            inputAction.inputAction.performed += ctx =>
            {
                GameObject uiElement = inputAction.uiElement;
                uiElement.SetActive(!uiElement.activeSelf);

                foreach (var element in inputAction.elementsToDisble)
                {
                    if (element != uiElement)
                    {
                        element.SetActive(!uiElement.activeSelf);
                    }
                }
            };
        }
    }
}


[Serializable]
public class UITogglers 
{
    public InputAction inputAction;
    public List<GameObject> elementsToDisble;
    public GameObject uiElement;
}
