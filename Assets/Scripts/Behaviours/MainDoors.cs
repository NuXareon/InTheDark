using UnityEngine;

public class MainDoors : MonoBehaviour, ITriggerOverrideInterface
{
    public GameObject LeftDoor;
    public GameObject RightDoor;
    public GameObject DoorAudio;
    public float slideDuration = 1f;
    public Vector3 slideAmount = new Vector3(5f, 0f, 0f);

    private bool bSliding = false;
    private float currentSlideDuration = 0f;
    private Vector3 StartingPosLeft;
    private Vector3 StartingPosRight;
    private Vector3 FinalPosLeft;
    private Vector3 FinalPosRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartingPosLeft = LeftDoor.transform.position;
        StartingPosRight = RightDoor.transform.position;
        FinalPosLeft = StartingPosLeft + slideAmount;
        FinalPosRight = StartingPosRight - slideAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (bSliding)
        {
            currentSlideDuration += Time.deltaTime;
            float progress = currentSlideDuration / slideDuration;
            progress = Mathf.Min(1f, progress);
            LeftDoor.transform.position = Vector3.Lerp(StartingPosLeft, FinalPosLeft, progress);
            RightDoor.transform.position = Vector3.Lerp(StartingPosRight, FinalPosRight, progress);

            if (progress == 1f)
            {
                bSliding = false;
                //CoinInteract.SetActive(true);
            }
        }
    }

    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        DoorAudio.GetComponent<AudioSource>().Play();
        bSliding = true;
        return true;
    }
}
