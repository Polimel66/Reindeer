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
    }
}
