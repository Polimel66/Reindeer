using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnAbilityGhost : MonoBehaviour
{
    public int whatAbilityIsActivate;
    public GameObject deerUnity;
    private void OnTriggerEnter2D(Collider2D collision)
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
