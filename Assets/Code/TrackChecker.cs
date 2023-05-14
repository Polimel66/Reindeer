using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackChecker : MonoBehaviour
{
    private GameObject deerUnity;
    private GameObject text;
    private Dictionary<string, Tuple<string, bool>> textDict;
    public static bool isChanged = false;
    private Timer timer;
    private bool isInArea = false;
    private Collider2D collision;
    private GameObject parent;
    private GameObject back;
    public GameObject InputManager;
    private GameObject firstHintBarier;
    private GameObject secondHintBarier;
    private GameObject thirdHintBarier;
    private GameObject fourthHintBarier;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Track")
        {
            this.collision = collision;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Track")
        {
            isInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Track")
        {
            isInArea = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        text = GameObject.Find("TracksText");
        firstHintBarier = GameObject.Find("FirstHintBarier");
        secondHintBarier = GameObject.Find("SecondHintBarier");
        thirdHintBarier = GameObject.Find("ThirdHintBarier");
        fourthHintBarier = GameObject.Find("FourthHintBarier");
        parent = text.transform.parent.Find("TracksTextParent").transform.gameObject;
        back = parent.transform.Find("TextBackground").gameObject;
        back.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        parent.GetComponent<Text>().text = "";

        text.GetComponent<Text>().text = "";
        textDict = new Dictionary<string, Tuple<string, bool>> { { "TrapFootprint 1", Tuple.Create("Сработавшая ловушка, их расставляют эти мерзавцы. \nКажется в неё уже кто-то попался. \nНужно быть внимательнее.", false) },
            { "WhirlwindFootprint 1", Tuple.Create("Здесь довольно ветренно. \nВетер опасен, но мама говорила, \nчто он может быть моим союзником.", false) },
            { "CasingsFootprint 1", Tuple.Create("Этим охотники стреляют в нас - пули. \nОни для нас очень опасны. Но если пуля попадет в хрупкую платформу, \nона может разрушиться и освободить путь.", false) },
            { "LittleShadow 8", Tuple.Create("Помню как с мамой отдыхал в тени.\n Было так прохладно.", false) },
            { "FallenStalactite 8", Tuple.Create("Он размером с меня, \nпод такой лучше не попадать.", false) },
            { "SmellOfDog 8", Tuple.Create("Я чувствую охотничьих псов, a с ними всегда охотники.\n Нужно срочно убегать отсюда.", false) }};
        timer = text.gameObject.GetComponent<Timer>();
        timer.SetPeriodForTick(5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed && isInArea)
        {
            var locationNumber = int.Parse(collision.gameObject.name.Split(' ')[1]);
            if (!textDict[collision.gameObject.name].Item2)
            {
                deerUnity.GetComponent<DeerUnity>().countOfFoundTracks += 1;
                if (deerUnity.GetComponent<DeerUnity>().countOfFoundTracks == 3)
                {
                    firstHintBarier.SetActive(false);
                }
                if (collision.gameObject.name == "LittleShadow 8")
                {
                    secondHintBarier.SetActive(false);
                }
                if (collision.gameObject.name == "FallenStalactite 8")
                {
                    thirdHintBarier.SetActive(false);
                }
                if (collision.gameObject.name == "SmellOfDog 8")
                {
                    fourthHintBarier.SetActive(false);
                }
                deerUnity.GetComponent<DeerUnity>().SetTask(locationNumber);
                textDict[collision.gameObject.name] = Tuple.Create(textDict[collision.gameObject.name].Item1, true);
            }
            text.GetComponent<Text>().text = textDict[collision.gameObject.name].Item1;

            parent.GetComponent<Text>().text = textDict[collision.gameObject.name].Item1;
            back.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            isChanged = true;
            timer.ClearTimer();
            timer.SetPeriodForTick(5f);
            timer.StartTimer();
            InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed = false;
        }

        var tick = timer.IsTicked();
        if (tick && !Hints.isChanged)
        {
            parent.GetComponent<Text>().text = "";
            back.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            text.GetComponent<Text>().text = "";
            timer.ClearTimer();
            timer.SetPeriodForTick(5f);
            timer.StopTimer();
            isChanged = false;
        }
        else if (tick && Hints.isChanged)
        {
            timer.ClearTimer();
            timer.SetPeriodForTick(5f);
            timer.StopTimer();
            isChanged = false;
        }
    }
}
