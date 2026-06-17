using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Controller Settings")]
    public float playerSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    public float sensitivity = 2f;

    [Header("Object References")]
    public CharacterController controller;
    public Transform playerParent;
    public Transform cameraPivot;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference sprintAction;
    public InputActionReference jumpAction;
    public InputActionReference lookAction;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float pitch = 0f;


    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        lookAction.action.Disable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < -2f)
        {
            playerVelocity.y = -2f;
        }

        // ===== LOOK =====
        Vector2 look = lookAction.action.ReadValue<Vector2>();

        // Horizontal rotation
        playerParent.Rotate(Vector3.up * look.x * sensitivity);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);

        // ===== MOVE =====
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Debug.Log(input);

        Vector3 move =
            -playerParent.forward * input.y +
            -playerParent.right * input.x;

        move = Vector3.ClampMagnitude(move, 1f);

        if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            playerVelocity.y =
                Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        float speed = playerSpeed;
        if (sprintAction.action.IsPressed())
            speed = sprintSpeed;

        Vector3 finalMove =
            move * speed +
            Vector3.up * playerVelocity.y;

        controller.Move(finalMove * Time.deltaTime);
    }
}