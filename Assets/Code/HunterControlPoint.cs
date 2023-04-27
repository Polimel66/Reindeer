using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterControlPoint : MonoBehaviour
{
    public GameObject hunter;
    private GameObject hunterPoint;
    public bool isStayAtPoint;
    public bool isAlreadyWorked = false;
    public bool isExtraDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        hunterPoint = transform.parent.Find("HunterPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !isAlreadyWorked)
        {
            hunter.GetComponent<Hunter>().isExtraDamage = isExtraDamage;
            isAlreadyWorked = true;
            if (isStayAtPoint)
            {
                hunter.GetComponent<Hunter>().StayAtPoint(hunterPoint.transform);
            }
            else
            {
                hunter.GetComponent<Hunter>().HuntDeerAtPoint(hunterPoint.transform);
            }
        }
    }

    public void DoSame()
    {
        hunter.GetComponent<Hunter>().isExtraDamage = isExtraDamage;
        if (isStayAtPoint)
        {
            hunter.GetComponent<Hunter>().StayAtPoint(hunterPoint.transform);
        }
        else
        {
            hunter.GetComponent<Hunter>().HuntDeerAtPoint(hunterPoint.transform);
        }
    }
}
