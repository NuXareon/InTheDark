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
    public GameObject Beep;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        // Insert keycard into slot
        OtherObject.transform.localPosition = KeycardReaderSlot.position;
        OtherObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        Rigidbody OtherObjectRigidBody = OtherObject.transform.GetComponent<Rigidbody>();
        if (OtherObjectRigidBody != null)
        {
            OtherObjectRigidBody.isKinematic = true;
        }
        Interactable OtherObjectInteractable = OtherObject.transform.GetComponentInChildren<Interactable>();
        if (OtherObjectInteractable != null)
        {
            OtherObjectInteractable.enabled = false;
        }
        Grabbable OtherObjectGrabbable = OtherObject.transform.GetComponentInChildren<Grabbable>();
        if (OtherObjectGrabbable != null)
        {
            OtherObjectGrabbable.enabled = false;
        }

        KeyboardTrigger.SetActive(false);
        PCScreenDeactivate.SetActive(false);
        PCScreenActivate.SetActive(true);
        Screen.GetComponent<Renderer>().material = ScreenOnMaterial;
        DoorDeactivateText.SetActive(false);
        DoorActivateText.SetActive(true);
        DoorTrigger.SetActive(true);
        Beep.GetComponent<AudioSource>().Play();

        return true;
    }
}
