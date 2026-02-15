using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsUI : MonoBehaviour
{
    public float Timer = 10f;

    private float CurrentTimer = 0f;
    private TextMeshProUGUI Text;

    private InputAction moveAction;
    private InputAction flyAction;
    private InputAction lookAction;
    private InputAction interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        flyAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");

        Text = GetComponent<TextMeshProUGUI>();
        Text.text = "Look - " + "Mouse" + "/(" + lookAction.bindings[0].ToDisplayString() + ")\n"
            + "Move - " + "WASD" + "/(" + moveAction.bindings[0].ToDisplayString() + ")\n"
            + "Fly - " + flyAction.bindings[0].ToDisplayString() + "/(" + flyAction.bindings[1].ToDisplayString() + ")\n"
            + "Interact - " + interactAction.bindings[0].ToDisplayString() + "/(" + interactAction.bindings[1].ToDisplayString() + ")";
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTimer += Time.deltaTime;
        if (CurrentTimer > Timer)
        {
            gameObject.SetActive(false);
        }
    }
}
