using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D coll;
    private GameObject deerUnity;
    private Color defaultColor = new Color(0, 0, 0, 0);
    private bool isInAbyss = false;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0);
        coll = GetComponent<BoxCollider2D>();
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (isInAbyss)
        {
            var deer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
            sr.color = new Color(0, 0, 0, GetAlpha(deer.transform.position.y));
        }
        else
        {
            if (!sr.color.Equals(defaultColor))
            {
                sr.color = defaultColor;
            }
        }
    }

    private float GetAlpha(float deerY)
    {
        var thisHeight = transform.localScale.y;
        var startDarkingY = transform.position.y + (thisHeight / 2);
        var endDarkingY = transform.position.y + (thisHeight / 4);
        if (deerY > startDarkingY)
        {
            return 0;
        }
        var alpha = 1 - (deerY - endDarkingY) / (thisHeight / 3);
        if (deerY < endDarkingY && time > 5)
        {
            deerUnity.GetComponent<DeerUnity>().TakeDamage(1000);
            time = 0;
        }
        if ((alpha < 0 || alpha > 1) && !isInAbyss)
            return 0;
        else if ((alpha < 0 || alpha > 1) && isInAbyss)
            return 1;
        return alpha;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            isInAbyss = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            isInAbyss = false;
        }
    }
}
