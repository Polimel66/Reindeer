using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticHunter : MonoBehaviour
{
    public GameObject laser;
    public GameObject circle;
    private bool isCanShooting;
    public AudioClip shootSound;
    private float t = 0;
    private float zRotation = 0;
    private Direction dir = Direction.Right;
    private float angleLimit = 35;
    private float rotationSpeed = 15;
    private GameObject deerUnity;
    private bool isShooted = false;
    // Start is called before the first frame update
    void Start()
    {
        isCanShooting = true;
        //laser.transform.localPosition = new Vector2(0, -laser.transform.localScale.y/2);
        //laser.transform.localEulerAngles = new Vector3(0, 0, 0);
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        t = Time.deltaTime;
        if (dir == Direction.Right)
        {
            if(zRotation < angleLimit)
            {
                zRotation += t * rotationSpeed;
            }
            else
            {
                dir = Direction.Left;
            }
        }
        else
        {
            if (zRotation > -angleLimit)
            {
                zRotation -= t * rotationSpeed;
            }
            else
            {
                dir = Direction.Right;
            }
        }
        circle.transform.localEulerAngles = new Vector3(0, 0, zRotation);
        if (!isShooted 
            && laser.GetComponent<Laser>().isTouching
            && !deerUnity.GetComponent<DeerUnity>().isBushed)
        {
            Shoot();
            isShooted = true;
            deerUnity.GetComponent<DeerUnity>().CatchDeer();
        }
    }

    private void Shoot()
    {
        if (isCanShooting)
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
            newBullet.GetComponent<Bullet>().GoToDeer(20);
        }
    }

    enum Direction{
        Right,
        Left
    }
}
