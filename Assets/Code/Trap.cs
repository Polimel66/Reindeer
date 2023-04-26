using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Sprite trapReady;
    public Sprite trapWorking;
    public Sprite trapWorked;
    private Sprite[] trapAnimation;
    private GameObject deerUnity;
    public bool isTriggered = false;
    public AudioClip triggerSound;
    private int currentSpriteIndex = 0;
    private bool isAniFinished = false;
    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        trapAnimation = new Sprite[] { trapReady, trapWorking, trapWorked };
        deerUnity = GameObject.Find("DeerUnity");
        if (isTriggered)
        {
            Trigger();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered && !isAniFinished)
        {
            time += Time.deltaTime;
            if (time > 0.05f)
            {
                time -= 0.05f;
                currentSpriteIndex++;
                GetComponent<SpriteRenderer>().sprite = trapAnimation[currentSpriteIndex];
                if (currentSpriteIndex == 2)
                {
                    isAniFinished = true;
                    currentSpriteIndex = 0;
                }
            }
        }
    }

    public void ResetMe()
    {
        GetComponent<SpriteRenderer>().sprite = trapAnimation[currentSpriteIndex];
        isTriggered = false;
        isAniFinished = false;
    }

    private void Trigger()
    {
        isTriggered = true;
        //GetComponent<SpriteRenderer>().color = Color.blue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var name = collision.name.Split();
        if (name[0].Equals("TrapTrigger"))
        {
            if (name[1].Equals(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveTrapTrigger().name.Split()[1])
                && !isTriggered && DeerUnity.CurrentActive != 2)
            {
                Trigger();
                deerUnity.GetComponent<DeerUnity>().Trapped();
                var audio = gameObject.GetComponent<AudioSource>();
                audio.volume = 1f;
                if (DeerUnity.VolumeRatio == 0)
                {
                    audio.volume = 0;
                }
                else
                {
                    audio.volume = 1;
                }
                audio.PlayOneShot(triggerSound);
            }
        }
    }
}
