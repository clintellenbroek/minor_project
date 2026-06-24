using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Controller Settings")]
    public float playerSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    public float rotationSpeed = 12f;

    [Header("Object References")]
    public CharacterController controller;
    public Transform playerParent;
    public Transform cameraTransform;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference sprintAction;
    public InputActionReference jumpAction;

    [Header("Player Effects")]
    public List<AudioClip> walkingSounds = new();
    public Animator animator;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private void OnEnable()
    {
        moveAction.action.Enable();
        sprintAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        sprintAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        animator.SetBool("Grounded", groundedPlayer);

        if (groundedPlayer && playerVelocity.y < -2f)
        {
            playerVelocity.y = -2f;
        }

        // ===== INPUT =====
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // ===== CAMERA-RELATIVE DIRECTION =====
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection =
            camForward * input.y +
            camRight * input.x;

        // ===== ROTATION (SNAPS TO DIRECTION) =====
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            playerParent.rotation = Quaternion.Slerp(
                playerParent.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // ===== SPEED =====
        float speed = sprintAction.action.IsPressed()
            ? sprintSpeed
            : playerSpeed;

        Vector3 horizontalMove =
            moveDirection.normalized * speed;

        // ===== JUMP =====
        if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            playerVelocity.y =
                Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // ===== GRAVITY =====
        playerVelocity.y += gravityValue * Time.deltaTime;

        // ===== FINAL MOVE =====
        Vector3 finalMove =
            horizontalMove +
            Vector3.up * playerVelocity.y;

        controller.Move(finalMove * Time.deltaTime);

        // ===== ANIMATION =====
        animator.SetFloat("MoveSpeed", moveDirection.magnitude);
    }
}