using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavinaWarmUp : MonoBehaviour
{
    public enum State
    {
        Start,
        End,
        None
    }
    private GameObject deerUnity;
    private GameObject canvas;
    private GameObject input;
    public State state;
    public bool isActivatingLavina = false;
    public bool isSnowIn = false;
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
        if (collision.tag.Equals("Player") && state == State.Start)
        {
            //deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().Trapped();
            canvas.SetActive(false);
            input.GetComponent<InputManager>().OnBoostStopPress();
            input.GetComponent<InputManager>().OnGoLeftButtonStopPressing();
            input.GetComponent<InputManager>().OnGoRightButtonPressed();
            
            
        }
        if (collision.tag.Equals("Player") && state == State.None && isSnowIn)
        {
            //deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().Trapped();
            deerUnity.GetComponent<DeerUnity>().isBited = true;
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().SnowIn();

        }
        if (collision.tag.Equals("Player") && state == State.End)
        {
            //deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().Trapped();
            //canvas.SetActive(false);
            input.GetComponent<InputManager>().OnBoostStopPress();
            input.GetComponent<InputManager>().OnGoLeftButtonStopPressing();
            input.GetComponent<InputManager>().OnGoRightButtonStopPressing();
            deerUnity.GetComponent<DeerUnity>().isBited = false;
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().SnowOut();
        }
        if (collision.tag.Equals("Player") && state == State.None && isActivatingLavina)
        {
            GameObject.Find("AvalancheIsland").GetComponent<AvalancheIsland>().PlayLavinaAni();
        }
    }
}
