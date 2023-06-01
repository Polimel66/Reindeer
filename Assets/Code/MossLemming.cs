using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossLemming : MonoBehaviour
{
    private static GameObject deerUnity;
    private static BoxCollider2D coll;
    private GameObject startEmergingIsland;
    public GameObject firstEmergingPhrase;
    public GameObject secondEmergingPhrase;
    private GameObject emptyShelter;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        coll = GetComponent<BoxCollider2D>();
        startEmergingIsland = GameObject.Find("EmergingIslandsChecker");
        emptyShelter = GameObject.Find("EmptyShelter");
        emptyShelter.SetActive(false);
        startEmergingIsland.SetActive(false);
        secondEmergingPhrase.SetActive(false);

        if(SaveManager.isThirdLemCollected == 1)
        {
            DeerUnity.countOfFoundLemmings = 3;
            startEmergingIsland.SetActive(true);
            gameObject.SetActive(false);
            firstEmergingPhrase.SetActive(false);
            secondEmergingPhrase.SetActive(true);
            //deerUnity.GetComponent<DeerUnity>().SetTask(12);
            //deerUnity.GetComponent<DeerUnity>().SetTask(10);
            emptyShelter.SetActive(true);
            //SaveManager.isThirdLemCollected = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var deer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
        if (coll.IsTouching(deer.GetComponent<BoxCollider2D>()) && DeerUnity.CurrentActive == 1 && DeerUnity.isMossFound && DeerUnity.isPossibleTakeLemming)
        {
            DeerUnity.countOfFoundLemmings += 1;
            startEmergingIsland.SetActive(true);
            gameObject.SetActive(false);
            firstEmergingPhrase.SetActive(false);
            secondEmergingPhrase.SetActive(true);
            deerUnity.GetComponent<DeerUnity>().SetTask(12);
            deerUnity.GetComponent<DeerUnity>().SetTask(10);
            emptyShelter.SetActive(true);
            SaveManager.isThirdLemCollected = 1;
        }
        DeerUnity.isPossibleTakeLemming = false;
    }
}
