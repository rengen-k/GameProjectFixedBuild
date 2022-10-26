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
 
    }

    public override void triggerAct(){
        if (isTrigger){
            isTrigger = false;
            Debug.Log("rotate");
            transform.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
        }
    }

    public override void triggerUnAct()
    {
        isTrigger = true;
    }
}
