using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterEnableArea : MonoBehaviour
{
    private List<GameObject> hunterPoints = new List<GameObject>();
    private bool isAlreadyMoved = false;
    public GameObject hunter;
    private GameObject deerUnity;
    // Start is called before the first frame update
    void Start()
    {
        hunterPoints.AddRange(GameObject.FindGameObjectsWithTag("HunterPoint"));
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("GeneralPlayer") && !isAlreadyMoved)
        {
            MoveHunterAtNearestPoint();
            isAlreadyMoved = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("GeneralPlayer") && !isAlreadyMoved)
        {
            MoveHunterAtNearestPoint();
            isAlreadyMoved = true;
        }
    }

    public void MoveHunterAtNearestPoint()
    {
        Hunter h;
        var isHunter = hunter.TryGetComponent<Hunter>(out h);
        if (isHunter)
        {
            hunter.GetComponent<Hunter>().isEnabled = true;
            GameObject min = null;
            var mind = float.MaxValue;
            foreach (var e in hunterPoints)
            {
                if (Math.Abs(e.transform.position.x - deerUnity.GetComponent<DeerUnity>().spawn.transform.position.x) < mind)
                {
                    mind = Math.Abs(e.transform.position.x - GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.x);
                    min = e;
                }
            }
            min.GetComponent<HunterControlPoint>().DoSame();
        }
        
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            GameObject.Find("Hunter").GetComponent<Hunter>().isEnabled = true;
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("GeneralPlayer"))
        {
            Hunter h;
            var isHunter = hunter.TryGetComponent<Hunter>(out h);
            if (isHunter)
            {
                hunter.GetComponent<Hunter>().isEnabled = false;
            }
            isAlreadyMoved = false;
        }
    }
}
