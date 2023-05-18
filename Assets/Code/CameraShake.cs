using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject shakeFX;
    public float shakeDur;
    // Start is called before the first frame update
    void Start()
    {
        shakeFX.SetActive(false);
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
            StartCoroutine(Shake(0.2f));
        }
    }

    IEnumerator Shake(float time)
    {
        shakeFX.SetActive(true);
        yield return new WaitForSeconds(time);
        shakeFX.SetActive(false);
    }
}
