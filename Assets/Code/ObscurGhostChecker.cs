using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObscurGhostChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ProtectionArea")
        {
            //GameObject.Find("LiftingWind").GetComponent<Wind>().totalForce = 0;
            gameObject.transform.parent.gameObject.GetComponent<ReindeerGhost>().setIsWindProtected(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ProtectionArea")
        {
            //GameObject.Find("LiftingWind").GetComponent<Wind>().totalForce = 25;
            gameObject.transform.parent.gameObject.GetComponent<ReindeerGhost>().setIsWindProtected(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
