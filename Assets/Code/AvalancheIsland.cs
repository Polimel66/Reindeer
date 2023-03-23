using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheIsland : MonoBehaviour
{
    public GameObject pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8;
    public float speed;
    public Transform startPos;
    Vector3 nextPos;
    GameObject[] positionQueue;
    public bool isLavinaStart;
    int indexOfPosition;
    void Start()
    {
        nextPos = startPos.position;
        positionQueue = new GameObject[] { pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8};
        indexOfPosition = 0;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLavinaStart)
        {
            if (indexOfPosition < positionQueue.Length - 1 && transform.position == positionQueue[indexOfPosition].transform.position)
            {
                indexOfPosition += 1;
                nextPos = positionQueue[indexOfPosition].transform.position;
            }
            else if(indexOfPosition == positionQueue.Length)
            {
                isLavinaStart = false;
                //Destroy(this);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

            }
        }
    }
}
