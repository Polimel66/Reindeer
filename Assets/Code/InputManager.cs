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
    private bool isBoostPressed = false;
    private bool isWalkingPressed = false;
    public Joystick joystick;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        t += Time.deltaTime;
        if ((isGoRightPressing || isGoLeftPressing) && !isAnyMoveButtonPressing)
        {
            isAnyMoveButtonPressing = true;
        }
        else if(!isGoRightPressing && !isGoLeftPressing && isAnyMoveButtonPressing)
        {
            isAnyMoveButtonPressing = false;
        }
        
        
        if (joystick.Horizontal > 0.25)
        {
            OnGoLeftButtonStopPress();
            OnGoRightButtonPressed();
            isWalkingPressed = true;
            isGoRightPressing = true;
            isGoLeftPressing = false;
        }
        else if (joystick.Horizontal < -0.25)
        {
            OnGoRightButtonStopPress();
            OnGoLeftButtonPressed();
            isWalkingPressed = true;
            isGoRightPressing = false;
            isGoLeftPressing = true;
        }
        else
        {
            OnGoLeftButtonStopPress();
            OnGoRightButtonStopPress();
            isWalkingPressed = false;
            isGoRightPressing = false;
            isGoLeftPressing = false;
        }
        if (joystick.Vertical > 0.5f)
        {
            if (t > 0.3f)
            {
                OnJumpButtonPressed();
                t = 0;
            }
            
        }
    }

    public void OnGoRightButtonPressed()
    {
        isGoRightButtonPressed = true;
        if (isBoostPressed)
        {
            isRunMode = true;
        }
        //if (t < 0.3f)
        //{
        //    isRunMode = true;
        //}
        //t = 0;
        isWalkingPressed = true;
        isGoRightPressing = true;
    }

    public void OnGoRightButtonStopPress()
    {
        isGoRightButtonStopPress = true;
        isRunMode = false;
        isGoRightPressing = false;
        isWalkingPressed = false;
    }

    public void OnBoostPressed()
    {
        isBoostPressed = true;
        if (isWalkingPressed)
        {
            isRunMode = true;
        }
    }

    public void OnBoostStopPress()
    {
        isBoostPressed = false;
        isRunMode = false;
    }
    public void OnGoLeftButtonPressed()
    {
        isGoLeftButtonPressed = true;
        if (isBoostPressed)
        {
            isRunMode = true;
        }
        //if (t < 0.3f) 
        //{
        //    isRunMode = true;
        //}
        //t = 0;
        isGoLeftPressing = true;
        isWalkingPressed = true;
    }

    public void OnGoLeftButtonStopPress()
    {
        isGoLeftButtonStopPress = true;
        isRunMode = false;
        isGoLeftPressing = false;
        isWalkingPressed = false;
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
