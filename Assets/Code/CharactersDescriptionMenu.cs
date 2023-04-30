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
        thirdDeerButton.GetComponent<Image>().color = defaultColor;
        firstAbilityButton.GetComponent<Image>().sprite = smallDeerFirstAbility;
        secondAbilityButton.GetComponent<Image>().sprite = smallDeerSecondAbility;
        deerImage.GetComponent<Image>().sprite = smallDeer;
        deerName.GetComponent<Text>().text = "ОЛЕНЁНОК";
        chosenDeer = ChosenDeer.Small;
        OnFirstAbilityButtonClick();

    }

    public void OnSecondDeerButtonClick()
    {
        firstDeerButton.GetComponent<Image>().color = defaultColor;
        secondDeerButton.GetComponent<Image>().color = chosenColor;
        thirdDeerButton.GetComponent<Image>().color = defaultColor;
        firstAbilityButton.GetComponent<Image>().sprite = ghostDeerFirstAbility;
        secondAbilityButton.GetComponent<Image>().sprite = ghostDeerSecondAbility;
        deerImage.GetComponent<Image>().sprite = ghostDeer;
        deerName.GetComponent<Text>().text = "ПРИЗРАК";
        chosenDeer = ChosenDeer.Ghost;
        OnFirstAbilityButtonClick();

    }

    public void OnThirdDeerButtonClick()
    {
        firstDeerButton.GetComponent<Image>().color = defaultColor;
        secondDeerButton.GetComponent<Image>().color = defaultColor;
        thirdDeerButton.GetComponent<Image>().color = chosenColor;
        firstAbilityButton.GetComponent<Image>().sprite = bigDeerFirstAbility;
        secondAbilityButton.GetComponent<Image>().sprite = bigDeerSecondAbility;
        deerImage.GetComponent<Image>().sprite = bigDeer;
        deerName.GetComponent<Text>().text = "СИЛАЧ(ЛОЛ)";
        chosenDeer = ChosenDeer.Big;
        OnFirstAbilityButtonClick();

    }

    public void OnFirstAbilityButtonClick()
    {
        firstAbilityButton.GetComponent<Image>().color = chosenColor;
        secondAbilityButton.GetComponent<Image>().color = defaultColor;
        if(chosenDeer == ChosenDeer.Small)
        {
            abilityDescription.GetComponent<Text>().text = "что-то про подбор предметов";
        }
        else if(chosenDeer == ChosenDeer.Ghost)
        {
            abilityDescription.GetComponent<Text>().text = "что-то про материализацию";
        }
        else if(chosenDeer == ChosenDeer.Big)
        {
            abilityDescription.GetComponent<Text>().text = "что-то про разрушение";
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
        abilityDescription.GetComponent<Text>().text = "";
        if (chosenDeer == ChosenDeer.Small)
        {
            abilityDescription.GetComponent<Text>().text = "что-то про нюх";
        }
        else if (chosenDeer == ChosenDeer.Ghost)
        {
            abilityDescription.GetComponent<Text>().text = "что-то про парение";
        }
        else if (chosenDeer == ChosenDeer.Big)
        {
            abilityDescription.GetComponent<Text>().text = "что-то перемещение";
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
