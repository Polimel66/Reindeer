using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private GameObject deerUnity;
    private bool isTriggeredByDeer = false;
    private bool isTriggeredByHunter = false;
    private float windHorizontalVelocity = 0;
    private float windVerticalForce = 0;
    public float totalForce = 10;
    private float previousTotalForce;
    public bool isWorking = true;
    private GameObject hunter;
    public bool isPeriodically = true;
    private float alphaRatio = 0;
    public float periodForTick;
    public AudioClip windSound;
    private AudioSource audio;
    private bool isPlayingSound = false;
    public bool isWithSound = false;
    public bool isGradient = true;
    private bool isFullVisible;
    private bool isWithCollider = true;
    public bool isIgnoreShift = false;
    public bool isNeedToChangeHearingRadius = true;
    private bool isWithAudioSource;
    public bool isSimpleWind = false;
    public GameObject windWithParticles;
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    //public bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        if(windWithParticles != null)
        {
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    particleSystems.Add(windWithParticles.transform.GetChild(i).GetChild(j).Find("Particle System (2)").gameObject.GetComponent<ParticleSystem>());
                }
            }
        }
        
        

        BoxCollider2D a;
        isWithCollider = TryGetComponent<BoxCollider2D>(out a);
        deerUnity = GameObject.Find("DeerUnity");
        hunter = GameObject.Find("Hunter");
        previousTotalForce = totalForce;
        gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(periodForTick);
        GetComponent<Timer>().StartTimer();
        isWithAudioSource = TryGetComponent<AudioSource>(out audio);
        if (isNeedToChangeHearingRadius && isWithAudioSource)
        {
            audio.volume = 1f;
            audio.maxDistance = 25;
            audio.spatialBlend = 1;
            audio.rolloffMode = AudioRolloffMode.Linear;
        }
        if (isWithCollider)
        {
            CalculateForces();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSimpleWind)
        {
            DoWorkAsSimpleWind();
        }
        else
        {
            if (isPeriodically && GetComponent<Timer>().IsTicked())
            {
                isWorking = !isWorking;
            }

            if (isWorking && alphaRatio != 1)
            {
                alphaRatio += Time.deltaTime;
                if (alphaRatio > 1 && !isFullVisible)
                {
                    alphaRatio = 1;
                    isFullVisible = true;
                    
                }
                if (isGradient)
                {
                    GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                }

                if (isWithSound && isWorking && alphaRatio > 0.5f && !isPlayingSound && isWithAudioSource)
                {
                    isPlayingSound = true;
                    var distance = Mathf.Sqrt((deerUnity.transform.position.x - transform.position.x) * (deerUnity.transform.position.x - transform.position.x)
                        + (deerUnity.transform.position.y - transform.position.y) * (deerUnity.transform.position.y - transform.position.y));
                    if (distance < audio.maxDistance)
                    {
                        if (DeerUnity.VolumeRatio == 0)
                        {
                            audio.volume = 0;
                        }
                        else
                        {
                            audio.volume = 1;
                        }
                        audio.PlayOneShot(windSound);
                        //PlayWindAni();
                    }
                }
            }

            if (!isWorking && isPlayingSound)
            {
                isPlayingSound = false;
            }

            if (!isWorking && alphaRatio != 0)
            {
                if(alphaRatio <= 0.5f)
                    isFullVisible = false;
                alphaRatio -= Time.deltaTime;
                if (alphaRatio < 0)
                {
                    alphaRatio = 0;
                }
                if (isGradient)
                {
                    GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                }
            }

            if (isWithCollider)
            {

                if (isFullVisible && !isTriggeredByDeer && GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
                {
                    isTriggeredByDeer = true;
                    deerUnity.GetComponent<DeerUnity>().StartBlowing(windHorizontalVelocity, windVerticalForce, isIgnoreShift);
                }
                else if (isTriggeredByDeer && !GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
                {
                    isTriggeredByDeer = false;
                    deerUnity.GetComponent<DeerUnity>().StopBlowing();
                }
                else if (!isFullVisible && isTriggeredByDeer)
                {
                    isTriggeredByDeer = false;
                    deerUnity.GetComponent<DeerUnity>().StopBlowing();
                }
                if (DeerUnity.isProtected && gameObject.name == "LiftingWind")
                    deerUnity.GetComponent<DeerUnity>().StopBlowing();
                if (previousTotalForce != totalForce)
                {
                    previousTotalForce = totalForce;
                    CalculateForces();
                }

                if (isFullVisible && !isTriggeredByHunter && GetComponent<BoxCollider2D>().IsTouching(hunter.GetComponent<BoxCollider2D>()))
                {
                    isTriggeredByHunter = true;
                    hunter.GetComponent<Hunter>().InWind(windHorizontalVelocity, windVerticalForce);
                }
                else if (isTriggeredByHunter && !GetComponent<BoxCollider2D>().IsTouching(hunter.GetComponent<BoxCollider2D>()))
                {
                    isTriggeredByHunter = false;
                    hunter.GetComponent<Hunter>().WindOut();
                }
                else if (!isFullVisible && isTriggeredByHunter)
                {
                    isTriggeredByHunter = false;
                    hunter.GetComponent<Hunter>().WindOut();
                }
            }
        }
        if(windWithParticles != null)
            UpdateWindAniAlpha();
    }

    private void CalculateForces()
    {
        var rotation = transform.localRotation.eulerAngles.z;
        rotation %= 360;

        if (rotation > 180)
            rotation -= 360;
        if (rotation < -180)
            rotation += 360;

        var vKatet = 0f;
        var hKatet = 0f;

        if (rotation >= 0 && rotation < 90)
        {
            vKatet = totalForce * Mathf.Sin((rotation * Mathf.PI) / 180);
            hKatet = Mathf.Sqrt(totalForce * totalForce - vKatet * vKatet);
        }
        else if (rotation >= 90 && rotation <= 180)
        {
            vKatet = totalForce * Mathf.Sin((rotation * Mathf.PI) / 180);
            hKatet = -Mathf.Sqrt(totalForce * totalForce - vKatet * vKatet);
        }
        else if (rotation >= -90 && rotation < 0)
        {
            vKatet = -totalForce * Mathf.Sin((Mathf.Abs(rotation) * Mathf.PI) / 180);
            hKatet = Mathf.Sqrt(totalForce * totalForce - vKatet * vKatet);
        }
        else if (rotation >= -180 && rotation < -90)
        {
            vKatet = -totalForce * Mathf.Sin((Mathf.Abs(rotation) * Mathf.PI) / 180);
            hKatet = -Mathf.Sqrt(totalForce * totalForce - vKatet * vKatet);
        }
        windVerticalForce = vKatet;
        windHorizontalVelocity = hKatet;
    }

    private void DoWorkAsSimpleWind()
    {
        if (isWorking)
        {

            if (!isTriggeredByDeer && GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
            {
                isTriggeredByDeer = true;
                deerUnity.GetComponent<DeerUnity>().StartBlowing(windHorizontalVelocity, windVerticalForce, isIgnoreShift);
            }
            else if (isTriggeredByDeer && !GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
            {
                isTriggeredByDeer = false;
                deerUnity.GetComponent<DeerUnity>().StopBlowing();
            }
            if (previousTotalForce != totalForce)
            {
                previousTotalForce = totalForce;
                CalculateForces();
            }
        }
        else
        {
            if (isTriggeredByDeer)
            {
                isTriggeredByDeer = false;
                deerUnity.GetComponent<DeerUnity>().StopBlowing();
            }
        }
    }

    private void PlayWindAni()
    {
        foreach(var particleSystem in particleSystems)
        {
            particleSystem.Play();
        }
    }

    private void StopWindAni()
    {
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }
    }

    private void UpdateWindAniAlpha()
    {
        foreach (var particleSystem in particleSystems)
        {
            //var main = particleSystem.main.startColor;
            //main.mode = ParticleSystemGradientMode.TwoColors;
            //main.colorMax = Color.red;
            //main.colorMax = Color.blue;

            var rrr = particleSystem.main;
            rrr.startColor = new ParticleSystem.MinMaxGradient(new Color(1, 1, 1, alphaRatio), new Color(0.7215686f, 0.9647059f, 1, alphaRatio));
        }
    }
}
