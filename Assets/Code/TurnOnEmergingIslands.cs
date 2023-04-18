using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnEmergingIslands : MonoBehaviour
{
    private GameObject startIsland;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            startIsland.GetComponent<EmergIsland>().isEmergIslandActivated = true;
            gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        startIsland = GameObject.Find("EmergingIslands");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
