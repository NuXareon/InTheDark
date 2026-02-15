using UnityEngine;

public class Keycard : MonoBehaviour, ITriggerOverrideInterface
{
    public GameObject PCScreenActivate;
    public GameObject PCScreenDeactivate;
    public GameObject DoorTrigger;
    public Transform KeycardReaderSlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        // Insert keycard into slot
        OtherObject.transform.localPosition = KeycardReaderSlot.position;
        OtherObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        Rigidbody OtherObjectRigidBody = OtherObject.GetComponent<Rigidbody>();
        if (OtherObjectRigidBody != null)
        {
            OtherObjectRigidBody.isKinematic = true;
        }
        Interactable OtherObjectInteractable = OtherObject.GetComponentInChildren<Interactable>();
        if (OtherObjectInteractable != null)
        {
            OtherObjectInteractable.enabled = false;
        }

        PCScreenDeactivate.SetActive(false);
        PCScreenActivate.SetActive(true);
        DoorTrigger.SetActive(true);

        return true;
    }
}
