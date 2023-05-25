using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startPosition;
    private Queue<GameObject> queue;
    private bool isStartCloning;
    private int counter;
    private int littleCounter;
    public int neededCount;
    void Start()
    {
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.05f);
        GetComponent<Timer>().StartTimer();
        isStartCloning = true;
        counter = 0;
        littleCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Timer>().IsTicked() && counter < neededCount)
        {
            if (littleCounter % 2 == 0)
            {
 
                var thord_clone = GameObject.Instantiate(this.gameObject.transform.Find("CircleStone (2)").gameObject, startPosition.transform.position, transform.rotation);
                var second_clone = GameObject.Instantiate(this.gameObject.transform.Find("CircleStone (1)").gameObject, startPosition.transform.position, transform.rotation);
                counter += 2;
            }
            littleCounter += 1;
            var clone = GameObject.Instantiate(this.gameObject.transform.Find("CircleStone").gameObject, startPosition.transform.position, transform.rotation);
            counter += 1;
        }
    }
}
