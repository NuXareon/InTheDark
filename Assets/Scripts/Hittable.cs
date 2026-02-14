using UnityEngine;

public class Hittable : MonoBehaviour
{
    [Tooltip("Object that will received the effects of the trigger. By default it will be Activated/Deactivated. Behaviour can be overriden (WIP)")]
    public GameObject ObjectTrigger;
    [Tooltip("Deactivate trigger after it has been triggered.")]
    public bool SingleTrigger = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
  
    }

    public void OnHit()
    {
        Debug.Log("Hit " + transform.name);
        TriggerObject(ObjectTrigger);
    }

    void TriggerObject(GameObject Object)
    {
        Object.SetActive(!Object.activeSelf);

        if (SingleTrigger)
        {
            gameObject.SetActive(false);
        }
    }
}
