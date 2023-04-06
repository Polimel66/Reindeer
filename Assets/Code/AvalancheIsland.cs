using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheIsland : MonoBehaviour
{
    public GameObject pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9;
    public float speed;
    public Transform startPos;
    Vector3 nextPos;
    GameObject[] positionQueue;
    public bool isLavinaStart;
    int indexOfPosition;
    private GameObject deerUnity;
    public GameObject SnowSystem;
    public GameObject FogSystem;
    private bool isLavinaStartInvoked = false;
    public GameObject canvas;
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        nextPos = startPos.position;
        positionQueue = new GameObject[] { pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9};
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
            else if(indexOfPosition == positionQueue.Length - 1)
            {
                isLavinaStart = false;
                var reindeer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
                reindeer.transform.parent = null;
                this.gameObject.SetActive(false);
                reindeer.transform.parent = null;
                reindeer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                reindeer.GetComponent<BoxCollider2D>().isTrigger = false;
                reindeer.GetComponent<ReindeerSmall>().StartMoving();
                canvas.SetActive(true);

                //Destroy(this);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

            }
        }
        else
        {
            if (GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>()
                .GetCurrentActiveDeer().transform.Find("Ground").gameObject.GetComponent<BoxCollider2D>()))
            {
                isLavinaStartInvoked = true;
                SnowSystem.GetComponent<ParticleSystem>().Play();
                FogSystem.GetComponent<ParticleSystem>().Play();
                Invoke("StartLavina", 1f);
            }
        }
    }

    private void StartLavina()
    {
        isLavinaStart = true;
        canvas.SetActive(false);
    }
}
