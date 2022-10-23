using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevel1Puzzle : Triggerable
{
    private bool isTrigger;
    private Quaternion nextRotation;

    public float speed = 0.1f; //speed in degrees per second?
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger) {
            // transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(0, 90 ,0), 20 * Time.deltaTime);
            transform.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
                isTrigger = false;
        }
        
    }

    public override void triggerAct(){
        Debug.Log("Puzzle activated");
        isTrigger = true;
    }
}
