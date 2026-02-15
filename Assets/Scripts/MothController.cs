using UnityEngine;
using UnityEngine.InputSystem;

public class MothController : MonoBehaviour
{
    private CharacterController controller;

    private float VerticalSpeed = 0f;
    private Vector2 HorizontalMovement = new Vector2();
    private Vector2 CameraRotation = new Vector2();
    private float InteractionCooldown = 0f;

    private InputAction moveAction;
    private InputAction flyAction;
    private InputAction lookAction;
    private InputAction interactAction;

    private GameObject GrabbedObject;

    private float TimeToStart = 0.5f;

    public float flyingSpeed = 5f;
    public float groundedSpeed = 1f;
    //public float movementDecay = 20f;
    public float maxAscendSpeed = 5f;
    public float maxFallSpeed = 2f;
    public float verticalAcceleration = 20f;
    public float gravity = 9.81f;
    public float mouseSensitivity = 10f;

    public GameObject AttachPoint;

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
        interactAction = InputSystem.actions.FindAction("Interact");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeToStart > 0f)
        {
            // Look the input for a bit to avoid accidental camera movement while game is loading
            TimeToStart -= Time.deltaTime;
            return;
        }
        UpdateCamera();
        UpdateMovement();
        UpdateGabbedItem();
    }

    void UpdateCamera()
    {
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

        /* TODO investigate this
        // Slow down behaviour
        if (movementInput == Vector2.zero && HorizontalMovement.sqrMagnitude > 0f)
        {
            Vector2 Slowdown = HorizontalMovement.normalized * Time.deltaTime * movementDecay;
            if (Slowdown.sqrMagnitude > HorizontalMovement.sqrMagnitude) 
            {
                movement = Vector3.zero;
            }
            else
            {
                movement = HorizontalMovement - Slowdown;
            }
            Debug.Log(HorizontalMovement);
            Debug.Log(Slowdown);
            Debug.Log(movement);
        }
        */
        HorizontalMovement = movement;

        // Apply movement
        movement.y = VerticalSpeed;
        controller.Move(movement * Time.deltaTime);
    }

    void UpdateGabbedItem()
    {
        if (InteractionCooldown > 0f)
        {
            InteractionCooldown -= Time.deltaTime;
            InteractionCooldown = Mathf.Max(0, InteractionCooldown);
        }

        bool bInteactionPressed = interactAction.IsPressed();
        if (bInteactionPressed)
        {
            ReleaseObject();
        }
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Hittable HittableComponment = hit.transform.GetComponentInChildren<Hittable>();
        if (HittableComponment)
        {
            HittableComponment.OnHit(gameObject);
        }
    }

    public bool Grab(GameObject Object)
    {
        if (GrabbedObject != null)
        {
            return false;
        }

        if (InteractionCooldown > 0f)
        {
            return false;
        }

        Object.transform.SetParent(AttachPoint.transform);
        Object.transform.localRotation = Quaternion.identity;
        Object.transform.localPosition = Vector3.zero;
        Rigidbody GrabbedRigidBody = Object.GetComponent<Rigidbody>();
        if (GrabbedRigidBody != null)
        {
            GrabbedRigidBody.isKinematic = true;
        }
        Collider GrabbedCollider = Object.GetComponent<Collider>();
        if (GrabbedCollider != null)
        {
            Object.transform.localPosition = new Vector3(0f, -GrabbedCollider.bounds.extents.y, 0f);
            GrabbedCollider.enabled = false;
        }
        GrabbedObject = Object;
        InteractionCooldown = 0.5f;
        return true;
    }

    bool ReleaseObject()
    {
        // TODO we might need additional valid checks, i.e. not grounded
        if (GrabbedObject == null)
        {
            return false;
        }

        if (InteractionCooldown > 0f)
        {
            return false;
        }

        GrabbedObject.transform.SetParent(null);
        Rigidbody GrabbedRigidBody = GrabbedObject.GetComponent<Rigidbody>();
        if (GrabbedRigidBody != null)
        {
            GrabbedRigidBody.isKinematic = false;
        }
        Collider GrabbedCollider = GrabbedObject.GetComponent<Collider>();
        if (GrabbedCollider != null)
        {
            GrabbedCollider.enabled = true;
        }
        Interactable GrabbedInteract = GrabbedObject.GetComponentInChildren<Interactable>(true);
        if (GrabbedInteract)
        {
            GrabbedInteract.gameObject.SetActive(true);
        }
        GrabbedObject = null;
        InteractionCooldown = 0.5f;
        return true;
    }
}
