using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangerDeer : MonoBehaviour
{
    public GameObject circleSmall;
    public GameObject circleGhost;
    public GameObject circleBig;
    // Start is called before the first frame update
    private GameObject deerUnity;
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        if (DeerUnity.CurrentActive == 1)
        {
            circleSmall.GetComponent<Image>().color = Color.white;
            circleSmall.GetComponent<RectTransform>().localScale = new Vector3(130f, 130f, 1f);
            circleGhost.GetComponent<Image>().color = Color.grey;
            circleGhost.GetComponent<RectTransform>().localScale = new Vector3(94.5f, 94.5f, 1f);
            circleBig.GetComponent<Image>().color = Color.grey;
            circleBig.GetComponent<RectTransform>().localScale = new Vector3(94.5f, 94.5f, 1f);
        }
        else if (DeerUnity.CurrentActive == 2)
        {
            circleSmall.GetComponent<Image>().color = Color.grey;
            circleSmall.GetComponent<RectTransform>().localScale = new Vector3(94.5f, 94.5f, 1f);
            circleGhost.GetComponent<Image>().color = Color.white;
            circleGhost.GetComponent<RectTransform>().localScale = new Vector3(130f, 130f, 1f);
            circleBig.GetComponent<Image>().color = Color.grey;
            circleBig.GetComponent<RectTransform>().localScale = new Vector3(94.5f, 94.5f, 1f);
        }
        else if (DeerUnity.CurrentActive == 3)
        {
            circleSmall.GetComponent<Image>().color = Color.grey;
            circleSmall.GetComponent<RectTransform>().localScale = new Vector3(94.5f, 94.5f, 1f);
            circleGhost.GetComponent<Image>().color = Color.grey;
            circleGhost.GetComponent<RectTransform>().localScale = new Vector3(94.5f, 94.5f, 1f);
            circleBig.GetComponent<Image>().color = Color.white;
            circleBig.GetComponent<RectTransform>().localScale = new Vector3(130f, 130f, 1f);
        }
    }
}
