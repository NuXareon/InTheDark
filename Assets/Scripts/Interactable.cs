using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    private InputAction interactAction;
    private TextMeshPro interactableInputText;

    public GameObject inputTextObject;

    [Tooltip("Object that will received the effects of the trigger. By default it will be Activated/Deactivated. Behaviour can be overriden (WIP). If left empty it will target it parent object.")]
    public GameObject ObjectTrigger;
    [Tooltip("Deactivate trigger after it has been triggered.")]
    public bool SingleTrigger = true;

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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputTextObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 Lookat = other.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Lookat, Vector3.up);

            if (interactAction.IsPressed())
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

        if (InteractionSuccess && SingleTrigger)
        {
            gameObject.SetActive(false);
        }
    }
}
