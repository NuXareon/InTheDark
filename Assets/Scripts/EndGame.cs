using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour, ITriggerOverrideInterface
{
    public GameObject Moth;
    public GameObject MothLight;
    public GameObject EndGameCamera;
    public GameObject Moon;
    public GameObject InTheDarkAudio;
    public GameObject InTheDarkTitle;
    public GameObject InTheDarkCredits;
    public float FlySpeed = 3f;
    private bool ToTheMoon = false;
    private CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = Moth.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ToTheMoon)
        {
            Moth.transform.LookAt(Moon.transform);
            Vector3 FlyDirection = Moon.transform.position - Moth.transform.position;
            FlyDirection.Normalize();
            controller.Move(FlyDirection * FlySpeed * Time.deltaTime);
        }
    }

    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        Moth.GetComponent<MothController>().enabled = false;
        Moth.GetComponentInChildren<Camera>().gameObject.SetActive(false);
        MothLight.SetActive(true);
        EndGameCamera.SetActive(true);
        ToTheMoon = true;
        
        StartCoroutine(InTheDark(3f));
        StartCoroutine(ResetScene(20f));

        return true;
    }

    IEnumerator InTheDark(float time)
    {
        yield return new WaitForSeconds(time);
        InTheDarkAudio.GetComponent<AudioSource>().Play();
        InTheDarkTitle.SetActive(true);
        InTheDarkCredits.SetActive(true);
    }

    IEnumerator ResetScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
