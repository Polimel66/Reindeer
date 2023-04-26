using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moss : MonoBehaviour
{
    private static GameObject deerUnity;
    private static BoxCollider2D coll;
    private GameObject wind;
    public GameObject darkPartCave;
    public GameObject phraseMoss;
    public GameObject phraseBeforeCollectingMoss;
    public GameObject phraseAfterCollectingMoss;
    // Start is called before the first frame update
    public void takeMoss()
    {
        
    }
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        coll = GetComponent<BoxCollider2D>();
        wind = GameObject.Find("LiftingWind (2)");
        phraseMoss.SetActive(false);
        phraseAfterCollectingMoss.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var deer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
        if (coll.IsTouching(deer.GetComponent<BoxCollider2D>()) && DeerUnity.CurrentActive == 1 && DeerUnity.isPossibleTakeMoss)
        {
            gameObject.SetActive(false);
            DeerUnity.isMossFound = true;
            wind.GetComponent<Wind>().isWorking = false;
            GameObject.Find("Wind (17)").SetActive(false);
            GameObject.Find("Wind (19)").SetActive(false);
            phraseMoss.SetActive(true);
            darkPartCave.SetActive(false);
            phraseBeforeCollectingMoss.SetActive(false);
            phraseAfterCollectingMoss.SetActive(true);
            deerUnity.GetComponent<DeerUnity>().SetTask(13);
        }
        DeerUnity.isPossibleTakeMoss = false;
    }
}
