using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReindeerSmall : MonoBehaviour
{
    // Start is called before the first frame update
    public float CurrentHorizontalVelocity { get; private set; } = 0;
    public float CurrentVerticalVelocity { get; private set; } = 0;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Light ambientLighting;
    private Light deerLighting;
    private int shiftRatio = 1;
    public bool isRunning = false;
    public float horizontalForceRatio = 0;
    private GameObject deerUnity;
    private bool isTrapped = false;
    private int countJumpsToEscape = 0;
    private float normalGravityScale = 0;
    public int direction = 1;
    private bool isCanMoving = true;
    private bool isInWind = false;
    private float windHorizontal = 0;
    private float windVertical = 0;
    private float windForceRatio = 0;
    public static bool isSmell = false;
    private GameObject tilemap1;
    private GameObject tilemap2;
    private bool isStacked = false;
    private int directionOfStack = 0;
    private int previousDirection = 1;
    private List<GameObject> allAnotherPlatforms = new List<GameObject>();
    private int shadowChecker;
    private float shadowDelay;
    private GameObject rightWallChecker;
    private GameObject leftWallChecker;
    private GameObject trapTriggerLeft;
    private GameObject trapTriggerRight;
    public GameObject CurrentActiveTrapTrigger;
    public GameObject currentLemmingArea;
    private float anotherHorForceRatio = 4;
    private float jumpForceRatio = 1f;
    //public bool isNeedToUpdatePlatformsList = false;
    public bool isInShadow { get; private set; }

    public RuntimeAnimatorController stayAnimation;
    public RuntimeAnimatorController walkAnimation;


    private bool isStayAni = true;
    private bool isWalkAni = false;

    public GameObject InputManager;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ambientLighting = GameObject.Find("DirectionalLight").GetComponent<Light>();
        deerLighting = GameObject.Find("DeerLight").GetComponent<Light>();
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();
        deerUnity = GameObject.Find("DeerUnity");
        normalGravityScale = rigidbody.gravityScale;
        shadowChecker = 0;
        isInShadow = false;
        shadowDelay = 0;
        tilemap1 = GameObject.Find("Tilemap1");
        tilemap2 = GameObject.Find("Tilemap2");

        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("CollapsingPlat"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("Platform"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("MaterialisedPlatform"));

        rightWallChecker = transform.Find("RightWallChecker").gameObject;
        leftWallChecker = transform.Find("LeftWallChecker").gameObject;

        trapTriggerLeft = transform.Find("TrapTriggerLeft").gameObject;
        trapTriggerRight = transform.Find("TrapTriggerRight").gameObject;
        CurrentActiveTrapTrigger = trapTriggerLeft;
}

    void FixedUpdate()
    {
        if (isCanMoving && isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 66));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TakeDamage(30f);
        if (deerUnity.GetComponent<DeerUnity>().isActivateCooling)
            CheckShadow();
        CheckIsStucked();
        MakeAction();
        FlipPlayer();

        if (isStayAni && horizontalForceRatio != 0 && CurrentHorizontalVelocity != 0 && DeerUnity.IsGrounded)
        {
            isStayAni = false;
            isWalkAni = true;
            GetComponent<Animator>().runtimeAnimatorController = walkAnimation;
        }
        else if (isWalkAni && (horizontalForceRatio == 0 || CurrentHorizontalVelocity == 0 || !DeerUnity.IsGrounded))
        {
            isStayAni = true;
            isWalkAni = false;
            GetComponent<Animator>().runtimeAnimatorController = stayAnimation;
        }
    }

    public void FlipPlayer()
    {
        if (direction < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            CurrentActiveTrapTrigger = trapTriggerRight;
        }
        if (direction > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            CurrentActiveTrapTrigger = trapTriggerLeft;
        }
    }

    public void SetShadowChecker(bool condition)
    {
        if (condition)
        {
            shadowChecker += 1;
        }
        else
        {
            shadowChecker -= 1;
        }
    }

    public void CheckShadow()
    {
        if (shadowChecker == 2)
        {
            shadowDelay = GetComponent<Timer>().GetTime();
            isInShadow = true;
        }
        else if(GetComponent<Timer>().GetTime() - shadowDelay > 0.5)
        {
            isInShadow = false;
        }
    }

    private void CheckIsStucked()
    {
        
        if (!isStacked) 
        {
            if (rightWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) || isTouchingAnythingElse(rightWallChecker)
                || rightWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>()))
            {
                directionOfStack = 1;
                isStacked = true;
            }
            if (leftWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) || isTouchingAnythingElse(leftWallChecker)
                || leftWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>()))
            {
                directionOfStack = -1;
                isStacked = true;
            }
        }
        else if (isStacked 
            && !rightWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) && !isTouchingAnythingElse(rightWallChecker)
            && !leftWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) && !isTouchingAnythingElse(leftWallChecker)
            && !rightWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>())
            && !leftWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>()))
        {
            isStacked = false;
            directionOfStack = 0;
        }
    }

    private bool isTouchingAnythingElse(GameObject e)
    {
        foreach(var obj in allAnotherPlatforms)
        {
            var vector = transform.position - obj.transform.position;
            if (vector.x < 10 && vector.x > -10 && vector.y > -10 && vector.y < 10)
            {
                var a = e.GetComponent<BoxCollider2D>();
                var b = obj.GetComponent<BoxCollider2D>();
                if (a == null)
                {

                }
                else if (b == null)
                {

                }
                else if (a.IsTouching(b))
                    return true;
            }
        }
        return false;
    }

    public void MakeAction()
    {
        if (InputManager.GetComponent<InputManager>().isJumpButtonPressed && DeerUnity.IsGrounded)
        {
            if (isCanMoving)
            {
                rigidbody.AddForce(new Vector2(0, 100 * jumpForceRatio));
                InputManager.GetComponent<InputManager>().isJumpButtonPressed = false;
            }
        }
        if (isTrapped && countJumpsToEscape > 0 && InputManager.GetComponent<InputManager>().isJumpButtonPressed)
        {
            InputManager.GetComponent<InputManager>().isJumpButtonPressed = false;
            countJumpsToEscape--;
            deerUnity.GetComponent<DeerUnity>().TakeDamage(2);
            if (countJumpsToEscape <= 0)
            {
                EscapedTrap();
            }
        }
        InputManager.GetComponent<InputManager>().isJumpButtonPressed = false;

        

        if (InputManager.GetComponent<InputManager>().isGoRightButtonPressed)
        {
            CurrentHorizontalVelocity += 4;
            InputManager.GetComponent<InputManager>().isGoRightButtonPressed = false;
            //horizontalForceRatio = 0;
        }
        if (InputManager.GetComponent<InputManager>().isGoRightButtonStopPress)
        {
            CurrentHorizontalVelocity += -4;
            InputManager.GetComponent<InputManager>().isGoRightButtonStopPress = false;
            //horizontalForceRatio = 0;
        }
        if (InputManager.GetComponent<InputManager>().isGoLeftButtonPressed)
        {
            CurrentHorizontalVelocity += -4;
            InputManager.GetComponent<InputManager>().isGoLeftButtonPressed = false;
            //horizontalForceRatio = 0;
        }
        if (InputManager.GetComponent<InputManager>().isGoLeftButtonStopPress)
        {
            CurrentHorizontalVelocity += 4;
            InputManager.GetComponent<InputManager>().isGoLeftButtonStopPress = false;
            //horizontalForceRatio = 0;
        }
        if (InputManager.GetComponent<InputManager>().isSecondAbilityButtonPressed)
        {
            isSmell = true;
            InputManager.GetComponent<InputManager>().isSecondAbilityButtonPressed = false;
            TurnOnScent(); // функци€ дл€ нюха, пока тут только затемнение экрана на некоторое врем€
            
            //Invoke("TurnOffScent", 3f); // функци€ вызывающа€ другую функцию, через заданный промежуток времени
        }
        if (InputManager.GetComponent<InputManager>().isSecondAbilityButtonStopPress)
        {
            InputManager.GetComponent<InputManager>().isSecondAbilityButtonStopPress = false;
            TurnOffScent();
        }
        if ((InputManager.GetComponent<InputManager>().isRunMode || isRunning) && deerUnity.GetComponent<DeerUnity>().currentStamina > 0 && DeerUnity.IsGrounded)
        {
            shiftRatio = 2;
            isRunning = true;
            deerUnity.GetComponent<DeerUnity>().isRunning = true;
        }
        if (!InputManager.GetComponent<InputManager>().isRunMode || !isRunning || (isRunning && deerUnity.GetComponent<DeerUnity>().currentStamina <= 0))
        {
            shiftRatio = 1;
            isRunning = false;
            deerUnity.GetComponent<DeerUnity>().isRunning = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && currentLemmingArea != null)
        {
            currentLemmingArea.GetComponent<CollectionLemming>().assembleLemming();
        }
        var previousDirection = direction;
        if (CurrentHorizontalVelocity > 0)
        {
            direction = 1;
        }
        else if (CurrentHorizontalVelocity < 0)
        {
            direction = -1;
        }
        if (previousDirection != direction)
        {
            horizontalForceRatio = 0;
        }

        if (!InputManager.GetComponent<InputManager>().isAnyMoveButtonPressing && CurrentHorizontalVelocity != 0)
        {
            CurrentHorizontalVelocity = 0;
        }

        if (GetComponent<Timer>().IsTicked())
        {
            if (horizontalForceRatio < 1 && CurrentHorizontalVelocity != 0)
            {
                horizontalForceRatio += 0.2f;
                if (horizontalForceRatio > 1)
                {
                    horizontalForceRatio = 1;
                }
            }
            if (horizontalForceRatio > 0 && CurrentHorizontalVelocity == 0)
            {
                horizontalForceRatio -= 0.33f;
                if (horizontalForceRatio < 0)
                {
                    horizontalForceRatio = 0;
                }
            }
            
            if (isInWind && windForceRatio < 1)
            {
                windForceRatio += 0.1f;
                if (windForceRatio >= 1)
                {
                    windForceRatio = 1;
                }
            }
            if (!isInWind && windForceRatio > 0)
            {
                windForceRatio -= 0.1f;
                if (windForceRatio <= 0)
                {
                    windForceRatio = 0;
                }
            }
        }

        if (isCanMoving)
        {
            if (!((directionOfStack == -1 && CurrentHorizontalVelocity < 0) || (directionOfStack == 1 && CurrentHorizontalVelocity > 0) ) )
            {
                if (CurrentHorizontalVelocity > -0.01f && CurrentHorizontalVelocity < 0.01f)
                {
                    horizontalForceRatio = 0;
                }
                var velocity = new Vector2(anotherHorForceRatio * direction * horizontalForceRatio * shiftRatio, rigidbody.velocity.y);
                if (isInWind)
                {
                    velocity += new Vector2((windForceRatio * windHorizontal) / 1, 0);
                }
                rigidbody.velocity = velocity;

                GameObject.Find("Info").GetComponent<Text>().text = velocity.ToString();
                //GameObject.Find("Info").GetComponent<Text>().text = (horizontalForceRatio).ToString();
            }
            else
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            /*if (isInWind)
            {
                rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 66));
            }*/
            
            CurrentVerticalVelocity = rigidbody.velocity.y;
        }
        else if (!isCanMoving)
        {
            rigidbody.velocity = new Vector2(0, 0);
        }
        
    }
    public void TurnOnScent() 
    {
        ambientLighting.intensity = 0.1f; //интенсивность фона на 0
        deerLighting.intensity = 1f; // интенсивность свечени€ олен€ на 1
    }
    public void TurnOffScent()
    {
        ambientLighting.intensity = 1; //интенсивность фона на 1, возвращение €ркости
        deerLighting.intensity = 0; //интенсивность свечени€ олен€ на 0, больше не светитс€
        isSmell = false;
    }

    public void SetHorizontalVelocity(float velocity)
    {
        CurrentHorizontalVelocity = velocity;
    }

    public void Trapped()
    {
        isTrapped = true;
        countJumpsToEscape = 5;
        StopMoving();
    }

    public void EscapedTrap()
    {
        countJumpsToEscape = 0;
        isTrapped = false;
        StartMoving();
    }

    public void StopMoving()
    {
        isCanMoving = false;
        normalGravityScale = rigidbody.gravityScale;
        rigidbody.gravityScale = 0;
        rigidbody.velocity = new Vector2(0, 0);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void StartMoving()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        rigidbody.gravityScale = normalGravityScale;
        isCanMoving = true;
    }

    public void InWind(float windHorizontalVelocity, float windVerticalForce)
    {
        isInWind = true;
        windHorizontal = windHorizontalVelocity;
        windVertical = windVerticalForce;
    }

    public void WindOut()
    {
        isInWind = false;
    }

    public void SlowDownMoving()
    {
        anotherHorForceRatio = 1f;
        jumpForceRatio = 0.33f;
        Invoke("UnSlowDownMoving", 1f);
    }

    private void UnSlowDownMoving()
    {
        anotherHorForceRatio = 4f;
        jumpForceRatio = 1f;
    }
}

