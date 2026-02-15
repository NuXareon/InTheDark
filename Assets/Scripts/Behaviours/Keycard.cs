using UnityEngine;

public class Keycard : MonoBehaviour, ITriggerOverrideInterface
{
    public GameObject PCScreenActivate;
    public GameObject PCScreenDeactivate;
    public GameObject DoorTrigger;
    public GameObject Screen;
    public Material ScreenOnMaterial;
    public Transform KeycardReaderSlot;
    public GameObject KeyboardTrigger;
    public GameObject DoorDeactivateText;
    public GameObject DoorActivateText;
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
        Screen.GetComponent<Renderer>().material = ScreenOnMaterial;
        DoorDeactivateText.SetActive(false);
        DoorActivateText.SetActive(true);
        DoorTrigger.SetActive(true);

        return true;
    }
}
