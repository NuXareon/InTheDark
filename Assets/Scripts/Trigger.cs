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
            Debug.LogAssertion("Null Object trigger for trigger " + transform.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ObjectFilter != null)
        {
            if (ObjectFilter == other.gameObject)
            {
                Debug.Log("Trigger " + transform.name + " entered by " + other.transform.name);
                TriggerObject(ObjectTrigger);
            }
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger " + transform.name + " entered by " + other.transform.name);
            TriggerObject(ObjectTrigger);
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
    void TriggerObject(GameObject Object)
    {
        Object.SetActive(!Object.activeSelf);

        if (SingleTrigger)
        {
            gameObject.SetActive(false);
        }
    }
}