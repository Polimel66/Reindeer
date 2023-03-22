using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayer : MonoBehaviour
{
    private GameObject insertVideo;
    //private Timer timer;
    private float startTime = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            insertVideo.SetActive(true);
            startTime = GetComponent<Timer>().GetTime();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        insertVideo = GameObject.Find("OwlVideoPlayer").gameObject;
        insertVideo.SetActive(false);
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Timer>().GetTime() - startTime >= 14)
            insertVideo.SetActive(false);
    }
}
