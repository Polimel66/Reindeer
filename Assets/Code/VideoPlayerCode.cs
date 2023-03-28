using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerCode : MonoBehaviour
{
    private GameObject insertVideo;
    //private Timer timer;
    private float startTime = 0;
    private Dictionary<string, int> videoDict;
    private bool isTurnOff = true;
    private int duration;
    private GameObject turnOffAbility;
    private string nameOfSceneOn;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            insertVideo.SetActive(true);
            startTime = GetComponent<Timer>().GetTime();
            nameOfSceneOn = gameObject.name;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        insertVideo = transform.GetChild(0).gameObject;
        insertVideo.SetActive(false);
        //turnOffAbility = GameObject.Find("TurnOffAbility");
        //turnOffAbility.SetActive(false);
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();
        videoDict = new Dictionary<string, int> { { "FirstDialogWithOwl", 14 }, { "LastDialogWithOwl", 14 } };
        duration = 0;
        nameOfSceneOn = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurnOff)
        {
            isTurnOff = false;
            duration = videoDict[gameObject.name];
            //turnOffAbility.SetActive(true);
        }
        if (GetComponent<Timer>().GetTime() - startTime >= duration && !isTurnOff)
        {
            insertVideo.SetActive(false);
            if (nameOfSceneOn == "FirstDialogWithOwl" || nameOfSceneOn == "LastDialogWithOwl")
            {
                gameObject.SetActive(false);
            }
        }
    }
}
