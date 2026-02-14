using UnityEngine;

public class Grabbable : MonoBehaviour, ITriggerOverrideInterface
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        Debug.Log("Interaction overriden by " + transform.name);
        MothController Controller = OtherObject.GetComponent<MothController>();
        if (Controller != null)
        {
            Debug.Log("Initiating grab for " + OtherObject.name);
            return Controller.Grab(gameObject.transform.parent != null ? gameObject.transform.parent.gameObject : gameObject);
        }
        return false;
    }
}
