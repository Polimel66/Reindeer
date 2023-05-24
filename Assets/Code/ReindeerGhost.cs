using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class ReindeerGhost : MonoBehaviour 
{
    private enum jumpPhase
    {
        Up,
        Down,
        Land
    }
    public int CurrentHorizontalVelocity { get; private set; } = 0;
    public float CurrentVerticalVelocity { get; private set; } = 0;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    public float horizontalForceRatio = 1;
    private float shiftRatio = 1;
    public bool isRunning = false;
    private GameObject deerUnity;
    public int direction = 1;
    private bool isInWind = false;
    private float windHorizontal = 0;
    private float windVertical = 0;
    private float windForceRatio = 0;
    private int directionOfStack = 0;
    public static bool isWindProtected = false;
    private int protectionChecker;
    private float protectionDelay;

    private GameObject tilemap1;
    private bool isStacked = false;
    private int previousDirection = 1;
    private List<GameObject> allAnotherPlatforms = new List<GameObject>();
    public GameObject currendGhostPlatform;
    private bool isFlying;
    public bool isCanMater = true;
    private bool isIgnoreShift = false;

    //public RuntimeAnimatorController stayAnimation;
    //public RuntimeAnimatorController walkAnimation;


    //private bool isStayAni = true;
    //private bool isWalkAni = false;

    //public bool isGrounded = true;
    public GameObject InputManager;

    private bool isStayAni = true;
    private bool isWalkAni = false;

    //private string[] allSpecificIdleAnies = new string[] { "IdleEar", "IdleStomp", "IdleTail", "HeadTilt" };
    //private string basicIdleAni = "IdleBasic";
    private string dieAni = "Die";
    private string levitacia = "Levitacia";
    private string joggingAni = "Walk";
    private string runAni = "Beg";
    private Dictionary<jumpPhase, string> jumpFhaseAnies = new Dictionary<jumpPhase, string>()
    {
        { jumpPhase.Up, "JumpUp" },
        { jumpPhase.Down, "JumpDown" },
        { jumpPhase.Land, "JumpLand" }
    };
    private float stayTime = 0;
    private float timeToWait;
    private bool isPlayingSpecificIdle = false;
    private bool isPlayingDieAnimation = false;
    private bool isPlayingJumpAnimation = false;
    private bool isPlayingLevitaciaAnimation = false;
    private jumpPhase currentJumpPhase = jumpPhase.Up;
    //public GameObject jumpLandTrigger;
    private bool isPlayingFallAnimation = false;
    public GameObject animation;
    private bool isCoorChangedForFlying = true;
    private bool isCoorChangedForNormal = true;
    private int movingButtonsNotPressingFramesCounter = 0;
    private GameObject rightWallChecker;
    private GameObject leftWallChecker;
    private GameObject tilemap2;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();
        deerUnity = GameObject.Find("DeerUnity");
        protectionChecker = 0;
        protectionDelay = 0;
        tilemap1 = GameObject.Find("Tilemap1");
        tilemap2 = GameObject.Find("Tilemap2");

        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("CollapsingPlat"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("Platform"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("GhostPlatform"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("MaterialisedPlatform"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("EmergIsland"));

        rightWallChecker = transform.Find("RightWallChecker").gameObject;
        leftWallChecker = transform.Find("LeftWallChecker").gameObject;
    }

    void FixedUpdate()
    {
        if (isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 150));
        }
    }

    void Update()
    {
        CheckIsStucked();
        MakeAction();
        FlipPlayer();
        CheckProtect();

        CheckAnimation();

        UpdateJumpAnimation();

        UpdateFallAnimation();

        

        if (!isCoorChangedForFlying)// && animation.GetComponent<SkeletonAnimation>().AnimationName.Equals(levitacia))
        {
            if (direction < 0)
            {
                animation.GetComponent<Transform>().localScale = new Vector3(-0.28f, 0.28f, 0.28f);
                animation.GetComponent<Transform>().localPosition = new Vector3(1.76f, -3.19f, 0);
                //CurrentActiveTrapTrigger = trapTriggerRight;
            }
            if (direction > 0)
            {
                //spriteRenderer.flipX = false;
                animation.GetComponent<Transform>().localScale = new Vector3(0.28f, 0.28f, 0.28f);
                animation.GetComponent<Transform>().localPosition = new Vector3(-1.76f, -3.19f, 0);
                //CurrentActiveTrapTrigger = trapTriggerLeft;
            }
            isCoorChangedForFlying = true;
            //isCoorChangedForNormal = false;
        }
        if (!isCoorChangedForNormal)// && !animation.GetComponent<SkeletonAnimation>().AnimationName.Equals(levitacia))
        {
            if (direction < 0)
            {
                animation.GetComponent<Transform>().localScale = new Vector3(-0.28f, 0.28f, 0.28f);
                animation.GetComponent<Transform>().localPosition = new Vector3(0.65f, -2.3f, 0);
                //CurrentActiveTrapTrigger = trapTriggerRight;
            }
            if (direction > 0)
            {
                //spriteRenderer.flipX = false;
                animation.GetComponent<Transform>().localScale = new Vector3(0.28f, 0.28f, 0.28f);
                animation.GetComponent<Transform>().localPosition = new Vector3(-0.65f, -2.3f, 0);
                //CurrentActiveTrapTrigger = trapTriggerLeft;
            }
            isCoorChangedForNormal = true;
            //isCoorChangedForFlying = false;
        }
    }

    private void CheckAnimation()
    {

        if (!isPlayingDieAnimation && !isPlayingJumpAnimation && !isPlayingLevitaciaAnimation)
        {
            if (isStayAni && horizontalForceRatio != 0 && CurrentHorizontalVelocity != 0 && DeerUnity.IsGrounded)
            {
                isStayAni = false;
                isWalkAni = true;
                //SetAnimation(joggingAni);
            }
            else if (isWalkAni && (horizontalForceRatio == 0 || CurrentHorizontalVelocity == 0 || !DeerUnity.IsGrounded))
            {
                isStayAni = true;
                isWalkAni = false;
                //SetAnimation(basicIdleAni);
                SetAnimation(null);
                animation.GetComponent<SkeletonAnimation>().ClearState();
                stayTime = 0;
            }

            if (isStayAni)
            {
                stayTime += Time.deltaTime;
                if (stayTime > 5 && !isPlayingSpecificIdle)
                {
                    //PlayRandomIdleAnimation();
                    isPlayingSpecificIdle = true;
                }
            }
            if (isWalkAni)
            {
                if (isRunning)
                {
                    SetAnimation(runAni);
                }
                else
                {
                    SetAnimation(joggingAni);
                }
            }

            if (CurrentVerticalVelocity < -1f && !isPlayingFallAnimation)
            {
                PlayFallAnimation();

            }
        }

    }

    private void SetAnimation(string name)
    {
        animation.GetComponent<SkeletonAnimation>().AnimationName = name;

    }

    /*private void PlayRandomIdleAnimation()
    {
        var r = Random.Range(0, 3.33f);
        //var r = 3;
        //animation.GetComponent<SkeletonAnimation>().loop = false;
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
        animation.GetComponent<SkeletonAnimation>().AnimationName = basicIdleAni;
    }*/

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
            if (currentJumpPhase == jumpPhase.Up && CurrentVerticalVelocity < -0.05f)
            {

                //jumpLandTrigger.GetComponent<JumpLandTrigger>().isNearToGround = false;
                PlayJumpDownAnimation();
            }
            if (currentJumpPhase == jumpPhase.Down && DeerUnity.IsGrounded)
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
        animation.GetComponent<SkeletonAnimation>().loop = false;
        animation.GetComponent<SkeletonAnimation>().AnimationName = dieAni;
        Invoke("StopPlayingDieAnimation", 1.1f);
    }

    private void StopPlayingDieAnimation()
    {
        isPlayingDieAnimation = false;
    }

    public void setIsWindProtected(bool condition)
    {
        if (condition)
        {
            protectionChecker += 1;
        }
        else
        {
            protectionChecker -= 1;
        }
    }

    public void CheckProtect()
    {
        if (protectionChecker == 2)
        {
            protectionDelay = GetComponent<Timer>().GetTime();
            isWindProtected = true;
            DeerUnity.isProtected = true;
            if (GameObject.Find("LiftingWind").GetComponent<Wind>().totalForce == 25)
            {
                GameObject.Find("LiftingWind").GetComponent<Wind>().totalForce = 0;
            }
        }
        else if (GetComponent<Timer>().GetTime() - protectionDelay > 0.5)
        {
            DeerUnity.isProtected = false;
            isWindProtected = false;
            if (GameObject.Find("LiftingWind").GetComponent<Wind>().totalForce == 0)
            {
                GameObject.Find("LiftingWind").GetComponent<Wind>().totalForce = 25;
            }
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
        foreach (var obj in allAnotherPlatforms)
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
            rigidbody.AddForce(new Vector2(0, 45));
            InputManager.GetComponent<InputManager>().isJumpButtonPressed = false;

            PlayJumpAnimation();
        }
        if (InputManager.GetComponent<InputManager>().isSecondAbilityButtonPressed && deerUnity.GetComponent<DeerUnity>().isSecondAbilityGhostAvailable)
        {
            isFlying = true;

            //isCoorChangedForFlying = false;
            

            InputManager.GetComponent<InputManager>().isSecondAbilityButtonPressed = false;
        }
        if (isFlying && rigidbody.velocity.y <= -0.1f)
        {
            //rigidbody.gravityScale = 0.1f;
            if(rigidbody.velocity.y < -2f)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -2f);
            }
            if (!isPlayingLevitaciaAnimation)
            {
                StopJumpAnimation();
                SetAnimation(levitacia);
                isPlayingLevitaciaAnimation = true;
                isCoorChangedForFlying = false;
            }
        }
        if (isFlying && rigidbody.velocity.y >= 0)
        {
            rigidbody.gravityScale = 1f;
        }
        if (!isFlying)
        {
            rigidbody.gravityScale = 1f;
        }
        if (isFlying && (InputManager.GetComponent<InputManager>().isSecondAbilityButtonStopPress || DeerUnity.IsGrounded))
        {
            rigidbody.gravityScale = 1f;
            isFlying = false;

            //isCoorChangedForNormal = false;
            //SetAnimation(null);
            //animation.GetComponent<SkeletonAnimation>().ClearState();
            isPlayingFallAnimation = false;
            isPlayingLevitaciaAnimation = false;
            
            isCoorChangedForNormal = false;
            

            InputManager.GetComponent<InputManager>().isSecondAbilityButtonStopPress = false;
        }

        if (InputManager.GetComponent<InputManager>().isGoRightButtonPressed && InputManager.GetComponent<InputManager>().isGoLeftButtonPressed)
        {
            CurrentHorizontalVelocity = 0;
            //InputManager.GetComponent<InputManager>().isGoRightButtonPressed = false;
            //horizontalForceRatio = 0;
        }
        else if (InputManager.GetComponent<InputManager>().isGoRightButtonPressed)
        {
            CurrentHorizontalVelocity = 4;
        }
        else if (InputManager.GetComponent<InputManager>().isGoLeftButtonPressed)
        {
            CurrentHorizontalVelocity = -4;
        }
        else
        {
            CurrentHorizontalVelocity = 0;
        }
        /*if (InputManager.GetComponent<InputManager>().isGoRightButtonStopPress)//���� ��������� ������, ��������� �������������� �������� ����
        {
            CurrentHorizontalVelocity += -4;
            InputManager.GetComponent<InputManager>().isGoRightButtonStopPress = false;
            //horizontalForceRatio = 0;
        }*/
        /*if (InputManager.GetComponent<InputManager>().isGoLeftButtonPressed)//���� ������ �����, ��������� �������������� �������� �����
        {
            CurrentHorizontalVelocity = -4;
            InputManager.GetComponent<InputManager>().isGoLeftButtonPressed = false;
            //horizontalForceRatio = 0;
        }*/
        if (InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed && currendGhostPlatform != null && isCanMater && deerUnity.GetComponent<DeerUnity>().isFirstAbilityGhostAvailable)//���� ������ �����, ��������� �������������� �������� �����
        {
            currendGhostPlatform.GetComponent<Materialization>().makeMaterialisation();
            isCanMater = false;
            InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed = false;
        }
        /*if (InputManager.GetComponent<InputManager>().isGoLeftButtonStopPress)//���� ��������� �����, ��������� �������������� �������� ������
        {
            CurrentHorizontalVelocity += 4;
            InputManager.GetComponent<InputManager>().isGoLeftButtonStopPress = false;
            //horizontalForceRatio = 0;
        }*/
        if ((InputManager.GetComponent<InputManager>().isRunMode || isRunning) && deerUnity.GetComponent<DeerUnity>().currentStamina > 0.25 && DeerUnity.IsGrounded)
        {
            shiftRatio = 1.5f;
            isRunning = true;
            deerUnity.GetComponent<DeerUnity>().isRunning = true;
        }
        if (!InputManager.GetComponent<InputManager>().isRunMode || !isRunning || deerUnity.GetComponent<DeerUnity>().currentStamina <= 0)
        {
            shiftRatio = 1;
            isRunning = false;
            deerUnity.GetComponent<DeerUnity>().isRunning = false;
            InputManager.GetComponent<InputManager>().isRunMode = false;
        }
        if (DeerUnity.IsGrounded)
        {
            rigidbody.gravityScale = 1f;
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

        /*if (!InputManager.GetComponent<InputManager>().isAnyMoveButtonPressing && CurrentHorizontalVelocity != 0)
        {
            CurrentHorizontalVelocity = 0;
        }*/

        //if (!InputManager.GetComponent<InputManager>().isGoLeftButtonStopPress && !InputManager.GetComponent<InputManager>().isGoRightButtonStopPress && CurrentHorizontalVelocity != 0)
        //{
        //    CurrentHorizontalVelocity = 0;
        //}

        if (!InputManager.GetComponent<InputManager>().isAnyMoveButtonPressing && Mathf.Abs(CurrentHorizontalVelocity) > 1)
        {
            movingButtonsNotPressingFramesCounter++;
            if (movingButtonsNotPressingFramesCounter > 5)
            {
                CurrentHorizontalVelocity = 0;
                movingButtonsNotPressingFramesCounter = 0;
            }
        }
        else
        {
            movingButtonsNotPressingFramesCounter = 0;
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

            if (isInWind && windForceRatio != 1)
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
        if (!((directionOfStack == -1 && CurrentHorizontalVelocity < 0) || (directionOfStack == 1 && CurrentHorizontalVelocity > 0)))
        {
            if (CurrentHorizontalVelocity > -0.01f && CurrentHorizontalVelocity < 0.01f)
            {
                horizontalForceRatio = 0;
            }
            var velocity = new Vector2(4 * direction * horizontalForceRatio * shiftRatio, rigidbody.velocity.y);
            if (isInWind)
            {
                if (isIgnoreShift)
                {
                    velocity += new Vector2((windForceRatio * windHorizontal * shiftRatio * 7) / 1, 0);
                }
                else
                {
                    velocity += new Vector2((windForceRatio * windHorizontal * 7) / 1, 0);
                }
            }
            rigidbody.velocity = velocity;
        }
        else
        {
            var velocity = new Vector2(0, rigidbody.velocity.y);
            if (isInWind)
            {
                if (isIgnoreShift)
                {
                    velocity += new Vector2((windForceRatio * windHorizontal * shiftRatio * 7) / 1, 0);
                }
                else
                {
                    velocity += new Vector2((windForceRatio * windHorizontal * 7) / 1, 0);
                }

            }
            rigidbody.velocity = velocity;
        }
        /*if (isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 150));
        }*/

        CurrentVerticalVelocity = rigidbody.velocity.y;
        //� ����� ���������� �������������� �������������� ��������, �� �� � �����������, �������� ������������ �������� ��� ��
        /*�� ���� ��� � ������, �� ����������� �������� �������, ��� ��������� ����, ���� ���� ���� � �������������� ��������.
         * ��������� �������� ����� ���� ��� � ������ ����� � ���� �������� ���������, � ��� ���� ����� ������������ � ��������� � ������ ������� ������ update(), �.�. ���� �������� ��� ����������� ����������.
         * ������ ����������� ������� ����� ���������� ����, � �� ����� ���������� ��������, ������ � ��� �� ����.
         */
    }

    public void FlipPlayer()
    {
        if (isFlying)
        {
            if (direction < 0 && animation.GetComponent<Transform>().localScale.x > 0)
            {
                animation.GetComponent<Transform>().localScale = new Vector3(-0.28f, 0.28f, 0.28f);
                animation.GetComponent<Transform>().localPosition = new Vector3(1.76f, -3.19f, 0);
                //CurrentActiveTrapTrigger = trapTriggerRight;
            }
            if (direction > 0 && animation.GetComponent<Transform>().localScale.x < 0)
            {
                //spriteRenderer.flipX = false;
                animation.GetComponent<Transform>().localScale = new Vector3(0.28f, 0.28f, 0.28f);
                animation.GetComponent<Transform>().localPosition = new Vector3(-1.76f, -3.19f, 0);
                //CurrentActiveTrapTrigger = trapTriggerLeft;
            }
        }
        else
        {
            if (direction < 0 && animation.GetComponent<Transform>().localScale.x > 0)
            {
                animation.GetComponent<Transform>().localScale = new Vector3(-0.28f, 0.28f, 0.28f);
                animation.GetComponent<Transform>().localPosition = new Vector3(0.65f, -2.3f, 0);
                //CurrentActiveTrapTrigger = trapTriggerRight;
            }
            if (direction > 0 && animation.GetComponent<Transform>().localScale.x < 0)
            {
                //spriteRenderer.flipX = false;
                animation.GetComponent<Transform>().localScale = new Vector3(0.28f, 0.28f, 0.28f);
                animation.GetComponent<Transform>().localPosition = new Vector3(-0.65f, -2.3f, 0);
                //CurrentActiveTrapTrigger = trapTriggerLeft;
            }
        }
        
    }

    public void SetHorizontalVelocity(int velocity)
    {
        CurrentHorizontalVelocity = velocity;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("thorns"))
        {
            deerUnity.GetComponent<DeerUnity>().TakeDamage(1000);
        }
    }
}
