using UnityEngine;

public class Keyboard : MonoBehaviour, ITriggerOverrideInterface
{
    public GameObject ScreenText;
    public GameObject Screen;
    public Material ScreenOnMaterial;
    public GameObject Beep;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        Beep.GetComponent<AudioSource>().Play();
        ScreenText.SetActive(true);
        Screen.GetComponent<Renderer>().material = ScreenOnMaterial;
        return true;
    }
}
