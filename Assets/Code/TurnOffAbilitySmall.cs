using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffAbilitySmall : MonoBehaviour
{
    public int whatAbilityIsActivate;
    public GameObject deerUnity;
    private bool isWorked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isWorked && collision.tag.Equals("Player"))
        {
            if (whatAbilityIsActivate == 1)
            {
                deerUnity.GetComponent<DeerUnity>().isFirstAbilitySmallAvailable = false;
                if (DeerUnity.CurrentActive == 1)
                {
                    deerUnity.GetComponent<DeerUnity>().firstAbilLock.SetActive(true);
                }
            }
            else if (whatAbilityIsActivate == 2)
            {
                deerUnity.GetComponent<DeerUnity>().isSecondAbilitySmallAvailable = false;
                if (DeerUnity.CurrentActive == 1)
                {
                    deerUnity.GetComponent<DeerUnity>().secondAbilLock.SetActive(true);
                }
            }
            isWorked = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("GeneralPlayer"))
        {
            if (whatAbilityIsActivate == 1)
            {
                deerUnity.GetComponent<DeerUnity>().isFirstAbilitySmallAvailable = true;
                if (DeerUnity.CurrentActive == 1)
                {
                    deerUnity.GetComponent<DeerUnity>().firstAbilLock.SetActive(false);
                }
            }
            else if (whatAbilityIsActivate == 2)
            {
                deerUnity.GetComponent<DeerUnity>().isSecondAbilitySmallAvailable = true;
                if (DeerUnity.CurrentActive == 1)
                {
                    deerUnity.GetComponent<DeerUnity>().secondAbilLock.SetActive(false);
                }
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
