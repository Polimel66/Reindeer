using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticHunter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject deerUnity;
    public int direction = 1;
    private GameObject tilemap1;
    private GameObject tilemap2;
    private float previousTime;
    private bool isCanMoving = true;
    private float shootTime = 0;
    private float standingTime = 0;
    private bool isStayAtPoint = false;
    public bool isEnabled;
    public AudioClip shootSound;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();
        deerUnity = GameObject.Find("DeerUnity");

        tilemap1 = GameObject.Find("Tilemap1");
        tilemap2 = GameObject.Find("Tilemap2");
    }

    void Update()
    {
        MakeAction();
        FlipPlayer();
    }

    public void MakeAction()
    {
        var deltaX = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.x - transform.position.x;
        var deltaY = Math.Abs(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.y - transform.position.y);
        if (deltaX > 0)
        {
            direction = 1;
        }
        else if (deltaX < 0)
        {
            direction = -1;
        }
        if (deltaX > -7 && deltaX < 7)
        {
            if (GetComponent<Timer>().GetTime() - shootTime > 3f)
            {
                Shoot();
                shootTime = GetComponent<Timer>().GetTime();
            }
        }
    }

    private void Shoot()
    {
        var audio = GetComponent<AudioSource>();
        if (DeerUnity.VolumeRatio == 0)
        {
            audio.volume = 0;
        }
        else
        {
            audio.volume = 0.1f;
        }
        audio.PlayOneShot(shootSound);
        var newBullet = GameObject.Instantiate(GameObject.Find("HunterKit1").transform.Find("Bullet").gameObject, transform.position, transform.rotation);
        newBullet.GetComponent<Bullet>().isDestroyOnTileCollision = false;
        newBullet.GetComponent<Bullet>().GoToDeer();
    }

    public void FlipPlayer()
    {
        if (direction < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
        if (direction > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
    }
}