using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject trace;
    private bool isCollected = false;
    //public GameObject lamp;
    public GameObject star;
    private int state = 0;
    
    void Start()
    {
        //if(lamp != null)
        //    lamp.SetActive(false);
        trace.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DeerUnity.CurrentActive == 1 && ReindeerSmall.isSmell && state == 0)
        {
            state = 1;
            trace.SetActive(true);
            if (!isCollected)
            {
                //if (lamp != null)
                //    lamp.SetActive(true);
            }
        }
        else if(!ReindeerSmall.isSmell && state == 1)
        {
            state = 0;
            //if (lamp != null)
            //    lamp.SetActive(false);
            trace.SetActive(false);
        }
        if(ReindeerSmall.isSmell && !isCollected && star != null)
        {
            star.SetActive(true);
        }
        if(!ReindeerSmall.isSmell && !isCollected && star != null)
        {
            star.SetActive(false);
        }
    }

    public void Collect()
    {
        if (!isCollected)
        {
            isCollected = true;
            if(star != null)
                star.SetActive(false);
        }
    }
}
