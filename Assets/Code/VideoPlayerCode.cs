using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerCode : MonoBehaviour
{
    //private GameObject insertVideo;
    //private Timer timer;
    /*private float startTime = 0;
    private Dictionary<string, int> videoDict;
    //private bool isTurnOff = true;
    private int duration;
    private GameObject turnOffAbility;
    private string nameOfSceneOn;*/
    public GameObject dialogPanel;
    public Sprite[] dialogWithOwlImages;
    private int currentIndex = 0;
    private bool isAlreadyPlayed = false;
    private GameObject deerUnity;
    private float t = 0;
    private float alpha = 0;
    private bool isPlaying = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isAlreadyPlayed)
        {
            //insertVideo.SetActive(true);
            //startTime = GetComponent<Timer>().GetTime();
            //nameOfSceneOn = gameObject.name;
            if(name.Equals("FirstDialogWithOwl"))
                SaveManager.SetIsTalkedWithOwl(1);
            if(name.Equals("SecondDialogWithOwl"))
                SaveManager.SetIsTalkedWithOwl(2);
            StartDialogScene();
            isAlreadyPlayed = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.isTalkedWithOwl == 1 && name.Equals("FirstDialogWithOwl"))
        {
            isAlreadyPlayed = true;
            this.gameObject.SetActive(false);
        }
        if (SaveManager.isTalkedWithOwl == 2 && name.Equals("SecondDialogWithOwl"))
        {
            isAlreadyPlayed = true;
            this.gameObject.SetActive(false);
        }

        deerUnity = GameObject.Find("DeerUnity");
        //insertVideo = transform.GetChild(0).gameObject;
        //insertVideo.SetActive(false);
        //turnOffAbility = GameObject.Find("TurnOffAbility");
        //turnOffAbility.SetActive(false);
        /*this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();
        videoDict = new Dictionary<string, int> { { "FirstDialogWithOwl", 10 }, { "LastDialogWithOwl", 14 } };
        duration = 0;
        nameOfSceneOn = "";*/
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying && alpha < 1)
        {
            alpha += Time.deltaTime * 2;
            if(alpha >= 1)
            {
                alpha = 1;
            }
            dialogPanel.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
        }
        if (!isPlaying && alpha > 0)
        {
            alpha -= Time.deltaTime * 2;
            if (alpha <= 0)
            {
                alpha = 0;
                
            }
            dialogPanel.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            if(alpha == 0)
            {
                dialogPanel.SetActive(false);
            }
        }
    }

    private void StartDialogScene()
    {
        isPlaying = true;
        dialogPanel.SetActive(true);
        deerUnity.GetComponent<DeerUnity>().currentDialog = this.gameObject;
        dialogPanel.GetComponent<Image>().sprite = dialogWithOwlImages[currentIndex];
        transform.Find("Owl").gameObject.SetActive(false);
    }

    public void SetNextDialogImage()
    {
        currentIndex++;
        if (currentIndex < dialogWithOwlImages.Length)
        {
            dialogPanel.GetComponent<Image>().sprite = dialogWithOwlImages[currentIndex];
        }
        else
        {
            //dialogPanel.SetActive(false);
            currentIndex = 0;
            isPlaying = false;
        }
    }
}
