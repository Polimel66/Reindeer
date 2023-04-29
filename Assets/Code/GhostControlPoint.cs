using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControlPoint : MonoBehaviour
{
    private GameObject ghost;
    private GameObject ghostPoint;
    private GameObject ani;
    private bool isStartMoving = false;
    private bool isEnd = false;
    private Vector3 ghostStartPoint;
    private float t = 0;
    private bool isActivateCameraTiedBefore = false;
    private static GameObject active;
    public bool isFirstControlPoint = false;
    private string levit = "Levitacia";
    private string polet = "Polet_gorizont";
    public int rot;
    public bool isLastControlPoint = false;
    // Start is called before the first frame update
    void Start()
    {
        ghost = transform.parent.parent.Find("Ghost").gameObject;
        ghostPoint = transform.parent.Find("GhostPoint").gameObject;
        ani = ghost.transform.Find("Animation").gameObject;
        ghostStartPoint = ghost.transform.position;
        if (active == null)
        {
            active = GameObject.Find("ActivatedSectionMap");
            Invoke("Disable", 3f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartMoving && !isEnd)
        {
            t += Time.deltaTime / 7;
            ghost.transform.position = Vector3.Lerp(ghostStartPoint, ghostPoint.transform.position, t);
            if (ghost.transform.position.Equals(ghostPoint.transform.position))
            {
                ani.GetComponent<SkeletonAnimation>().AnimationName = levit;
                ani.transform.localPosition = new Vector3(-1.69f, -3.22f, 0);
                ani.transform.localEulerAngles = new Vector3(0, 0, 0);
                if(DeerUnity.CurrentActive == 1)
                {
                    GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().StartMoving();
                }
                
                isEnd = true;
                Invoke("TurnOffCameraTiedGhost", 1f);
            }
        }
        
    }

    private void Disable()
    {
        active.SetActive(false);
    }

    private void TurnOffCameraTiedGhost()
    {
        DeerUnity.isCameraTiedGhost = false;
    }

    private void TurnOnCameraTiedGhost()
    {
        active.SetActive(true);
        DeerUnity.isCameraTiedGhost = true;
        //GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().isNeedToUpdatePlatformsList = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!isStartMoving && collision.tag.Equals("Player"))
        {
            isStartMoving = true;
            ghostStartPoint = ghost.transform.position;
            var delta = ghostPoint.transform.position.x - ghostStartPoint.x;
            if (delta > 0)
            {
                ghost.transform.localScale = new Vector3(0.28f, 0.28f, 1);
            }
            if (delta < 0)
            {
                ghost.transform.localScale = new Vector3(-0.28f, 0.28f, 1);
            }
            if (!isLastControlPoint)
            {
                ani.GetComponent<SkeletonAnimation>().AnimationName = polet;
                ani.transform.localPosition = new Vector3(0.08f, -2.5f, 0);
                ani.transform.localEulerAngles = new Vector3(0, 0, rot);
            }
            if (gameObject.transform.tag == "moveCamera" && !isActivateCameraTiedBefore)
            {
                if (DeerUnity.CurrentActive == 1)
                {
                    GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().StopMoving();
                }
                isActivateCameraTiedBefore = true;
                TurnOnCameraTiedGhost();
                GameObject.Find("FirstGroupHints").SetActive(false);
                GameObject.Find("MovingPlatforms").SetActive(false);  
            }
        }
    }
}
