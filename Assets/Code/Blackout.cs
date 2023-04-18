using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Blackout : MonoBehaviour
{
    private Light ambientLighting;
    private GameObject deerUnity;
    private BoxCollider2D coll;
    public static bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        ambientLighting = GameObject.Find("DirectionalLight").GetComponent<Light>();
        deerUnity = GameObject.Find("DeerUnity");
        coll = GetComponent<BoxCollider2D>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        var deer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
        if (coll.IsTouching(deer.GetComponent<BoxCollider2D>()))
        {
            ambientLighting.intensity = getIntensity(deer.transform.position.x);
        }
        if (isDead)
        {
            ambientLighting.intensity = 1;
            isDead = false;
        }
    }

    public float getIntensity(float deerX)
    {
        var thisWidth = transform.localScale.x;
        var startDarkingX = transform.position.x + thisWidth / 2;
        var endDarkingX = transform.position.x - thisWidth / 2;
        if (deerX > startDarkingX)
        {
            DeerUnity.isBlackoutNow = false;
            return 1;
        }
        if (deerX < endDarkingX)
        {
            DeerUnity.isBlackoutNow = true;
            return 0.1f;
        }
        var alpha = 1 - (startDarkingX - deerX) / (thisWidth);
        if (alpha < 0.1f || alpha > 1)
            return 0.1f;
        return alpha;
    }
}
