using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingHunter : MonoBehaviour
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
    public GameObject laserTargetPoint;
    private bool isStopRotation = false;
    private float tForRaycast = 0;
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
        FlipPlayer();
        t = Time.deltaTime;
        if (!isStopRotation)
        {
            if (dir == Direction.Right)
            {
                if (zRotation < angleLimit)
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
        }
        tForRaycast += Time.deltaTime;
        if(tForRaycast > 0.033f)
        {
            tForRaycast = 0;
            var directionOfRay = new Vector2(10 * Mathf.Sin(zRotation * Mathf.PI / 180), -10 * Mathf.Sin((90 - Mathf.Abs(zRotation)) * Mathf.PI / 180));
            var origin = new Vector2(circle.transform.position.x, circle.transform.position.y);
            var hits = Physics2D.RaycastAll(origin, directionOfRay);
            var distance = 0.0f;
            var dis = new Vector2();
            foreach (var hit in hits)
            {
                if (hit.collider.tag.Equals("LaserBound"))
                {
                    distance = hit.distance;
                    laserTargetPoint.transform.position = hit.point;
                    dis = hit.point;

                    //Debug.DrawLine(circle.transform.position, hit.point, Color.red);

                    break;
                }
            }
            if (distance != 0)
            {
                var scaleRatio = laser.transform.parent.parent.localScale.x;
                distance += 0.2f;
                //distance = Mathf.Sqrt((circle.transform.position.x - dis.x) * (circle.transform.position.x - dis.x) + (circle.transform.position.y - dis.y) * (circle.transform.position.y - dis.y));
                laser.transform.localScale = new Vector3(0.1f, distance / scaleRatio, 1);
                laser.transform.localPosition = new Vector3(0, (-distance / 2) / scaleRatio, 0);
            }
            else
            {
                laser.transform.localScale = new Vector3(0.1f, 20, 1);
                laser.transform.localPosition = new Vector3(0, -10, 0);
            }
            circle.transform.localEulerAngles = new Vector3(0, 0, zRotation);
            if (!isShooted
                && laser.GetComponent<Laser>().isTouching
                && !deerUnity.GetComponent<DeerUnity>().isBushed)
            {
                isStopRotation = true;
                Shoot();
                isShooted = true;
                //deerUnity.GetComponent<DeerUnity>().CatchDeer();
                deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().Trapped();
                Invoke("UnShoot", 5f);
                Invoke("UnStopRotation", 5f);
            }
        }
        
    }

    private void UnShoot()
    {
        isShooted = false;
    }

    private void UnStopRotation()
    {
        isStopRotation = false;
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
            newBullet.GetComponent<Bullet>().isDestroyOnTileCollision = false;
            newBullet.GetComponent<Bullet>().GoToDeer(50, 1000);
        }
    }

    public void FlipPlayer()
    {
        if (zRotation < 0 && !GetComponent<SpriteRenderer>().flipX)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (zRotation > 0 && GetComponent<SpriteRenderer>().flipX)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    enum Direction{
        Right,
        Left
    }
}
