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

    public void OnHit(GameObject Instigator)
    {
        Debug.Log("Hit " + transform.name);
        TriggerObject(ObjectTrigger, Instigator);
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
