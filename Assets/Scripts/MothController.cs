using UnityEngine;
using UnityEngine.InputSystem;

public class MothController : MonoBehaviour
{
    private CharacterController controller;

    private float VerticalSpeed;
    private Vector2 CameraRotation = new Vector2();

    private InputAction moveAction;
    private InputAction flyAction;
    private InputAction lookAction;

    public float flyingSpeed = 5f;
    public float groundedSpeed = 1f;
    public float maxAscendSpeed = 5f;
    public float maxFallSpeed = 2f;
    public float verticalAcceleration = 20f;
    public float gravity = 9.81f;
    public float mouseSensitivity = 10f;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        flyAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
        UpdateMovement();
    }

    void UpdateCamera()
    {
        // TODO maybe we should delay reading the input, since we can get big initial deltas when pressing play, although on final builds it might not be an issue.
        // Read input
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        lookInput = lookInput * mouseSensitivity * Time.deltaTime;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x - lookInput.y, -85f, 85f);
        CameraRotation.y = CameraRotation.y + lookInput.x;

        // Look at
        transform.localRotation = Quaternion.Euler(CameraRotation);
    }

    void UpdateMovement()
    {
        // Read input
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        bool bIsFlying = flyAction.IsPressed();

        // Transform input into absolute coordinates
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y);
        movement.Normalize();
        movement = transform.TransformDirection(movement);

        // Flying behaviour
        if (bIsFlying)
        {
            VerticalSpeed += verticalAcceleration * Time.deltaTime;
            VerticalSpeed = Mathf.Min(maxAscendSpeed, VerticalSpeed);
        }
        else
        {
            VerticalSpeed -= gravity * Time.deltaTime;
            VerticalSpeed = Mathf.Max(-maxFallSpeed, VerticalSpeed);
        }

        // Grounded behaviour
        if (controller.isGrounded)
        {
            VerticalSpeed = 0f;
            movement *= groundedSpeed;
        }
        else
        {
            movement *= flyingSpeed;
        }

        // Apply movement
        movement.y = VerticalSpeed;
        controller.Move(movement * Time.deltaTime);
    }
}
