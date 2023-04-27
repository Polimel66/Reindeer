using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCooling : MonoBehaviour
{
    private static GameObject deerUnity;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "ShadOnCheck")
            deerUnity.GetComponent<DeerUnity>().isActivateCooling = true;
        else if (gameObject.name == "ShadOffCheck")
            deerUnity.GetComponent<DeerUnity>().isActivateCooling = false;
    }
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
