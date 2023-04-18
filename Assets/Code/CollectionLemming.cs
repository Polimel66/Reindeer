using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionLemming : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject caveBarrier;
    public void assembleLemming()
    {
        gameObject.transform.Find("Lemming").gameObject.SetActive(false);
        DeerUnity.countOfFoundLemmings += 1;
        if (DeerUnity.countOfFoundLemmings == 2)
        {
            caveBarrier.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    void Start()
    {
        caveBarrier = GameObject.Find("CaveBarrier");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
