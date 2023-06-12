using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject continueButton;
    public GameObject loadingScreen;
    public GameObject loadingProgressBar;
    public AudioMixer audioMixer;
    public GameObject settingsMovingPanel;
    private float settingsPanelHeight = 0;
    private float t = 0;
    public GameObject audioOn;
    public GameObject audioOff;
    private Color settingsButtonActive = new Color(1, 0, 1, 1);
    private Color settingsButtonDisactive = new Color(1, 1, 1, 1);
    public static bool isAudiOn = true;
    public GameObject audioButton;
    public GameObject settingsButton;
    private bool isSettingsOn = false;
    public float settingsPanelAnimationDuration;
    public float rotationAngleDuringAnimation;

    private bool isPlayingSettingsOnAni = false;
    private bool isPlayingSettingsOffAni = false;

    private Vector3 settingsPanelOffPosition;
    private Vector3 settingsPanelOnPosition;
    private float localEulerAngleBeforeAni = 0;

    private float debugger = 0;
    public GameObject exitWarningPanel;
    public GameObject deleteSavesWarningPanel;

    public GameObject controllerType;
    public Sprite Joystick;
    public Sprite Buttons;
    public static int TypeOfInputSystem = 0;
    public GameObject charactersDescriptionPanel;
    public Image joystickInputIcon;
    public Image buttonsInputIcon;
    public GameObject textHint;
    private Timer timer;
    private string[] allHints = { "Подсказка: чтобы собрать след, нужно включить режим нюха и нажать на кнопку взаимодействия.",
        "Подсказка: используя способность нюха, оленёнок может видить запахи оставленные его стадом.",
        "Подсказка: призрак может сделать только одну платформу реальной за 1 вызов.",
        "Подсказка: у оленей есть выносливость, поэтому используй ускорение с умом.",
        "Подсказка: собирай все следы, чтобы увидеть воспоминания оленёнка.",
        "Подсказка: остерегайся капканов и шипов, которые расставляют охотники.",
        "Подсказка: чтобы спуститься с большой высоты используй левитацию призрака."};
    private int hintCounter;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        /*if (SaveManager.isAnySaves())
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }*/
        //settingsMovingPlatform.transform.parent.GetComponent<RectTransform>().rect.height
        settingsPanelHeight = settingsMovingPanel.GetComponent<RectTransform>().rect.height;
        settingsPanelOnPosition = settingsMovingPanel.transform.localPosition;
        settingsMovingPanel.transform.localPosition += new Vector3(0, settingsPanelHeight, 0);
        settingsPanelOffPosition = settingsMovingPanel.transform.localPosition;

        if (TypeOfInputSystem == 0)
        {
            //TypeOfInputSystem = 1;
            controllerType.GetComponent<Image>().sprite = Joystick;
        }
        else
        {
            //TypeOfInputSystem = 0;
            controllerType.GetComponent<Image>().sprite = Buttons;
        }

        if (isAudiOn)
        {
            //audioButton.GetComponent<Image>().sprite = audiOn;
            audioOn.SetActive(true);
            audioOff.SetActive(false);
            audioMixer.SetFloat("MasterVolume", 0);
        }
        else
        {
            //audioButton.GetComponent<Image>().sprite = audiOff;
            audioOn.SetActive(false);
            audioOff.SetActive(true);
            audioMixer.SetFloat("MasterVolume", -80);
        }
        //whatLocationStartDebugButton.transform.Find("LocationNumber").GetComponent<Text>().text = LocationStartNumber.ToString();

        //settingsButton.transform.localEulerAngles = new Vector3(0, 0, 0);
        timer = gameObject.AddComponent<Timer>();
        timer.SetPeriodForTick(3f);
        hintCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RotateAndMove();
    }

    public void OnSettingsButtonClick()
    {

        if (isSettingsOn)
        {
            
            PlaySettingsOffAnimation();
        }
        else
        {
            
            PlaySettingsOnAnimation();
        }
    }

    private void PlaySettingsOnAnimation()
    {
        //settingsButton.GetComponent<Image>().color = settingsButtonActive;
        isPlayingSettingsOnAni = true;
        isPlayingSettingsOffAni = false;
        localEulerAngleBeforeAni = settingsButton.transform.localEulerAngles.z;
    }

    private void PlaySettingsOffAnimation()
    {
        //settingsButton.GetComponent<Image>().color = settingsButtonDisactive;
        isPlayingSettingsOnAni = false;
        isPlayingSettingsOffAni = true;
        localEulerAngleBeforeAni = settingsButton.transform.localEulerAngles.z;
    }


    private void RotateAndMove()
    {
        if (isPlayingSettingsOnAni)
        {
            t = Time.deltaTime;
            var k = rotationAngleDuringAnimation * t / settingsPanelAnimationDuration;
            settingsButton.transform.localEulerAngles -= new Vector3(0, 0, k);
            settingsMovingPanel.transform.localPosition -= new Vector3(0, k * settingsPanelHeight / rotationAngleDuringAnimation);
            if(settingsButton.transform.localEulerAngles.z <= (360 - rotationAngleDuringAnimation) && settingsButton.transform.localEulerAngles.z > 0)
            {
                settingsButton.transform.localEulerAngles = new Vector3(0, 0, 360 - rotationAngleDuringAnimation);
                settingsMovingPanel.transform.localPosition = settingsPanelOnPosition;
                isPlayingSettingsOnAni = false;
                isSettingsOn = true;
            }
        }
        else if (isPlayingSettingsOffAni)
        {
            t = Time.deltaTime;
            var k = rotationAngleDuringAnimation * t / settingsPanelAnimationDuration;
            settingsButton.transform.localEulerAngles += new Vector3(0, 0, k);
            settingsMovingPanel.transform.localPosition += new Vector3(0, k * settingsPanelHeight / rotationAngleDuringAnimation);
            if (settingsButton.transform.localEulerAngles.z >= 0 && settingsButton.transform.localEulerAngles.z < rotationAngleDuringAnimation)
            {
                settingsButton.transform.localEulerAngles = new Vector3(0, 0, 0);
                settingsMovingPanel.transform.localPosition = settingsPanelOffPosition;
                isPlayingSettingsOffAni = false;
                isSettingsOn = false;
            }
        }
    }

    public void OnOffAudio()
    {
        if (isAudiOn)
        {
            isAudiOn = false;
            //audioButton.GetComponent<Image>().sprite = audiOff;
            audioOff.SetActive(true);
            audioOn.SetActive(false);
            audioMixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            isAudiOn = true;
            //audioButton.GetComponent<Image>().sprite = audiOn;
            audioOn.SetActive(true);
            audioOff.SetActive(false);
            audioMixer.SetFloat("MasterVolume", 0);
        }
    }

    public void OnContinueGameButtonClick()
    {
        
        SaveManager.LoadGame();
        Load();
    }

    /*public void OnStartNewGameButtonCkick()
    {
            
    }*/

    public void OnExitButtonClick()
    {
        exitWarningPanel.SetActive(true);
    }

    public void OnExitAcceptButtonClick()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }

    public void OnCancelExitButtonClick()
    {
        exitWarningPanel.SetActive(false);
    }

    public void OnDeleteSavesButtonClick()
    {
        deleteSavesWarningPanel.SetActive(true);
    }

    public void OnDeleteSavesAcceptButtonClick()
    {
        SaveManager.DeleteSaves();
        deleteSavesWarningPanel.SetActive(false);
    }

    public void OnCancelDeleteSavesButtonClick()
    {
        deleteSavesWarningPanel.SetActive(false);
    }

    private void Load()
    {
        //SceneManager.LoadScene("Game");
        loadingScreen.SetActive(true);
        timer.StopTimer();
        timer.ClearTimer();
        timer.StartTimer();
        textHint.GetComponent<Text>().text = allHints[(int)Random.Range(0, 6.33f)];
        hintCounter = (int)Random.Range(0, 6.33f);
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        //asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            loadingProgressBar.GetComponent<Slider>().value = asyncLoad.progress;
            /*if(asyncLoad.progress > 0.9f && !asyncLoad.allowSceneActivation)
            {
                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                asyncLoad.allowSceneActivation = true;
            }*/
            if (timer.IsTicked())
            {
                textHint.GetComponent<Text>().text = allHints[hintCounter];
                hintCounter = (int)Random.Range(0, 6.33f);
                if (hintCounter >= allHints.Length)
                {
                    hintCounter = 0;
                }
            }
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    public void OnSwitchInputSystem()
    {
        if (TypeOfInputSystem == 0)
        {
            TypeOfInputSystem = 1;
            controllerType.GetComponent<Image>().sprite = Buttons;
        }
        else
        {
            TypeOfInputSystem = 0;
            controllerType.GetComponent<Image>().sprite = Joystick;
        }
        //whatLocationStartDebugButton.transform.Find("LocationNumber").GetComponent<Text>().text = LocationStartNumber.ToString();
    }

    public void OnCharactersDiscriptionOpenButtonClick()
    {
        charactersDescriptionPanel.SetActive(true);
    }

}
