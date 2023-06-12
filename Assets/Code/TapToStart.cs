using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToStart : MonoBehaviour
{
    public GameObject background;
    public GameObject text;
    private float alpha = 1;
    private static bool isTapped = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isTapped)
        {
            alpha = 0;
            background.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            text.GetComponent<Text>().color = new Color(1, 1, 1, alpha);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isTapped && alpha > 0)
        {
            alpha -= Time.deltaTime * 2;
            background.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            text.GetComponent<Text>().color = new Color(1, 1, 1, alpha);
            if(alpha <= 0)
            {
                alpha = 0;
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTap()
    {
        isTapped = true;
    }
}
