using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffAbilityGhost : MonoBehaviour
{
    public int whatAbilityIsActivate;
    public GameObject deerUnity;
    private bool isWorked = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isWorked && collision.tag.Equals("GeneralPlayer"))
        {
            if (whatAbilityIsActivate == 1)
            {
                deerUnity.GetComponent<DeerUnity>().isFirstAbilityGhostAvailable = false;
                if (DeerUnity.CurrentActive == 2)
                {
                    deerUnity.GetComponent<DeerUnity>().firstAbilLock.SetActive(true);
                }
            }
            else if (whatAbilityIsActivate == 2)
            {
                deerUnity.GetComponent<DeerUnity>().isSecondAbilityGhostAvailable = false;
                if (DeerUnity.CurrentActive == 2)
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
                deerUnity.GetComponent<DeerUnity>().isFirstAbilityGhostAvailable = true;
                if (DeerUnity.CurrentActive == 2)
                {
                    deerUnity.GetComponent<DeerUnity>().firstAbilLock.SetActive(false);
                }
            }
            else if (whatAbilityIsActivate == 2)
            {
                deerUnity.GetComponent<DeerUnity>().isSecondAbilityGhostAvailable = true;
                if (DeerUnity.CurrentActive == 2)
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
