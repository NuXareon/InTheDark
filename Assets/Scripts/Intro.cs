using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject IntroCamera;
    public GameObject Moth;
    public GameObject Controls;
    public GameObject Fade;
    public GameObject PressToStart;
    public float FadeTime = 1f;

    private InputAction flyAction;
    private MothController mothController;
    private Image FadeImage;
    private float FadeProgress = 0f;
    private bool Fading = false;
    private bool ReadyToStart = false;
    private float ReadyToStartTime = 0f;
    private Color FadeColor;
    private TextMeshProUGUI PressToStartText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flyAction = InputSystem.actions.FindAction("Jump");
        mothController = Moth.GetComponent<MothController>();
        FadeImage = Fade.GetComponent<Image>();
        FadeColor = FadeImage.color;
        PressToStartText = GetComponent<TextMeshProUGUI>();
        PressToStartText.text = "Press " + flyAction.bindings[0].ToDisplayString() + "/(" + flyAction.bindings[1].ToDisplayString() + ") to Start";

        mothController.enabled = false;
        MainCamera.SetActive(false);
        IntroCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ReadyToStart)
        {
            ReadyToStartTime += Time.deltaTime;
            if (ReadyToStartTime > 1f)
            {
                PressToStart.SetActive(true);
                ReadyToStart = true;
            }
        }
        if (ReadyToStart)
        { 
            if (!Fading)
            {
                bool Start = flyAction.IsPressed();
                if (Start)
                {
                    Fading = true;
                    FadeImage.CrossFadeAlpha(1.0f, FadeTime, true);
                }
            }
            if (Fading)
            {
                FadeProgress += Time.deltaTime;
                float ProgressPct = FadeProgress / FadeTime;
                ProgressPct = Mathf.Clamp(ProgressPct, 0f, 1f);
                FadeColor.a = ProgressPct;
                FadeImage.color = FadeColor;
                if (FadeProgress >= FadeTime + 0.5f)
                {
                    MainCamera.SetActive(true);
                    IntroCamera.SetActive(false);
                    mothController.enabled = true;
                    Controls.SetActive(true);

                    gameObject.SetActive(false);
                }
            }
        }
    }
}
