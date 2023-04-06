using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnEmerging : MonoBehaviour
{
    private BoxCollider2D collider;
    private int counter;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == 0)
        {

        }
    }
}
