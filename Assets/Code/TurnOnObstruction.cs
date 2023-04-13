using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnObstruction : MonoBehaviour
{
    private GameObject obstruction;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        obstruction.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        obstruction = GameObject.Find("Obstruction");
        obstruction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
