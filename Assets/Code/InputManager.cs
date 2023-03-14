using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public bool isGoLeftButtonPressed = false;
    public bool isGoLeftButtonStopPress = false;
    public bool isGoRightButtonPressed = false;
    public bool isGoRightButtonStopPress = false;
    public bool isJumpButtonPressed = false;
    public bool isFirstAbilityButtonPressed = false;
    public bool isSecondAbilityButtonPressed = false;
    public bool isSecondAbilityButtonStopPress = false;
    public bool isRunMode = false;
    private float t = 0;
    public bool isAnyMoveButtonPressing = false;
    private bool isGoRightPressing = false;
    private bool isGoLeftPressing = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (isGoRightPressing || isGoLeftPressing)
        {
            isAnyMoveButtonPressing = true;
        }
        else
        {
            isAnyMoveButtonPressing = false;
        }
    }

    public void OnGoRightButtonPressed()
    {
        isGoRightButtonPressed = true;
        if (t < 0.3f)
        {
            isRunMode = true;
        }
        t = 0;
        isGoRightPressing = true;
    }

    public void OnGoRightButtonStopPress()
    {
        isGoRightButtonStopPress = true;
        isRunMode = false;
        isGoRightPressing = false;
    }

    public void OnGoLeftButtonPressed()
    {
        isGoLeftButtonPressed = true;
        if (t < 0.3f)
        {
            isRunMode = true;
        }
        t = 0;
        isGoLeftPressing = true;
    }

    public void OnGoLeftButtonStopPress()
    {
        isGoLeftButtonStopPress = true;
        isRunMode = false;
        isGoLeftPressing = false;
    }

    public void OnJumpButtonPressed()
    {
        isJumpButtonPressed = true;
    }
    public void OnFirstAbilityButtonPressed()
    {
        isFirstAbilityButtonPressed = true;
        Invoke("TurnOffFirstAbility", 0.1f);
    }

    private void TurnOffFirstAbility()
    {
        isFirstAbilityButtonPressed = false;
    }

    public void OnSecondAbilityButtonPressed()
    {
        isSecondAbilityButtonPressed = true;
    }

    public void OnSecondAbilityButtonStopPress()
    {
        isSecondAbilityButtonStopPress = true;
    }
}
