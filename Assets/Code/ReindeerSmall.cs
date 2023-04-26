using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReindeerSmall : MonoBehaviour
{
    private enum jumpPhase{
        Up,
        Down,
        Land
    }
    // Start is called before the first frame update
    public float CurrentHorizontalVelocity { get; private set; } = 0;
    public float CurrentVerticalVelocity { get; private set; } = 0;
    private Rigidbody2D rigidbody;
    //private SpriteRenderer spriteRenderer;
    public GameObject animation;
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
    //public GameObject mossArea;
    private float anotherHorForceRatio = 4;
    private float jumpForceRatio = 1f;
    private bool isIgnoreShift = false;
    private GameObject[] snowDrifts;
    //public bool isNeedToUpdatePlatformsList = false;
    public bool isInShadow { get; private set; }

    //public RuntimeAnimatorController stayAnimation;
    //public RuntimeAnimatorController walkAnimation;


    private bool isStayAni = true;
    private bool isWalkAni = false;

    public GameObject InputManager;
    private string currentAniName = "";
    private string[] allSpecificIdleAnies = new string[] { "IdleStomp", "IdleStomp", "IdleStomp", "IdleStomp" };
    private string basicIdleAni = "IdleBasic";
    private string dieAni = "DieTest";
    private string joggingAni = "jogging";
    private string runAni = "RunBasic";
    private Dictionary<jumpPhase, string> jumpFhaseAnies = new Dictionary<jumpPhase, string>()
    {
        { jumpPhase.Up, "JumpBasicUp" },
        { jumpPhase.Down, "JumpBasicDown" },
        { jumpPhase.Land, "JumpBasicLand" }
    };
    private float stayTime = 0;
    private float timeToWait;
    private bool isPlayingSpecificIdle = false;
    private bool isPlayingDieAnimation = false;
    private bool isPlayingJumpAnimation = false;
    private jumpPhase currentJumpPhase = jumpPhase.Up;
    public GameObject jumpLandTrigger;
    private bool isPlayingFallAnimation = false;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
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
        snowDrifts = GameObject.FindGameObjectsWithTag("SnowDrift");

}

    void FixedUpdate()
    {
        if (isCanMoving && isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 66));
        }
        CheckIsInSnowDrift();
        if (deerUnity.GetComponent<DeerUnity>().isActivateCooling)
            CheckShadow();
        CheckIsStucked();
        FlipPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //TakeDamage(30f);
        
        MakeAction();

        CheckAnimation();

        UpdateJumpAnimation();

        UpdateFallAnimation();
    }

    private void CheckAnimation()
    {
        if (!isPlayingDieAnimation && !isPlayingJumpAnimation)
        {
            if (isStayAni && horizontalForceRatio != 0 && CurrentHorizontalVelocity != 0 && DeerUnity.IsGrounded)
            {
                isStayAni = false;
                isWalkAni = true;
                //SetAnimation(joggingAni);
                currentAniName = joggingAni;
            }
            else if (isWalkAni && (horizontalForceRatio == 0 || CurrentHorizontalVelocity == 0) && DeerUnity.IsGrounded)
            {
                isStayAni = true;
                isWalkAni = false;
                SetAnimation(basicIdleAni);
                currentAniName = basicIdleAni;
                stayTime = 0;
            }

            if (isStayAni)
            {
                stayTime += Time.deltaTime;
                if (stayTime > 5 && !isPlayingSpecificIdle)
                {
                    PlayRandomIdleAnimation();
                    isPlayingSpecificIdle = true;
                }
            }
            if (isWalkAni && !isPlayingJumpAnimation && DeerUnity.IsGrounded)
            {
                if (isRunning)
                {
                    SetAnimation(runAni);
                    currentAniName = runAni;
                }
                else
                {
                    SetAnimation(joggingAni);
                    currentAniName = joggingAni;
                }
            }

            if(CurrentVerticalVelocity < -1f && !isPlayingFallAnimation && !isTrapped)
            {
                PlayFallAnimation();
                
            }
        }
    }

    private void SetAnimation(string name)
    {
        animation.GetComponent<SkeletonAnimation>().AnimationName = name;
        
    }

    private void PlayRandomIdleAnimation()
    {
        var r = Random.Range(0, 3.33f);
        //var r = 3;
        //animation.GetComponent<SkeletonAnimation>().loop = false;
        animation.GetComponent<SkeletonAnimation>().loop = false;
        if (DeerUnity.IsGrounded)
            animation.GetComponent<SkeletonAnimation>().AnimationName = allSpecificIdleAnies[(int)r];
        
        timeToWait = 2f;
        if ((int)r == 3)
            timeToWait = 4.4f;
        Invoke("PlayBasicIdleAnimation", timeToWait);
    }

    private void PlayBasicIdleAnimation()
    {
        isPlayingDieAnimation = false;
        stayTime = 0;
        isPlayingSpecificIdle = false;

        if (DeerUnity.IsGrounded)
            animation.GetComponent<SkeletonAnimation>().AnimationName = basicIdleAni;
        animation.GetComponent<SkeletonAnimation>().loop = true;
    }

    private void PlayJumpAnimation()
    {
        isPlayingJumpAnimation = true;
        
        animation.GetComponent<SkeletonAnimation>().loop = false;
        PlayJumpUpAnimation();
    }

    private void UpdateJumpAnimation()
    {
        if (isPlayingJumpAnimation)
        {
            if(currentJumpPhase == jumpPhase.Up && CurrentVerticalVelocity < 0)
            {
                
                //jumpLandTrigger.GetComponent<JumpLandTrigger>().isNearToGround = false;
                PlayJumpDownAnimation();
            }
            if(currentJumpPhase == jumpPhase.Down && DeerUnity.IsGrounded)
            {
                
                PlayJumpLandAnimation();
            }
        }
    }

    private void PlayJumpUpAnimation()
    {
        currentJumpPhase = jumpPhase.Up;
        animation.GetComponent<SkeletonAnimation>().AnimationName = jumpFhaseAnies[currentJumpPhase];
        animation.GetComponent<SkeletonAnimation>().timeScale = 1.5f;
    }

    private void PlayJumpDownAnimation()
    {
        currentJumpPhase = jumpPhase.Down;
        animation.GetComponent<SkeletonAnimation>().AnimationName = jumpFhaseAnies[currentJumpPhase];
        animation.GetComponent<SkeletonAnimation>().timeScale = 1.5f;
    }

    private void PlayJumpLandAnimation()
    {
        currentJumpPhase = jumpPhase.Land;
        animation.GetComponent<SkeletonAnimation>().AnimationName = jumpFhaseAnies[currentJumpPhase];
        currentJumpPhase = jumpPhase.Up;
        animation.GetComponent<SkeletonAnimation>().timeScale = 3;
        var timeToWait = 0.3f / animation.GetComponent<SkeletonAnimation>().timeScale;
        Invoke("StopJumpAnimation", timeToWait);
    }

    private void StopJumpAnimation()
    {
        isPlayingJumpAnimation = false;
        isPlayingFallAnimation = false;
        animation.GetComponent<SkeletonAnimation>().loop = true;
        animation.GetComponent<SkeletonAnimation>().timeScale = 1;
    }

    private void PlayFallAnimation()
    {
        isPlayingFallAnimation = true;
        animation.GetComponent<SkeletonAnimation>().loop = false;
        PlayJumpDownAnimation();
    }

    private void UpdateFallAnimation()
    {
        if (isPlayingFallAnimation)
        {
            if (currentJumpPhase == jumpPhase.Down && DeerUnity.IsGrounded)
            {
                PlayJumpLandAnimation();
            }
        }
    }

    public void PlayDieAnimation()
    {
        StopJumpAnimation();
        isPlayingDieAnimation = true;
        animation.GetComponent<SkeletonAnimation>().AnimationName = dieAni;
        Invoke("PlayBasicIdleAnimation", 1.1f);
    }

    public void FlipPlayer()
    {
        if (direction < 0 && animation.GetComponent<Transform>().localScale.x > 0)
        {
            animation.GetComponent<Transform>().localScale = new Vector3(-0.027f, 0.027f, 0.027f);
            animation.GetComponent<Transform>().localPosition = new Vector3(0.15f, -0.61f, 0);
            CurrentActiveTrapTrigger = trapTriggerRight;
        }
        if (direction > 0 && animation.GetComponent<Transform>().localScale.x < 0)
        {
            //spriteRenderer.flipX = false;
            animation.GetComponent<Transform>().localScale = new Vector3(0.027f, 0.027f, 0.027f);
            animation.GetComponent<Transform>().localPosition = new Vector3(-0.15f, -0.61f, 0);
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

    private void CheckIsInSnowDrift()
    {
        var isInSnow = false;
        foreach(var snowDrift in snowDrifts)
        {
            if (snowDrift.GetComponent<BoxCollider2D>().IsTouching(GetComponent<BoxCollider2D>()))
            {
                isInSnow = true;
                break;
            }
        }
        if (isInSnow && DeerUnity.IsGrounded)
        {
            SnowIn();
        }
        else if(!isInSnow && DeerUnity.IsGrounded)
        {
            SnowOut();
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

                PlayJumpAnimation();
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
        if (InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed && currentLemmingArea != null)
        {
            currentLemmingArea.GetComponent<CollectionLemming>().assembleLemming();
            deerUnity.GetComponent<DeerUnity>().SetTask(10);
            DeerUnity.countOfFoundLemmings += 1;

        }
        if (InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed)
        {
            DeerUnity.isPossibleTakeMoss = true;
            DeerUnity.isPossibleTakeLemming = true;
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
                    if (isIgnoreShift)
                    {
                        velocity += new Vector2((windForceRatio * windHorizontal * shiftRatio) / 1, 0);
                    }
                    else
                    {
                        velocity += new Vector2((windForceRatio * windHorizontal) / 1, 0);
                    }

                }
                rigidbody.velocity = velocity;

                //GameObject.Find("Info").GetComponent<Text>().text = velocity.ToString();
                //GameObject.Find("Info").GetComponent<Text>().text = (horizontalForceRatio).ToString();
            }
            else
            {
                var velocity = new Vector2(0, rigidbody.velocity.y);
                if (isInWind)
                {
                    if (isIgnoreShift)
                    {
                        velocity += new Vector2((windForceRatio * windHorizontal * shiftRatio) / 1, 0);
                    }
                    else
                    {
                        velocity += new Vector2((windForceRatio * windHorizontal) / 1, 0);
                    }

                }
                rigidbody.velocity = velocity;
            }
            /*if (isInWind)
            {
                rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 66));
            }*/
            
            CurrentVerticalVelocity = rigidbody.velocity.y;
        }
        else if (!isCanMoving)
        {
            //rigidbody.velocity = new Vector2(0, 0);
        }
        
    }
    public void TurnOnScent() 
    {
        ambientLighting.intensity = 0.1f; //интенсивность фона на 0
        deerLighting.intensity = 1f; // интенсивность свечени€ олен€ на 1
    }
    public void TurnOffScent()
    {
        if (DeerUnity.isBlackoutNow == true)
            ambientLighting.intensity = 0.1f;
        else
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
        //rigidbody.gravityScale = 3;
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        //GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void StartMoving()
    {
        //GetComponent<BoxCollider2D>().isTrigger = false;
        //rigidbody.gravityScale = normalGravityScale;
        isCanMoving = true;
    }

    public void InWind(float windHorizontalVelocity, float windVerticalForce, bool isIgnoreShift)
    {
        isInWind = true;
        this.isIgnoreShift = isIgnoreShift;
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

    public void SnowIn()
    {
        anotherHorForceRatio = 2f;
        jumpForceRatio = 0.75f;
    }

    public void SnowOut()
    {
        anotherHorForceRatio = 4f;
        jumpForceRatio = 1f;
    }
}

