using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersDescriptionMenu : MonoBehaviour
{
    enum ChosenDeer
    {
        Small,
        Ghost,
        Big
    }

    private Color chosenColor = new Color(1, 1, 1, 1);
    private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 1);
    public GameObject firstDeerButton;
    public GameObject secondDeerButton;
    public GameObject thirdDeerButton;
    public GameObject firstAbilityButton;
    public GameObject secondAbilityButton;
    public Sprite smallDeerFirstAbility;
    public Sprite smallDeerSecondAbility;
    public Sprite ghostDeerFirstAbility;
    public Sprite ghostDeerSecondAbility;
    public Sprite bigDeerFirstAbility;
    public Sprite bigDeerSecondAbility;
    public Sprite smallDeer;
    public Sprite ghostDeer;
    public Sprite bigDeer;
    public Sprite smallName;
    public Sprite ghostName;
    public Sprite firstSmallDescription;
    public Sprite secondSmallDescription;
    public Sprite firstGhostDescription;
    public Sprite secondGhostDescription;

    public GameObject deerImage;
    public GameObject deerName;
    public GameObject abilityDescription;
    private ChosenDeer chosenDeer;


    // Start is called before the first frame update
    void Start()
    {
        OnFirstDeerButtonClick();
        OnFirstAbilityButtonClick();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFirstDeerButtonClick()
    {
        firstDeerButton.GetComponent<Image>().color = chosenColor;
        secondDeerButton.GetComponent<Image>().color = defaultColor;
        //thirdDeerButton.GetComponent<Image>().color = defaultColor;
        firstAbilityButton.GetComponent<Image>().sprite = smallDeerFirstAbility;
        secondAbilityButton.GetComponent<Image>().sprite = smallDeerSecondAbility;
        deerImage.GetComponent<Image>().sprite = smallDeer;
        deerName.GetComponent<Text>().text = "ОЛЕНЁНОК";
        //deerName.GetComponent<Image>().sprite = smallName;
        chosenDeer = ChosenDeer.Small;
        OnFirstAbilityButtonClick();

    }

    public void OnSecondDeerButtonClick()
    {
        firstDeerButton.GetComponent<Image>().color = defaultColor;
        secondDeerButton.GetComponent<Image>().color = chosenColor;
        //thirdDeerButton.GetComponent<Image>().color = defaultColor;
        firstAbilityButton.GetComponent<Image>().sprite = ghostDeerFirstAbility;
        secondAbilityButton.GetComponent<Image>().sprite = ghostDeerSecondAbility;
        deerImage.GetComponent<Image>().sprite = ghostDeer;
        //deerName.GetComponent<Image>().sprite = ghostName;
        deerName.GetComponent<Text>().text = "ПРИЗРАК";
        chosenDeer = ChosenDeer.Ghost;
        OnFirstAbilityButtonClick();

    }

    public void OnThirdDeerButtonClick()
    {
        //firstDeerButton.GetComponent<Image>().color = defaultColor;
        //secondDeerButton.GetComponent<Image>().color = defaultColor;
        //thirdDeerButton.GetComponent<Image>().color = chosenColor;
        //firstAbilityButton.GetComponent<Image>().sprite = bigDeerFirstAbility;
        //secondAbilityButton.GetComponent<Image>().sprite = bigDeerSecondAbility;
        //deerImage.GetComponent<Image>().sprite = bigDeer;
        //deerName.GetComponent<Text>().text = "СИЛАЧ";
        //chosenDeer = ChosenDeer.Big;
        //OnFirstAbilityButtonClick();

    }

    public void OnFirstAbilityButtonClick()
    {
        firstAbilityButton.GetComponent<Image>().color = chosenColor;
        secondAbilityButton.GetComponent<Image>().color = defaultColor;
        if(chosenDeer == ChosenDeer.Small)
        {
            //abilityDescription.GetComponent<Text>().text = "ВЗАИМОДЕЙСТВИЕ\n" +
            //                                               "-----------------------------\n" +
            //                                               "При активации позволяет\nперсонажу взаимодействовать\nс игровыми сущностями.";
            abilityDescription.GetComponent<Image>().sprite = firstSmallDescription;
        }
        else if(chosenDeer == ChosenDeer.Ghost)
        {
            //abilityDescription.GetComponent<Text>().text = "МАТЕРИАЛИЗАЦИЯ\n" +
            //                                               "-----------------------------\n" +
            //                                               "Позволяет персонажу делать\nплатформы активными для\nостальных оленей. Один вызов\nпризрака = 1 платформа.";
            abilityDescription.GetComponent<Image>().sprite = firstGhostDescription;
        }
        else if(chosenDeer == ChosenDeer.Big)
        {
            //abilityDescription.GetComponent<Text>().text = "ПОГРОМ\n" +
            //                                               "-------------\n" +
            //                                               "При активации позволяет\n" +
            //                                               "персонажу ломать предметы\n" +
            //                                               "с трещинами.";
        }
        else
        {
            abilityDescription.GetComponent<Text>().text = "error(";
        }

    }

    public void OnSecondAbilityButtonClick()
    {
        firstAbilityButton.GetComponent<Image>().color = defaultColor;
        secondAbilityButton.GetComponent<Image>().color = chosenColor;
        if (chosenDeer == ChosenDeer.Small)
        {
            //abilityDescription.GetComponent<Text>().text = "СЛЕДЫ\n" +
            //                                               "-----------\n" +
            //                                               "Ограничивает видимость\nперсонажа, а также\nподсвечивает следы.";
            abilityDescription.GetComponent<Image>().sprite = secondSmallDescription;
        }
        else if (chosenDeer == ChosenDeer.Ghost)
        {
            //abilityDescription.GetComponent<Text>().text = "ЛЕВИТАЦИЯ\n" +
            //                                               "-------------------\n" +
            //                                               "Персонаж может медленно\n" +
            //                                               "скользить по направлению\n" +
            //                                               "к земле.";
            abilityDescription.GetComponent<Image>().sprite = secondGhostDescription;
        }
        else if (chosenDeer == ChosenDeer.Big)
        {
            //abilityDescription.GetComponent<Text>().text = "КИНЕЗ\n" +
            //                                               "----------\n" +
            //                                               "Удерживайте кнопку\n" +
            //                                               "способности и идите в нужную\n" +
            //                                               "сторону, чтобы передвигать\n" +
            //                                               "подсвеченные предметы.";
        }
        else
        {
            abilityDescription.GetComponent<Text>().text = "error(";
        }

    }

    public void OnReturnButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
