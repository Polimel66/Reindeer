using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private GameObject deerUnity;
    private bool isTriggered = false;
    public AudioClip bushInSound;
    private AudioSource audio;
    public GameObject InputManager;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        audio = GetComponent<AudioSource>();
        audio.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed && isTriggered)
        {
            InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed = false;

            if (DeerUnity.VolumeRatio == 0)
            {
                audio.volume = 0;
            }
            else
            {
                audio.volume = 1;
            }
            audio.PlayOneShot(bushInSound);
            isTriggered = false;
            deerUnity.GetComponent<DeerUnity>().UnBushed(gameObject);
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        }
        else if (InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed && !isTriggered && DeerUnity.CurrentActive == 1
            && GetComponent<PolygonCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
        {
            InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed = false;
            if (DeerUnity.VolumeRatio == 0)
            {
                audio.volume = 0;
            }
            else
            {
                audio.volume = 1;
            }
            audio.PlayOneShot(bushInSound);
            isTriggered = true;
            deerUnity.GetComponent<DeerUnity>().Bushed(gameObject);
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.5f);
        }
    }
}
