using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hunterDisablePoint : MonoBehaviour
{
    private bool isTriggered = false;
    public GameObject hunter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            hunter.GetComponent<Hunter>().DisableHunter();
        }
    }
}
