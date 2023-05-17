using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformChecker : MonoBehaviour
{
    private GameObject deerUnity;
    private Collider2D collision;
    private Queue<Collider2D> queueOn = new Queue<Collider2D>();
    private Queue<Collider2D> queueOff = new Queue<Collider2D>();
    private int counter = 0;
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform" || collision.tag == "CircleGhostPlatform")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
            if (!collision.gameObject.name.Equals("RotateBox"))
            {
                deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.parent = collision.transform;
                deerUnity.GetComponent<DeerUnity>().isOnMovePlatform = true;
            }
        }
        else if (collision.tag == "EmergIsland")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
            collision.gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255);
            if (collision.gameObject.GetComponent<SpriteRenderer>().color.a == 1)
                collision.gameObject.transform.parent.GetComponent<EmergIsland>().counterPlat += 1;
        }
        else if(collision.tag == "MaterialisedPlatform")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
        }
        else if (collision.tag == "MoveObject")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
        }
        else if (collision.tag == "CollapsingPlat")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
            if (DeerUnity.CurrentActive != 2)
            {
                /*if (!(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.Find("RightWallChecker").gameObject.GetComponent<BoxCollider2D>().IsTouching(collision)
                    || deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.Find("LeftWallChecker").gameObject.GetComponent<BoxCollider2D>().IsTouching(collision)))
                {
                    queueOff.Enqueue(collision);
                    queueOn.Enqueue(collision);
                    Invoke("TurnOffPlatform", 1f);
                }*/
                queueOff.Enqueue(collision);
                queueOn.Enqueue(collision);
                Invoke("TurnOffPlatform", 1f);
            }
        }
        else if (collision.tag == "thorns" && collision.isTrigger)
        {
            deerUnity.GetComponent<DeerUnity>().TakeDamage(1000f);
        }
        else if (collision.tag == "GhostPlatform")
        {
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerGhost>().currendGhostPlatform = collision.gameObject;
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
        }
        
        else if (collision.tag == "LavinaMovingPlat")
        {
            /*var reindeer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
            reindeer.transform.parent = collision.transform;
            deerUnity.GetComponent<DeerUnity>().isOnMovePlatform = true;
            reindeer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            reindeer.GetComponent<BoxCollider2D>().isTrigger = true;
            reindeer.GetComponent<ReindeerSmall>().StopMoving();
            reindeer.GetComponent<Rigidbody2D>().gravityScale = 0f;
            counter++;
            GameObject.Find("Info").GetComponent<Text>().text = counter.ToString();*/
        }
        if (collision.tag == "CollectionArea")
        {
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().currentLemmingArea = collision.gameObject;
        }
        
    }

    void TurnOffPlatform()
    {
        queueOff.Dequeue().gameObject.SetActive(false);
        Invoke("TurnOnPlatform", 5f);
    }
    void TurnOnPlatform()
    {
        queueOn.Dequeue().gameObject.SetActive(true);
    }
    
    //void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name.Equals("RotateBox"))
    //        deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platform" || collision.tag == "GhostPlatform" || collision.tag == "MoveObject" || collision.tag == "CircleGhostPlatform" || collision.tag == "MaterialisedPlatform")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = false;
            deerUnity.GetComponent<DeerUnity>().isOnMovePlatform = false;
            deerUnity.GetComponent<DeerUnity>().isOnGhostPlatform = false;
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.parent = null;
            //deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerGhost>().currendGhostPlatform = null;
        }
        else if(collision.tag == "CollapsingPlat" || collision.tag == "EmergIsland")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = false;
        }
        else if (collision.tag == "LavinaMovingPlat")
        {
            /*var reindeer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
            reindeer.transform.parent = null;
            deerUnity.GetComponent<DeerUnity>().isOnMovePlatform = false;
            reindeer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            reindeer.GetComponent<BoxCollider2D>().isTrigger = false;
            reindeer.GetComponent<ReindeerSmall>().StartMoving();
            reindeer.GetComponent<Rigidbody2D>().gravityScale = 1.5f;*/
        }
        else if (collision.tag == "CollectionArea")
        {
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().currentLemmingArea = null;
        }
    }

    void Update()
    {
        
    }
}
