using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class StarterDialogue : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject camera;
    public InputAction inputAction;

    public List<GameObject> objectsToStartOnEnable = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController.gameObject.SetActive(false);
        inputAction.Enable();

        inputAction.performed += ctx =>
        {
            playerController.gameObject.SetActive(true);
            camera.gameObject.SetActive(true);
            foreach (var obj in objectsToStartOnEnable)
            {
                obj.SetActive(true);
            }
            gameObject.SetActive(false);
            inputAction.Disable();
        };
    }
}
