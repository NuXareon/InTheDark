using UnityEngine;

public class Grabbable : MonoBehaviour, ITriggerOverrideInterface
{
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
