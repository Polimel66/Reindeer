using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangerDeer : MonoBehaviour
{
    public GameObject circleSmall;
    public GameObject circleGhost;
    public GameObject circleBig;
    public GameObject currentActiveCircle;
    public bool isSecondOn;
    public bool isThirdOn;
    // Start is called before the first frame update
    private GameObject deerUnity;
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        isSecondOn = false;
        isThirdOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSecondOn && DeerUnity.isFirstLocationComplete)
        {
            circleGhost.transform.GetChild(2).gameObject.SetActive(false);
            isSecondOn = true;
        }
        if (!isThirdOn && DeerUnity.isThirdDeerComplete)
        {
            circleBig.transform.GetChild(2).gameObject.SetActive(false);
            isThirdOn = true;
        }
        if (DeerUnity.CurrentActive == 1)
        {
            currentActiveCircle.GetComponent<Image>().color = Color.grey;
            currentActiveCircle.GetComponent<RectTransform>().localScale = new Vector3(76.4f, 76.4f, 1f);
            var center = currentActiveCircle.GetComponent<RectTransform>().position;
            if (center.x != circleSmall.GetComponent<RectTransform>().position.x)
            {
                currentActiveCircle.GetComponent<RectTransform>().position -= new Vector3(center.x - circleSmall.GetComponent<RectTransform>().position.x, center.y - circleSmall.GetComponent<RectTransform>().position.y, 0);
                circleSmall.GetComponent<RectTransform>().position += new Vector3(center.x - circleSmall.GetComponent<RectTransform>().position.x, center.y - circleSmall.GetComponent<RectTransform>().position.y, 0);
            }
            circleSmall.GetComponent<Image>().color = Color.white;
            circleSmall.GetComponent<RectTransform>().localScale = new Vector3(171.6f, 171.6f, 1f);
            currentActiveCircle = circleSmall;
        }
        else if (DeerUnity.CurrentActive == 2)
        {
            currentActiveCircle.GetComponent<Image>().color = Color.grey;
            currentActiveCircle.GetComponent<RectTransform>().localScale = new Vector3(76.4f, 76.4f, 1f);
            var center = currentActiveCircle.GetComponent<RectTransform>().position;
            if (center.x != circleGhost.GetComponent<RectTransform>().position.x)
            {
                currentActiveCircle.GetComponent<RectTransform>().position -= new Vector3(center.x - circleGhost.GetComponent<RectTransform>().position.x, center.y - circleGhost.GetComponent<RectTransform>().position.y, 0);
                circleGhost.GetComponent<RectTransform>().position += new Vector3(center.x - circleGhost.GetComponent<RectTransform>().position.x, center.y - circleGhost.GetComponent<RectTransform>().position.y, 0);
            }
            circleGhost.GetComponent<Image>().color = Color.white;
            circleGhost.GetComponent<RectTransform>().localScale = new Vector3(171.6f, 171.6f, 1f);
            currentActiveCircle = circleGhost;
        }
        else if (DeerUnity.CurrentActive == 3)
        {
            currentActiveCircle.GetComponent<Image>().color = Color.grey;
            currentActiveCircle.GetComponent<RectTransform>().localScale = new Vector3(76.4f, 76.4f, 1f);
            var center = currentActiveCircle.GetComponent<RectTransform>().position;
            if (center.x != circleBig.GetComponent<RectTransform>().position.x)
            {
                currentActiveCircle.GetComponent<RectTransform>().position -= new Vector3(center.x - circleBig.GetComponent<RectTransform>().position.x, center.y - circleBig.GetComponent<RectTransform>().position.y, 0);
                circleBig.GetComponent<RectTransform>().position += new Vector3(center.x - circleBig.GetComponent<RectTransform>().position.x, center.y - circleBig.GetComponent<RectTransform>().position.y, 0);
            }
            circleBig.GetComponent<Image>().color = Color.white;
            circleBig.GetComponent<RectTransform>().localScale = new Vector3(171.6f, 171.6f, 1f);
            currentActiveCircle = circleBig;
        }
    }
}
