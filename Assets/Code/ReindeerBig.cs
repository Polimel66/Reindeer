using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReindeerBig : MonoBehaviour //большой олень. Пока полностью совпадает с призрачным, кроме сил передвижения. Подробно все описано в призрачном
{
    public float CurrentHorizontalVelocity { get; private set; } = 0;
    public float CurrentVerticalVelocity { get; private set; } = 0;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    public float horizontalForceRatio = 1;
    private int shiftRatio = 1;
    public bool isRunning = false;
    private GameObject deerUnity;
    private bool isTrapped = false;
    private int countJumpsToEscape = 0;
    private float normalGravityScale = 0;
    public int direction = 1;
    //public bool isGrounded = true;
    private bool isInWind = false;
    private float windHorizontal = 0;
    private float windVertical = 0;
    private float windForceRatio = 0;
    private int directionOfStack = 0;
    private GameObject tilemap1;
    private bool isStacked = false;
    private int previousDirection = 1;
    private List<GameObject> allAnotherPlatforms = new List<GameObject>();
    public int lungeImpulse;
    public static bool isLunge;
    private GameObject trapTriggerLeft;
    private GameObject trapTriggerRight;
    public GameObject CurrentActiveTrapTrigger;
    private bool isFlipLocked = false;
    private int directionOfLudge = 0;
    private float anotherHorForceRatio = 4;
    private List<GameObject> collapsingPlatforms = new List<GameObject>();
    private List<GameObject> moveObjects = new List<GameObject>();
    private bool isMovingObject = false;
    private HingeJoint2D joint;
    private bool isCanJump = true;
    private bool isIgnoreShift;

    //public RuntimeAnimatorController stayAnimation;
    //public RuntimeAnimatorController walkAnimation;


    //private bool isStayAni = true;
    //private bool isWalkAni = false;
    public GameObject InputManager;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();
        deerUnity = GameObject.Find("DeerUnity");

        tilemap1 = GameObject.Find("Tilemap1");

        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("CollapsingPlat"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("Platform"));
        lungeImpulse = 1000;
        isLunge = false;

        trapTriggerLeft = transform.Find("TrapTriggerLeft").gameObject;
        trapTriggerRight = transform.Find("TrapTriggerRight").gameObject;
        CurrentActiveTrapTrigger = trapTriggerLeft;
        collapsingPlatforms.AddRange(GameObject.FindGameObjectsWithTag("CollapsingPlat"));
        moveObjects.AddRange(GameObject.FindGameObjectsWithTag("MoveObject"));
        joint = GetComponent<HingeJoint2D>();
        joint.enabled = false;
    }

    void FixedUpdate()
    {
        if (isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 25));
        }
    }

    void Update()
    {
        LungeToDestroid();
        CheckIsStucked();
        MakeAction();
        FlipPlayer();
        MoveObject();

        if (isLunge)
        {
            BoxCollider2D coll;
            if (directionOfLudge > 0)
            {
                coll = transform.Find("RightWallChecker").gameObject.GetComponent<BoxCollider2D>();
            }
            else
            {
                coll = transform.Find("LeftWallChecker").gameObject.GetComponent<BoxCollider2D>();
            }
            foreach (var e in collapsingPlatforms)
            {
                if (coll.IsTouching(e.GetComponent<BoxCollider2D>()))
                {
                    e.SetActive(false);
                }
            }
        }
    }

    private void CheckIsStucked()
    {
        if (!isStacked && !DeerUnity.IsGrounded
            && (GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) || isTouchingAnythingElse()))
        {
            directionOfStack = direction;
            isStacked = true;
        }
        else if (isStacked
            && (DeerUnity.IsGrounded
            || (!GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) && !isTouchingAnythingElse())))
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
        if (InputManager.GetComponent<InputManager>().isJumpButtonPressed && DeerUnity.IsGrounded && isCanJump)
        {
            if (!isTrapped)
            {
                rigidbody.AddForce(new Vector2(0, 240));
            }
        }
        if (InputManager.GetComponent<InputManager>().isJumpButtonPressed && isCanJump)
        {
            if (isTrapped && countJumpsToEscape > 0)
            {
                countJumpsToEscape--;
                deerUnity.GetComponent<DeerUnity>().TakeDamage(10);
                if (countJumpsToEscape <= 0)
                {
                    EscapedTrap();
                }
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

        //if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && CurrentHorizontalVelocity != 0 )
        //{
        //    CurrentHorizontalVelocity = 0;
        //}

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
        
        if (!isTrapped)
        {
            if (!((directionOfStack == -1 && CurrentHorizontalVelocity < 0) || (directionOfStack == 1 && CurrentHorizontalVelocity > 0)))
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
                        velocity += new Vector2((windForceRatio * windHorizontal * shiftRatio) / 5, 0);
                    }
                    else
                    {
                        velocity += new Vector2((windForceRatio * windHorizontal) / 5, 0);
                    }
                }
                rigidbody.velocity = velocity;
            }
           /* if (isInWind)
            {
                rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 25));
            }*/
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
            CurrentVerticalVelocity = rigidbody.velocity.y;
        }
        else if (isTrapped)
        {
            rigidbody.velocity = new Vector2(0, 0);
        }
    }

    private void MoveObject()
    {
        if (InputManager.GetComponent<InputManager>().isSecondAbilityButtonPressed)
        {
            InputManager.GetComponent<InputManager>().isSecondAbilityButtonPressed = false;
            if (!isMovingObject)
            {
                BoxCollider2D coll;
                if (direction > 0)
                {
                    coll = transform.Find("RightWallChecker").gameObject.GetComponent<BoxCollider2D>();
                }
                else
                {
                    coll = transform.Find("LeftWallChecker").gameObject.GetComponent<BoxCollider2D>();
                }
                foreach (var e in moveObjects)
                {
                    if (coll.IsTouching(e.GetComponent<BoxCollider2D>()))
                    {
                        isCanJump = false;
                        joint.enabled = true;
                        isMovingObject = true;
                        isFlipLocked = true;
                        var rigid = e.GetComponent<Rigidbody2D>();
                        rigid.mass = 10;
                        joint.connectedBody = rigid;
                        rigidbody.velocity = new Vector2(0, 0);
                    }
                }
            }
        }
        if (InputManager.GetComponent<InputManager>().isSecondAbilityButtonStopPress)
        {
            InputManager.GetComponent<InputManager>().isSecondAbilityButtonStopPress = false;
            if (isMovingObject)
            {
                isMovingObject = false;
                isFlipLocked = false;
                joint.connectedBody.mass = 1000;
                joint.connectedBody = null;
                joint.enabled = false;
                isCanJump = true;
            }
        }
    }

    public void LungeToDestroid()
    {
        if (InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed)
        {
            InputManager.GetComponent<InputManager>().isFirstAbilityButtonPressed = false;
            if (!isLunge && DeerUnity.IsGrounded)
            {
                directionOfLudge = direction;
                rigidbody.velocity = new Vector2(0, 0);
                isLunge = true;
                //Invoke("ChangeLunge", 3f);
                isFlipLocked = true;
                DoFirstStepLunge();
            }
        }
    }

    private void DoFirstStepLunge()
    {
        CurrentHorizontalVelocity = 4 * directionOfLudge * -1;
        Invoke("DoSecondStepLunge", 0.5f);
    }

    private void DoSecondStepLunge()
    {
        CurrentHorizontalVelocity = 0;
        Invoke("DoThirdStepLunge", 0.1f);
    }

    private void DoThirdStepLunge()
    {
        CurrentHorizontalVelocity = 4 * directionOfLudge;
        anotherHorForceRatio = 50;
        Invoke("DoFourthStepLunge", 0.33f);
    }

    private void DoFourthStepLunge()
    {
        CurrentHorizontalVelocity = 0;
        anotherHorForceRatio = 4;
        isFlipLocked = false;
        Invoke("RefreshLunge", 3f);
    }

    private void RefreshLunge()
    {
        isLunge = false;
    }

    public void FlipPlayer()
    {
        if (!isFlipLocked)
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
    }

    public void SetVerticalVelocity(float velocity)
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, velocity);
    }

    /*public static void SetGorizontalForce()
    {

    }*/

    public void SetHorizontalVelocity(float velocity)
    {
        CurrentHorizontalVelocity = velocity;
    }

    public void Trapped()
    {
        isTrapped = true;
        countJumpsToEscape = 5;
        normalGravityScale = rigidbody.gravityScale;
        rigidbody.gravityScale = 0;
        rigidbody.velocity = new Vector2(0, 0);
    }

    public void EscapedTrap()
    {
        countJumpsToEscape = 0;
        isTrapped = false;
        rigidbody.gravityScale = normalGravityScale;
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
}
