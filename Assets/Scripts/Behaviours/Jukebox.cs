using UnityEngine;
using System.Collections;

public class Jukebox : MonoBehaviour, ITriggerOverrideInterface
{
    private int Coins = 0;

    public Transform CoinSlot1;
    public Transform CoinSlot2;
    public GameObject JukeboxLight;

    public Material JukeboxEmissive;
    public GameObject JukeboxEmissiveObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        Debug.Log("Coin inserted in jukebox");

        // Insert coin into slot
        if (Coins == 0)
        {
            OtherObject.transform.localPosition = CoinSlot1.position;
        }
        else
        {
            OtherObject.transform.localPosition = CoinSlot2.position;
        }
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

        // Increase coins and trigger logic if ready
        Coins++;
        if (Coins == 2) 
        {
            StartCoroutine(StartJukeboxDelayed(0.5f));
        }

        Debug.Log("Coins in jukebox: " + Coins);

        return true;
    }

    void StartJukebox()
    {
        JukeboxEmissiveObject.GetComponent<Renderer>().material = JukeboxEmissive;
        StartCoroutine(StartLightsDelayed(1f));
    }

    void StartLights()
    {
        Flicker LightFlicker = JukeboxLight.GetComponent<Flicker>();
        if (LightFlicker != null)
        {
            LightFlicker.StartFlicker();
        }
        else
        {
            JukeboxLight.SetActive(true);
        }
    }

    IEnumerator StartJukeboxDelayed(float time)
    {
        yield return new WaitForSeconds(time);
        StartJukebox();
    }

    IEnumerator StartLightsDelayed(float time)
    {
        yield return new WaitForSeconds(time);
        StartLights();
    }
}
