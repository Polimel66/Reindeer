using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
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
        if (collision.tag == "Circle")
        {
            GameObject.Find("DeerUnity").GetComponent<DeerUnity>().TakeDamage(1000f);
        }
        if(collision.tag == "thorns" && name.Equals("UpWallChecker") && collision.isTrigger)
        {
            GameObject.Find("DeerUnity").GetComponent<DeerUnity>().TakeDamage(1000f);
        }
    }
}
