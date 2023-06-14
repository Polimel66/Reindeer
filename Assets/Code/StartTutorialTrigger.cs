using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorialTrigger : MonoBehaviour
{
    public GameObject smellTapAni;
    public GameObject collectTapAni;
    private int index = -1;
    private bool isStarted = false;
    public static bool isCollectPressed = false;
    public static bool isSmellPressed = false;
    private bool isSmellUsedOnFirstVar = false;
    private GameObject deerUnity;
    public static bool isPlayingTutorial = false;
    private float tCollect = 0;
    private float tSmell = 0;
    private bool isCollectResetTimeToZero = false;
    private bool isSmellResetTimeToZero = false;
    private bool isPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        if (SaveManager.LastCheckPointName != null && int.Parse(SaveManager.LastCheckPointName.Split()[1]) > 0)
        {
            isPlayed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            if (index == 0 && !smellTapAni.activeSelf)
            {
                smellTapAni.SetActive(true);
            }
            if (index == 0 && smellTapAni.activeSelf && isSmellPressed)
            {
                smellTapAni.SetActive(false);
                isStarted = false;
                deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().StartMoving();
                isPlayingTutorial = false;
                isPlayed = true;
            }


            if (index == 1 && !smellTapAni.activeSelf && !isSmellUsedOnFirstVar && ReindeerSmall.isSmell)
            {
                smellTapAni.SetActive(false);
                isSmellUsedOnFirstVar = true;
            }

            if (index == 1 && !smellTapAni.activeSelf && !isSmellUsedOnFirstVar && !ReindeerSmall.isSmell)
            {
                smellTapAni.SetActive(true);
            }

            if (index == 1 && smellTapAni.activeSelf && isSmellPressed && !isSmellUsedOnFirstVar)
            {
                smellTapAni.SetActive(false);
                isSmellUsedOnFirstVar = true;
            }


            if (index == 1 && !collectTapAni.activeSelf && isSmellUsedOnFirstVar && ReindeerSmall.isSmell)
            {
                collectTapAni.SetActive(true);
            }

            if (index == 1 && collectTapAni.activeSelf && isSmellUsedOnFirstVar && !ReindeerSmall.isSmell)
            {
                collectTapAni.SetActive(false);
                isSmellUsedOnFirstVar = false;
            }

            if (index == 1 && collectTapAni.activeSelf && isCollectPressed && isSmellUsedOnFirstVar)
            {
                collectTapAni.SetActive(false);
                isStarted = false;
                deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().StartMoving();
                isPlayingTutorial = false;
                isPlayed = true;
            }
        }
        tCollect += Time.deltaTime;
        tSmell += Time.deltaTime;
        if (isCollectPressed && !isCollectResetTimeToZero)
        {
            tCollect = 0;
            isCollectResetTimeToZero = true;
        }
        if (isCollectPressed && isCollectResetTimeToZero && tCollect > 0.5f)
        {
            isCollectResetTimeToZero = false;
            isCollectPressed = false;
        }

        if (isSmellPressed && !isSmellResetTimeToZero)
        {
            tSmell = 0;
            isSmellResetTimeToZero = true;
        }
        if (isSmellPressed && isSmellResetTimeToZero && tSmell > 0.5f)
        {
            isSmellResetTimeToZero = false;
            isSmellPressed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !isPlayed)
        {
            index = int.Parse(name.Split()[1]);
            isStarted = true;
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().StopMoving();
            isPlayingTutorial = true;
        }
    }
}
