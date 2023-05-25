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
    private bool platOn;
    public float color;
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
            firstPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1);
            secondPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1);
            GetComponent<Timer>().SetPeriodForTick(6f);
            GetComponent<Timer>().ClearTimer();
            GetComponent<Timer>().StartTimer();
            isFirstPlatformShown = false;
        }

        color = firstPlatformShown.GetComponent<SpriteRenderer>().color.a;
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
        else
        {
            if (counterPlat == 0 && GetComponent<Timer>().IsTicked() && !platOn)
            {
                firstPlatformShown.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 1);
                secondPlatformShown.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 1);
                color = firstPlatformShown.GetComponent<SpriteRenderer>().color.a;
                platOn = true;
                Invoke("TurnOffBothPlat", 1f);
            }
            else if (counterPlat == 1 && GetComponent<Timer>().IsTicked() && !platOn)
            {
                secondPlatformShown.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 1);
                platOn = true;
                Invoke("TurnOffOnePlat", 1f);
            }
        }
        color = firstPlatformShown.GetComponent<SpriteRenderer>().color.a;
    }

    public void TurnOffBothPlat()
    {
        firstPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1);
        secondPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1);
        platOn = false;
    }
    public void TurnOffOnePlat()
    {
        secondPlatformShown.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1);
        platOn = false;
    }
}
