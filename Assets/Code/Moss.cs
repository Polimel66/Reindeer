using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moss : MonoBehaviour
{
    private static GameObject deerUnity;
    private static BoxCollider2D coll;
    // Start is called before the first frame update
    public void takeMoss()
    {
        
    }
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var deer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
        if (coll.IsTouching(deer.GetComponent<BoxCollider2D>()) && DeerUnity.CurrentActive == 1 && DeerUnity.isPossibleTakeMoss)
        {
            gameObject.SetActive(false);
            DeerUnity.isMossFound = true;
        }
        DeerUnity.isPossibleTakeMoss = false;
    }
}
