using UnityEngine;
using System.Collections;

public class PhoneLight : MonoBehaviour, ITriggerOverrideInterface
{
    public float delay = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        StartCoroutine(DelayedStart(delay));
        return true;
    }

    IEnumerator DelayedStart(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Light>().enabled = true;
    }

}
