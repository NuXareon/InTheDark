using UnityEngine;

public class Trigger : MonoBehaviour
{
    [Tooltip("Object that will trigger this script. If not set, the player will be the target.")]
    public GameObject ObjectFilter;
    [Tooltip("Object that will received the effects of the trigger. By default it will be Activated/Deactivated. Behaviour can be overriden (WIP)")]
    public GameObject ObjectTrigger;
    [Tooltip("Deactivate trigger after it has been triggered.")]
    public bool SingleTrigger = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ObjectTrigger == null)
        {
            ObjectTrigger = transform.parent ? transform.parent.gameObject : gameObject;
            if (ObjectTrigger == null)
            {
                Debug.LogAssertion("Null Object trigger for trigger " + transform.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ObjectFilter != null)
        {
            if (ObjectFilter == other.gameObject)
            {
                Debug.Log("Trigger " + transform.name + " entered by " + other.transform.name);
                TriggerObject(ObjectTrigger, other.gameObject);
            }
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger " + transform.name + " entered by " + other.transform.name);
            TriggerObject(ObjectTrigger, other.gameObject);
        }
    }

    /* For now we might not need this
    private void OnTriggerExit(Collider other)
    {
        if (ObjectFilter != null)
        {
            if (ObjectFilter == other.gameObject)
            {
                Debug.Log("Trigger " + transform.name + " left by " + other.transform.name);
            }
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger " + transform.name + " left by " + other.transform.name);
        }
    }
    */
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