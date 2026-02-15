using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    private InputAction interactAction;
    private TextMeshPro interactableInputText;
    private float currentCooldown = 0f;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enabled)
        {
            inputTextObject.SetActive(true);
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
                Debug.Log("Interaction " + transform.name + " started");
                TriggerObject(ObjectTrigger != null ? ObjectTrigger : transform.parent.gameObject, other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputTextObject.SetActive(false);
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
