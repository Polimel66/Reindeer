using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavinaWarmUp : MonoBehaviour
{
    private GameObject deerUnity;
    private GameObject canvas;
    private GameObject input;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        canvas = GameObject.Find("Canvas");
        input = GameObject.Find("InputManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            //deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().Trapped();
            canvas.SetActive(false);
            input.GetComponent<InputManager>().OnBoostStopPress();
            input.GetComponent<InputManager>().OnGoLeftButtonStopPressing();
            input.GetComponent<InputManager>().OnGoRightButtonPressed();
        }
    }
}
