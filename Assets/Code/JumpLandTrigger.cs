using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLandTrigger : MonoBehaviour
{
    public bool isNearToGround = false;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform"
            || collision.tag == "EmergIsland" || collision.tag == "MaterialisedPlatform"
            || collision.tag == "MoveObject" || collision.tag == "CollapsingPlat"
            || collision.tag == "LavinaMovingPlat"
            || collision.tag == "TileMap")
        {
            isNearToGround = true;
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platform" || collision.tag == "CircleGhostPlatform"
            || collision.tag == "EmergIsland" || collision.tag == "MaterialisedPlatform"
            || collision.tag == "MoveObject" || collision.tag == "CollapsingPlat"
            || collision.tag == "throns" || collision.tag == "GhostPlatform"
            || collision.tag == "Circle" || collision.tag == "LavinaMovingPlat"
            || collision.tag == "CollectionArea" || collision.tag == "TileMap")
        {
            isNearToGround = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Platform"
            || collision.tag == "EmergIsland" || collision.tag == "MaterialisedPlatform"
            || collision.tag == "MoveObject" || collision.tag == "CollapsingPlat"
            || collision.tag == "LavinaMovingPlat"
            || collision.tag == "TileMap")
        {
            isNearToGround = true;
        }
        else
        {
            isNearToGround = false;
        }
    }*/

    void Update()
    {

    }
}
