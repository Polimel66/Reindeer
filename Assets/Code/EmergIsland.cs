using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergIsland : MonoBehaviour
{
    private GameObject firstPlatformShown;
    private GameObject secondPlatformShown;
    private bool isFirstPlatformShown;
    private bool isSecondPlatformShown;
    private bool isPlatformShowOn;
    public bool isEmergIslandActivated;
    public GameObject nextGroup;
    public int counterPlat;
    private GameObject timerIsland;
    private bool platOn;
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
            
    //    }
    //}
    // Start is called before the first frame update
    void Start()
    {
        firstPlatformShown = transform.GetChild(0).gameObject;
        firstPlatformShown.SetActive(false);
        secondPlatformShown = transform.GetChild(1).gameObject;
        secondPlatformShown.SetActive(false);
        isPlatformShowOn = false;
        isFirstPlatformShown = false;
        isSecondPlatformShown = false;
        counterPlat = 0;
        timerIsland = GameObject.Find("TimerIsland");
        platOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEmergIslandActivated)
        {
            isPlatformShowOn = true;
            this.gameObject.AddComponent<Timer>();
            GetComponent<Timer>().SetPeriodForTick(1f);
            GetComponent<Timer>().StartTimer();
            isEmergIslandActivated = false;
            //timerIsland.GetComponent<Timer>().SetPeriodForTick(6f);
            //timerIsland.GetComponent<Timer>().StopTimer();
            //timerIsland.GetComponent<Timer>().ClearTimer();
            //timerIsland.GetComponent<Timer>().StartTimer();
        }
        if (isPlatformShowOn)
        {
            firstPlatformShown.SetActive(true);
            secondPlatformShown.SetActive(true);
            isFirstPlatformShown = true;
            isSecondPlatformShown = true;
            isPlatformShowOn = false;
        }
        if(isFirstPlatformShown && isSecondPlatformShown && GetComponent<Timer>().IsTicked()) 
        {
            firstPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
            secondPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
            isFirstPlatformShown = false;
        }


        if (counterPlat == 2)
        {
            if (nextGroup != null)
            {
                nextGroup.GetComponent<EmergIsland>().isEmergIslandActivated = true;
                isPlatformShowOn = false;
                isFirstPlatformShown = false;
                isSecondPlatformShown = false;
                counterPlat = 0;
                platOn = false;
            }
        }
        //else
        //{
        //    if (timerIsland.GetComponent<Timer>().IsTicked() && !platOn)
        //    {
        //        firstPlatformShown.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255);
        //        secondPlatformShown.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255);
        //        platOn = true;
        //        Invoke("TurnOffBothPlat", 1f);
        //    }
        //    //else if (counterPlat == 1 && timerIsland.GetComponent<Timer>().IsTicked() && !platOn)
        //    //{
        //    //    secondPlatformShown.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255);
        //    //    platOn = true;
        //    //    Invoke("TurnOffOnePlat", 1f);
        //    //}
        //}
    }

    public void TurnOffBothPlat()
    {
        firstPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
        secondPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
        platOn = false;
    }
    public void TurnOffOnePlat()
    {
        secondPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
        platOn = false;
    }
}
