using UnityEngine;

public class Register : MonoBehaviour, ITriggerOverrideInterface
{
    public GameObject CoinInteract;
    public Vector3 OpenMovement = new Vector3(0f, 0f, 2f);
    public float Duration = 1f;
    public GameObject CashSound;

    private bool isMoving = false;
    private float moveDuration = 0f;
    private Vector3 StartingPos;
    private Vector3 FinalPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartingPos = transform.position;
        FinalPos = StartingPos + OpenMovement;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            moveDuration += Time.deltaTime;
            float progress = moveDuration / Duration;
            progress = Mathf.Min(1f, progress);
            transform.position = Vector3.Lerp(StartingPos, FinalPos, 1 - Mathf.Pow(1 - progress, 3));

            if (progress == 1f)
            {
                isMoving = false;
                CoinInteract.SetActive(true);
            }
        }
    }

    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        CashSound.GetComponent<AudioSource>().Play();
        isMoving = true;
        return true;
    }
}
