using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionLemming : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject caveBarrier;
    private static GameObject deerUnity;
    public void assembleLemming()
    {
        gameObject.transform.Find("Lemming").gameObject.SetActive(false);
        //DeerUnity.countOfFoundLemmings += 1;
        //deerUnity.GetComponent<DeerUnity>().SetTask(10);
        if (DeerUnity.countOfFoundLemmings == 2)
        {
            caveBarrier.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    void Start()
    {
        caveBarrier = GameObject.Find("CaveBarrier");
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
