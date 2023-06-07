using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControlPoint : MonoBehaviour
{
    public GameObject mainCanvas;
    private GameObject ghost;
    public GameObject ghostPoint;
    private GameObject ani;
    public bool isStartMoving = false;
    public bool isEnd = false;
    private Vector3 ghostStartPoint;
    private float t = 0;
    private bool isActivateCameraTiedBefore = false;
    public static GameObject active;
    public bool isFirstControlPoint = false;
    private string levit = "Levitacia";
    private string polet = "Polet_gorizont";
    public int rot;
    public bool isLastControlPoint = false;
    //private static GameObject joystickDisabler;
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
            if(SaveManager.LastCheckPointName == null || int.Parse(SaveManager.LastCheckPointName.Split()[1]) < 2)
                Invoke("Disable", 3f);
        }
        /*if(joystickDisabler == null)
            joystickDisabler = GameObject.Find("JoystickDisabler");
        joystickDisabler.SetActive(false);*/
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
        if(mainCanvas != null && !mainCanvas.activeSelf)
        {
            mainCanvas.SetActive(true);
            //mainCanvas.transform.Find("Floating Joystick").gameObject.transform.localPosition += new Vector3(0, 5f, 0);
            //GameObject.Find("ReindeerSmall").GetComponent<ReindeerSmall>().EscapedTrap();
            //mainCanvas.transform.Find("Floating Joystick").gameObject.GetComponent<FloatingJoystick>().enabled = true;
            //joystickDisabler.SetActive(false);
            Invoke("TurnOnJoystick", 1f);
        }
            
    }

    private void TurnOnJoystick()
    {
        //mainCanvas.transform.Find("Floating Joystick").gameObject.SetActive(true);
    }

    private void TurnOnCameraTiedGhost()
    {
        //joystickDisabler.SetActive(true);
        if (mainCanvas != null)
            mainCanvas.SetActive(false);
        active.SetActive(true);
        DeerUnity.isCameraTiedGhost = true;
        
        //GameObject.Find("ReindeerSmall").GetComponent<ReindeerSmall>().Trapped();
        //GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().isNeedToUpdatePlatformsList = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!isStartMoving && collision.tag.Equals("Player") && !isEnd)
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

    public void FastMove()
    {
        ghost = transform.parent.parent.Find("Ghost").gameObject;
        ghostPoint = transform.parent.Find("GhostPoint").gameObject;
        ghost.transform.position = ghostPoint.transform.position;
        isEnd = true;
    }
}
