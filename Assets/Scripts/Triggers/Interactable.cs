using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    private InputAction interactAction;
    private TextMeshPro interactableInputText;
    private float currentCooldown = 0f;
    private GameObject InteractUIPrompt;
    private TextMeshProUGUI PromptText;
    private bool SetUIPrompt = false;

    public GameObject inputTextObject;

    [Tooltip("Object that will received the effects of the trigger. By default it will be Activated/Deactivated. Behaviour can be overriden (WIP). If left empty it will target it parent object.")]
    public GameObject ObjectTrigger;
    [Tooltip("Deactivate trigger after it has been triggered.")]
    public bool SingleTrigger = true;
    [Tooltip("Minimum time between interactions. Only used if not Single Trigger.")]
    public float Cooldown = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        interactableInputText = inputTextObject.GetComponent<TextMeshPro>();
        interactableInputText.text = interactAction.bindings[0].ToDisplayString() + "/(" + interactAction.bindings[1].ToDisplayString() + ")";

        InteractUIPrompt = GameObject.FindWithTag("InteractPrompt");
        PromptText = InteractUIPrompt.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            currentCooldown = Mathf.Max(currentCooldown, 0f);
        }
    }

    void OnDisable()
    {
        if (SetUIPrompt)
        {
            GameObject player = GameObject.FindWithTag("Player");
            MothController Controller = player.GetComponent<MothController>();
            if (!Controller.IsGrabbing())
            {
                PromptText.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enabled)
        {
            if (!PromptText.enabled)
            {
                PromptText.text = interactAction.bindings[0].ToDisplayString() + "/(" + interactAction.bindings[1].ToDisplayString() + ") - Interact";
                PromptText.enabled = true;
                SetUIPrompt = true;
            }
            //inputTextObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && enabled)
        {
            transform.LookAt(other.transform);
            interactableInputText.transform.LookAt(Camera.main.transform);
            interactableInputText.transform.Rotate(Vector3.up - new Vector3(0, 180, 0));
            if (interactAction.IsPressed() && currentCooldown == 0f)
            {
                MothController Controller = other.GetComponent<MothController>();
                if (!Controller.IsGrabbing())
                {
                    Debug.Log("Interaction " + transform.name + " started");
                    TriggerObject(ObjectTrigger != null ? ObjectTrigger : transform.parent.gameObject, other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SetUIPrompt)
            {
                MothController Controller = other.GetComponent<MothController>();
                if (!Controller.IsGrabbing())
                {
                    PromptText.enabled = false;
                }
            }
        }
    }

    void TriggerObject(GameObject Object, GameObject Instigator)
    {
        bool InteractionSuccess = true;
        ITriggerOverrideInterface InteractionOverride = Object.GetComponentInChildren<ITriggerOverrideInterface>();
        if (InteractionOverride != null)
        {
            InteractionSuccess = InteractionOverride.OnInteraction(gameObject, Instigator);
        }
        else
        {
            Object.SetActive(!Object.activeSelf);
        }

        if (InteractionSuccess)
        {
            if (SingleTrigger)
            {
                gameObject.SetActive(false);
            }
            else
            {
                currentCooldown = Cooldown;
            }
        }
    }
}
