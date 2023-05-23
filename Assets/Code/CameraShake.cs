using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject shakeFX;
    public float shakeDur;
    private GameObject deerUnity;
    // Start is called before the first frame update
    void Start()
    {
        shakeFX.SetActive(false);
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        if ((DeerUnity.CurrentActive == 1 || DeerUnity.CurrentActive == 3) && DeerUnity.isShakeCamera)
        {
            DeerUnity.isShakeCamera = false;
            StopAllCoroutines();
            StartCoroutine(Shake(shakeDur));
        }
        else if ((DeerUnity.CurrentActive == 1 || DeerUnity.CurrentActive == 3) && DeerUnity.isShortShakeCamera)
        {
            DeerUnity.isShortShakeCamera = false;
            StopAllCoroutines();
            StartCoroutine(Shake(0.3f));
        }
    }

    IEnumerator Shake(float time)
    {
        shakeFX.SetActive(true);
        yield return new WaitForSeconds(time);
        shakeFX.SetActive(false);
        if (DeerUnity.CurrentActive == 1 && deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().countJumpsToEscape <= 0)
        {
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().EscapedTrap();
        }
        else if (DeerUnity.CurrentActive == 3 && deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerBig>().countJumpsToEscape <= 0)
        {
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerBig>().EscapedTrap();
        }
    }
}
