using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    enum Direction
    {
        Right,
        Left,
        No
    }
    public bool isGoLeftButtonPressed = false;
    public bool isGoRightButtonPressed = false;
    public bool isJumpButtonPressed = false;
    public bool isFirstAbilityButtonPressed = false;
    public bool isSecondAbilityButtonPressed = false;
    public bool isSecondAbilityButtonStopPress = false;
    public bool isRunMode = false;
    private float t = 0;
    private float targetFrameRateRefreshTime = 0;
    public bool isAnyMoveButtonPressing = false;
    private bool isGoRightPressing = false;
    private bool isGoLeftPressing = false;
    private bool isBoostPressed = false;
    private bool isWalkingPressed = false;
    public Joystick joystick;
    public GameObject joystickObject;
    public GameObject runButtonObject;
    public GameObject goLeftButtonObject;
    public GameObject goRightButtonObject;
    public GameObject jumpButtonObject;
    private Direction currentDirection = Direction.No;
    private Direction previousDirection = Direction.No;
    private GameObject canvas;
    public GameObject goLeftButton;
    public GameObject goRightButton;
    public GameObject jumpButton;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        if(MainMenu.TypeOfInputSystem == 0)
        {
            joystickObject.SetActive(true);
            runButtonObject.SetActive(true);

            goRightButtonObject.SetActive(false);
            goLeftButtonObject.SetActive(false);
            jumpButtonObject.SetActive(false);
        }
        else
        {
            goRightButtonObject.SetActive(true);
            goLeftButtonObject.SetActive(true);
            jumpButtonObject.SetActive(true);

            joystickObject.SetActive(false);
            runButtonObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetFrameRateRefreshTime += Time.deltaTime;
        if(targetFrameRateRefreshTime > 60)
        {
            Application.targetFrameRate = 60;
        }
        t += Time.deltaTime;
        if ((isGoRightPressing || isGoLeftPressing) && !isAnyMoveButtonPressing)
        {
            isAnyMoveButtonPressing = true;
        }
        else if(!isGoRightPressing && !isGoLeftPressing && isAnyMoveButtonPressing)
        {
            isAnyMoveButtonPressing = false;
            isRunMode = false;
        }
        
        if(MainMenu.TypeOfInputSystem == 0)
        {
            if (joystick.Horizontal > 0.25)
            {
                currentDirection = Direction.Right;

            }
            else if (joystick.Horizontal < -0.25)
            {
                currentDirection = Direction.Left;

            }
            else
            {
                currentDirection = Direction.No;

            }
            if (currentDirection != previousDirection)
            {
                if (currentDirection == Direction.Right)
                {
                    OnGoLeftButtonStopPressing();
                    OnGoRightButtonPressed();
                    
                    isWalkingPressed = true;
                    isGoRightPressing = true;
                    isGoLeftPressing = false;
                }
                else if (currentDirection == Direction.Left)
                {
                    OnGoRightButtonStopPressing();
                    OnGoLeftButtonPressed();
                    isWalkingPressed = true;
                    isGoRightPressing = false;
                    isGoLeftPressing = true;
                }
                else
                {
                    OnGoRightButtonStopPressing();
                    OnGoLeftButtonStopPressing();
                    isWalkingPressed = false;
                    isGoRightPressing = false;
                    isGoLeftPressing = false;
                }
                previousDirection = currentDirection;
            }
            if (joystick.Vertical > 0.5f && canvas.activeSelf)
            {
                if (DeerUnity.CurrentActive == 2)
                {
                    if (t > 0.25f)
                    {
                        OnJumpButtonPressed();
                        t = 0;
                    }
                }
                else
                {
                    if (t > 0.125f)
                    {
                        OnJumpButtonPressed();
                        t = 0;
                    }
                }


            }
            if (!DeerUnity.IsGrounded)
            {
                t = 0;
            }
        }
        
        if (isLavinaPlaying)
        {
            isLavinaPlaying = false;
            isWalkingPressed = false;
            isGoRightPressing = false;
            isGoLeftPressing = false;
        }
    }

    public void OnGoRightButtonPressed()
    {
        isGoRightButtonPressed = true;
        if (MainMenu.TypeOfInputSystem == 0 && isBoostPressed)
        {
            isRunMode = true;
        }
        if(MainMenu.TypeOfInputSystem == 1)
        {
            if (t < 0.3f)
            {
                isRunMode = true;
            }
            t = 0;
        }
        
        isWalkingPressed = true;
        isGoRightPressing = true;
    }

    public void OnGoRightButtonStopPressing()
    {
        isGoRightButtonPressed = false;
        isGoRightPressing = false;
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
        if (MainMenu.TypeOfInputSystem == 0 && isBoostPressed)
        {
            isRunMode = true;
        }
        if(MainMenu.TypeOfInputSystem == 1)
        {
            if (t < 0.3f)
            {
                isRunMode = true;
            }
            t = 0;
        }
        
        isGoLeftPressing = true;
        isWalkingPressed = true;
    }

    public void OnGoLeftButtonStopPressing()
    {
        isGoLeftButtonPressed = false;
        isGoLeftPressing = false;
    }

    public void OnJumpButtonPressed()
    {
        isJumpButtonPressed = true;
    }
    public void OnFirstAbilityButtonPressed()
    {
        isFirstAbilityButtonPressed = true;
        StartTutorialTrigger.isCollectPressed = true;
        Invoke("TurnOffFirstAbility", 0.1f);
    }

    private void TurnOffFirstAbility()
    {
        isFirstAbilityButtonPressed = false;
    }

    public void OnSecondAbilityButtonPressed()
    {
        isSecondAbilityButtonPressed = true;
        StartTutorialTrigger.isSmellPressed = true;
    }

    public void OnSecondAbilityButtonStopPress()
    {
        isSecondAbilityButtonStopPress = true;
    }

    public void UpdateInputButtonsOrJoystick()
    {
        OnGoLeftButtonStopPressing();
        OnGoRightButtonStopPressing();
        OnBoostStopPress();
        if (MainMenu.TypeOfInputSystem == 0)
        {
            var newJoystickObject = Instantiate(joystickObject, joystickObject.transform.parent);
            var oldJoystickObject = joystickObject;
            joystickObject = newJoystickObject;
            joystick = joystickObject.GetComponent<Joystick>();
            joystickObject.transform.SetAsFirstSibling();
            DestroyObjects(new List<GameObject> { oldJoystickObject });
            //Destroy(joystickObject);
            //Destroy(newJoystickObject);
        }
        else
        {
            var newGoLeftButton = Instantiate(goLeftButton, goLeftButton.transform.parent);
            var oldGoLeftButton = goLeftButton;
            goLeftButton = newGoLeftButton;
            //joystick = joystickObject.GetComponent<Joystick>();
            goLeftButton.transform.SetAsFirstSibling();

            var newGoRightButton = Instantiate(goRightButton, goRightButton.transform.parent);
            var oldGoRightButton = goRightButton;
            goRightButton = newGoRightButton;
            //joystick = joystickObject.GetComponent<Joystick>();
            goRightButton.transform.SetAsFirstSibling();

            var newJumpButton = Instantiate(jumpButton, jumpButton.transform.parent);
            var oldJumpButton = jumpButton;
            jumpButton = newJumpButton;
            //joystick = joystickObject.GetComponent<Joystick>();
            jumpButton.transform.SetAsFirstSibling();


            DestroyObjects(new List<GameObject> { oldGoLeftButton, oldGoRightButton, oldJumpButton });
        }
        
    }

    private void DestroyObjects(List<GameObject> objects)
    {
        for(var i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }
    }

    public static bool isLavinaPlaying = false;
}
