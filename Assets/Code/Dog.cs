using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    private enum jumpPhase
    {
        Up,
        Down,
        Land
    }
    public float CurrentHorizontalVelocity { get; private set; } = 0;
    public float CurrentVerticalVelocity { get; private set; } = 0;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    public float horizontalForceRatio = 1;
    private int shiftRatio = 1;
    public bool isRunning = false;
    private GameObject deerUnity;
    public int direction = 1;
    private bool isInWind = false;
    private float windHorizontal = 0;
    private float windVertical = 0;
    private float windForceRatio = 0;
    private int directionOfStack = 0;
    private GameObject tilemap1;
    private GameObject tilemap2;
    private bool isStacked = false;
    private List<GameObject> allAnotherPlatforms = new List<GameObject>();
    private float previousTime;
    private bool isGrounded = true;
    private bool isCanMoving = true;
    private float previousHorizontalVelocity;
    private float standingTime = 0;
    public HunterMode mode;
    private bool isAlreadyStanded = false;

    private GameObject rightWallChecker;
    private GameObject leftWallChecker;
    private bool isStayAtPoint = false;
    public bool isEnabled;
    private HunterMode previousHunterMode = HunterMode.Searching;

    private bool isStayAni = true;
    private bool isWalkAni = false;

    //private string[] allSpecificIdleAnies = new string[] { "IdleEar", "IdleStomp", "IdleTail", "HeadTilt" };
    private string basicIdleAni = "Standing";
    //private string dieAni = "DieTest";
    //private string levitacia = "Levitacia";
    //private string joggingAni = "Walking";
    private string runAni = "run";
    private Dictionary<jumpPhase, string> jumpFhaseAnies = new Dictionary<jumpPhase, string>()
    {
        { jumpPhase.Up, "jumpUp" },
        { jumpPhase.Down, "jumpMid" },
        { jumpPhase.Land, "jumpDown" }
    };
    private float stayTime = 0;
    private float timeToWait;
    //private bool isPlayingSpecificIdle = false;
    //private bool isPlayingDieAnimation = false;
    private bool isPlayingJumpAnimation = false;
    //private bool isPlayingLevitaciaAnimation = false;
    private jumpPhase currentJumpPhase = jumpPhase.Up;
    //public GameObject jumpLandTrigger;
    private bool isPlayingFallAnimation = false;
    public GameObject animation;
    private float t = 0;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();
        deerUnity = GameObject.Find("DeerUnity");

        tilemap1 = GameObject.Find("Tilemap1");
        tilemap2 = GameObject.Find("Tilemap2");

        //allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("CollapsingPlat"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("Platform"));

        rightWallChecker = transform.Find("RightWallChecker").gameObject;
        leftWallChecker = transform.Find("LeftWallChecker").gameObject;

        //mode = HunterMode.Searching;
    }

    void Update()
    {
        t += Time.deltaTime;
        CheckIsStucked();
        MakeAction();
        FlipPlayer();

        if (previousHunterMode == HunterMode.Searching && mode == HunterMode.Chasing)
        {
            deerUnity.GetComponent<DeerUnity>().PlayHunterTheme();
        }
        previousHunterMode = mode;
        isGrounded = transform.Find("Ground").GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>())
            || transform.Find("Ground").GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>());


        CheckAnimation();

        UpdateJumpAnimation();

        UpdateFallAnimation();

        if (direction < 0)
        {
            animation.GetComponent<Transform>().localScale = new Vector3(-0.015f, 0.015f, 0f);

            /*if (isWalkAni)
            {
                handsRotatingPoint.transform.localPosition = new Vector3(-0.085f, 0.0740011f, 0);
            }
            else
            {
                handsRotatingPoint.transform.localPosition = new Vector3(0.048f, 0.0740011f, 0);
            }

            handsRotatingPoint.transform.localScale = new Vector3(-1, 1, 1);*/
            //animation.GetComponent<Transform>().localPosition = new Vector3(1.76f, -3.19f, 0);
            //CurrentActiveTrapTrigger = trapTriggerRight;
        }
        if (direction > 0)
        {
            //spriteRenderer.flipX = false;
            animation.GetComponent<Transform>().localScale = new Vector3(0.015f, 0.015f, 0f);

            /*if (isWalkAni)
            {
                handsRotatingPoint.transform.localPosition = new Vector3(-0.215f, 0.0740011f, 0);
            }
            else
            {
                handsRotatingPoint.transform.localPosition = new Vector3(-0.3629858f, 0.0740011f, 0);
            }

            handsRotatingPoint.transform.localScale = new Vector3(1, 1, 1);*/
            //animation.GetComponent<Transform>().localPosition = new Vector3(-1.76f, -3.19f, 0);
            //CurrentActiveTrapTrigger = trapTriggerLeft;
        }
    }

    private void CheckAnimation()
    {
        //GameObject.Find("Info").GetComponent<Text>().text = animation.GetComponent<SkeletonAnimation>().AnimationName;
        if (!isPlayingJumpAnimation && !isPlayingFallAnimation)
        {
            if (isStayAni && horizontalForceRatio != 0 && CurrentHorizontalVelocity != 0 && isGrounded)
            {
                isStayAni = false;
                isWalkAni = true;
                //SetAnimation(joggingAni);
            }
            else if (isWalkAni && (horizontalForceRatio == 0 || CurrentHorizontalVelocity == 0 || !isGrounded))
            {
                isStayAni = true;
                isWalkAni = false;
                //SetAnimation(basicIdleAni);
                //SetAnimation(null);
                //animation.GetComponent<SkeletonAnimation>().ClearState();
                stayTime = 0;
            }

            if (isStayAni)
            {
                //SetAnimation(basicIdleAni, 1f);
                animation.GetComponent<SkeletonAnimation>().ClearState();
                //animation.GetComponent<SkeletonAnimation>().AnimationName = null;
            }
            if (isWalkAni)
            {
                if (isRunning)
                {
                    SetAnimation(runAni, 2f);
                }
                else
                {
                    SetAnimation(runAni, 2f);
                }
            }

            if (GetComponent<Rigidbody2D>().velocity.y < -0.1f && !isPlayingFallAnimation)
            {
                PlayFallAnimation();
            }
        }

    }

    private void SetAnimation(string name)
    {
        animation.GetComponent<SkeletonAnimation>().AnimationName = name;
    }

    private void SetAnimation(string name, float timeScale)
    {
        animation.GetComponent<SkeletonAnimation>().AnimationName = name;
        animation.GetComponent<SkeletonAnimation>().timeScale = timeScale;
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
        //GameObject.Find("Info").GetComponent<Text>().text = isGrounded.ToString();
        if (isPlayingJumpAnimation)
        {
            if (currentJumpPhase == jumpPhase.Up && GetComponent<Rigidbody2D>().velocity.y < -0.05f)
            {

                //jumpLandTrigger.GetComponent<JumpLandTrigger>().isNearToGround = false;
                PlayJumpDownAnimation();
            }
            if (currentJumpPhase == jumpPhase.Down && isGrounded)
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
            if (currentJumpPhase == jumpPhase.Down && isGrounded)
            {
                PlayJumpLandAnimation();
            }
        }
    }

    private void CheckIsStucked()
    {
        if (!isStacked && !isGrounded
            && (GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>())
            || GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>()) || isTouchingAnythingElse()))
        {
            directionOfStack = direction;
            isStacked = true;
        }
        else if (isStacked
            && (isGrounded
            || (!GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>())
            && !GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>()) && !isTouchingAnythingElse())))
        {
            isStacked = false;
            directionOfStack = 0;
        }
    }

    private bool isTouchingAnythingElse()
    {
        foreach (var obj in allAnotherPlatforms)
        {
            var vector = transform.position - obj.transform.position;
            if (vector.x < 10 && vector.x > -10 && vector.y > -10 && vector.y < 10)
            {
                if (GetComponent<BoxCollider2D>().IsTouching(obj.GetComponent<BoxCollider2D>()))
                    return true;
            }
        }
        return false;
    }

    public void MakeAction()
    {
        if (mode == HunterMode.Searching)
        {

        }
        else if (mode == HunterMode.Chasing)
        {
            var deltaX = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.x - transform.position.x;
            var deltaY = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.y - transform.position.y;
            if (deltaX > 0)
            {
                direction = 1;
            }
            else if (deltaX < 0)
            {
                direction = -1;
            }
            if (deltaX > 0.5 && isCanMoving && !isStayAtPoint)
            {
                GoRight();
            }
            else if (deltaX < -0.5 && isCanMoving && !isStayAtPoint)
            {
                GoLeft();
            }
            else if (deltaX > -0.5 && deltaX < 0.5)
            {
                StopMoving();
            }

            if ((deltaX > 10 || deltaX < -10) && !isStayAtPoint)
            {
                Run();
            }
            else if (isRunning)
            {
                StopRunning();
            }

            

            if (deltaX < 0.5 && deltaX > -0.5 && !deerUnity.GetComponent<DeerUnity>().isCatched && !deerUnity.GetComponent<DeerUnity>().isBited
                && deltaY < 0.5 && deltaY > -0.5)
            {
                //deerUnity.GetComponent<DeerUnity>().CatchDeer();
                deerUnity.GetComponent<DeerUnity>().Bite();
            }
        }


        if ((direction > 0 && rightWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()))
            || (direction < 0 && leftWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()))
            || (direction > 0 && rightWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>()))
            || (direction < 0 && leftWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap2.GetComponent<CompositeCollider2D>())))
        {
            if (previousTime == 0 && t > 1f)
            {
                t = 0;
                isCanMoving = false;
                StopMoving();
                Jump();
                PlayJumpAnimation();
                previousTime = GetComponent<Timer>().GetTime();
            }
            if (previousTime != 0 && GetComponent<Timer>().GetTime() - previousTime > 1)
            {
                t = 0;
                Jump();
                PlayJumpAnimation();
                previousTime = GetComponent<Timer>().GetTime();
            }
        }
        else if (!isCanMoving)
        {
            isCanMoving = true;
            StartMoving();
            previousTime = 0;
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
            var velocity = new Vector2(7 * direction * horizontalForceRatio * shiftRatio, rigidbody.velocity.y);
            if (isInWind)
            {
                velocity += new Vector2((windForceRatio * windHorizontal) / 5, 0);
            }
            rigidbody.velocity = velocity;
        }
        if (isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 25));
        }
    }

    public void StayAtPoint(Transform tr)
    {
        mode = HunterMode.Chasing;
        isStayAtPoint = true;
        StopMoving();
        transform.position = tr.position;
    }

    public void HuntDeerAtPoint(Transform tr)
    {
        mode = HunterMode.Chasing;
        isStayAtPoint = false;
        transform.position = tr.position;
    }

    private void GoRight()
    {
        CurrentHorizontalVelocity = 4;
        previousHorizontalVelocity = CurrentHorizontalVelocity;
    }

    private void GoLeft()
    {
        CurrentHorizontalVelocity = -4;
        previousHorizontalVelocity = CurrentHorizontalVelocity;
    }

    public void StopMoving()
    {
        previousHorizontalVelocity = CurrentHorizontalVelocity;
        CurrentHorizontalVelocity = 0;
        //isCanMoving = false;
    }

    public void StartMoving()
    {
        CurrentHorizontalVelocity = previousHorizontalVelocity;
        //isCanMoving = true;
    }

    private void Jump()
    {
        rigidbody.AddForce(new Vector2(0, 250));
    }

    private void Run()
    {
        shiftRatio = 2;
        isRunning = true;
    }

    private void StopRunning()
    {
        shiftRatio = 1;
        isRunning = false;
    }

    public void FlipPlayer()
    {
        if (direction < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
        if (direction > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
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

    public void DisableHunter()
    {
        deerUnity.GetComponent<DeerUnity>().PlayMainTheme();
    }
}
